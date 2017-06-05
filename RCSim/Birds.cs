using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Objects;
using Bonsai.Core.Interfaces;
using Bonsai.Objects.Meshes;
using Microsoft.DirectX;

namespace RCSim
{
    internal class Birds : IDisposable
    {
        internal class Bird : GameObject
        {
            public int Number;
            public Vector3 Velocity;
            public Vector3 Acceleration;
            public Vector3 Target;
            public List<Bird> Birds = null;
            public bool Update = true;
            public bool Scared = false;
            public float UpdateElapsed = 0f;
            private float roll = 0;
            private float speed = 3f;
            private float acc = 3f;

            
            public Bird(int number)
            {
                this.Number = number;
                AnimatedXMesh mesh = new AnimatedXMesh("data/bird.x");
                mesh.GameObject = this;
                mesh.SetTrackSpeed(0, 2.9f + ((number % 10) * 0.2f));
                this.Position = new Vector3((number % 10), 5f + 0.5f*number%6, ((number + 5) % 10) * number/20f + 50f);
                this.Velocity = new Vector3(1, 0, 0);
                this.Mesh = mesh;
                this.Scale = new Vector3(0.0005f, 0.0005f, 0.0005f);
            }

            public override void  Dispose()
            {
 	            base.Dispose();
            }

            public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime, int targetBird)
            {
                Scared = false;
                if (Update)
                {
                    speed = 3f;
                    acc = 3f;
                    Acceleration = Target - Position;
                    if (Number == targetBird)
                    {
                        if ((Target.Y == 0.0f) && (Acceleration.LengthSq() > 100))
                        {
                            Acceleration = new Vector3(Target.X, 10f, Target.Z) - Position;
                        }
                    }
                    else
                    {
                        foreach (Bird bird in Birds)
                        {
                            if (bird.Number != this.Number)
                            {
                                Vector3 vDistance = bird.Position - Position;
                                if (vDistance.LengthSq() < 0.5f)
                                {
                                    Acceleration += Position - bird.Position;
                                }
                            }
                        }
                    }

                    if (RCSim.Birds.ScareCrow != null)
                    {
                        Vector3 vDistance = RCSim.Birds.ScareCrow.Position - Position;
                        if (vDistance.LengthSq() < 100f)
                        {
                            Acceleration = Position - RCSim.Birds.ScareCrow.Position;
                            if (Position.Y < 1.0f)
                                Acceleration.Y = 1.0f;
                            speed = 6f;
                            acc = 6f;
                            Scared = true;
                        }
                    }
                    Acceleration.Normalize();                    
                }
                this.Velocity += acc * Acceleration * elapsedTime;
                Velocity.Normalize();
                this.Position += elapsedTime * speed * Velocity;
                if (Position.Y < 0)
                    Position = new Vector3(Position.X, 0, Position.Z);
                if (Update)
                {
                    // Determine roll
                    Vector3 left = Vector3.Cross(Velocity, Up);
                    roll -= (Vector3.Dot(left, Acceleration) + roll) * UpdateElapsed;
                    if (roll < -1) roll = -1f;
                    else if (roll > 1) roll = 1f;
                    this.YawPitchRoll = new Vector3((float)Math.Atan2(Velocity.Z, -Velocity.X) + (float)Math.PI / 2, Velocity.Y / speed, roll);
                    Update = false;
                }
                //this.RotateYAngle = (float)Math.Atan2(Velocity.Z , -Velocity.X) + (float)Math.PI/2;
                base.OnFrameMove(device, totalTime, elapsedTime);
            }
        }

        #region Private fields
        private List<Bird> birds = new List<Bird>();
        private double lastUpdate = -10.0;
        private double lastMoveUpdate = -10.0;
        private Random rnd = new Random();
        private int nBirds = 100;
        private int targetBird = 0;
        #endregion

        /// <summary>
        /// Gets/Sets the scarecrow
        /// </summary>
        public static GameObject ScareCrow
        {
            get;
            set;
        }


        public bool Random
        {
            get;
            set;
        }

        public bool TargetReached
        {
            get { return (birds[targetBird].Position - birds[targetBird].Target).LengthSq() < 2.0f; }
        }

        public bool Scared
        {
            get;
            set;
        }

        #region Constructor
        public Birds(int nBirds)
        {
            this.nBirds = nBirds;
            for (int i = 0; i < nBirds; i++)
            {
                birds.Add(new Bird(i));
            }
            foreach (Bird bird in birds)
                bird.Birds = birds;
            targetBird = 0;
            birds[0].Target = new Vector3(0f, 10f, 20f);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            foreach (Bird bird in birds)
            {
                bird.Dispose();                
            }
            birds.Clear();
        }
        #endregion

        #region Public methods
        public void SetRandomTarget()
        {
            targetBird = rnd.Next(nBirds);
            birds[targetBird].Target = new Vector3((float)rnd.Next(200) - 100f, (float)(rnd.Next(5, 10)), (float)rnd.Next(200) - 100f);
        }

        public void SetTarget(Vector3 target)
        {
            targetBird = rnd.Next(nBirds);
            birds[targetBird].Target = target;
        }
        #endregion

        #region IFrameworkCallback Members
        public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            int nScared = 0;
            if (Random && (totalTime > lastUpdate + 10.0))
            {
                SetRandomTarget();
                lastUpdate = totalTime;
            }
            if (totalTime > lastMoveUpdate + 0.1f)
            {                
                foreach (Bird bird in birds)
                {
                    bird.Update = true;
                    bird.UpdateElapsed = (float)(totalTime - lastMoveUpdate);
                }
                lastMoveUpdate = totalTime;
            }
            foreach (Bird bird in birds)
            {
                if (bird.Number != targetBird)
                    bird.Target = birds[targetBird].Position;
                bird.OnFrameMove(device, totalTime, elapsedTime, targetBird);
                if (bird.Scared)
                    nScared++;
            }
            if (nScared > nBirds / 2)
                Scared = true;
            else
                Scared = false;
        }

        public void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            foreach (Bird bird in birds)
                bird.OnFrameRender(device, totalTime, elapsedTime);
        }
        #endregion

       
    }
}
