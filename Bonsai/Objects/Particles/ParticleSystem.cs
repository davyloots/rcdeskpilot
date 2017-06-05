using System;
using System.Collections.Generic;
using Bonsai.Core;
using Bonsai.Core.Interfaces;
using Bonsai.Objects;
using Bonsai.Objects.Meshes;
using Microsoft.DirectX.Direct3D;
using Bonsai.Objects.Textures;
using Microsoft.DirectX;


namespace Bonsai.Objects.Particles
{
    public class ParticleSystem : GameObject
    {
        #region Private fields
        private Queue<Particle> deadParticles = new Queue<Particle>();
        private Queue<Particle> aliveParticles = new Queue<Particle>();
        private Vector3 windVector = new Vector3();
        private int nMaxParticles = 100;
        private double lifeTime = 3.0;
        private TransparentObjectManager transparentObjectManager = null;
        private SquareMesh mesh = null;
        private double lastCreationTime = 0.0;
        private bool emitting = true;
        private int intensity = 100;
        private Vector3 variation = new Vector3();
        private Random random = new Random();
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/Sets the maximum number of particles in the system.
        /// </summary>
        public int NMaxParticles
        {
            get { return nMaxParticles; }
            set 
            { 
                nMaxParticles = value;
                Initialize();
            }
        }

        /// <summary>
        /// Gets/sets the lifetime of a particle.
        /// </summary>
        public double LifeTime
        {
            get { return lifeTime; }
            set { lifeTime = value; }
        }

        /// <summary>
        /// Gets/sets the texture to use on the particle.
        /// </summary>
        public TextureBase Texture
        {
            get { return mesh.Texture; }
            set { mesh.Texture = value; }
        }

        /// <summary>
        /// Gets/Sets whether new particles should be emitted.
        /// </summary>
        public bool Emitting
        {
            get { return emitting; }
            set { emitting = value; }
        }

        /// <summary>
        /// Gets/sets the wind vector that will move the particles.
        /// </summary>
        public Vector3 WindVector
        {
            get { return windVector; }
            set { windVector = value; }
        }

        /// <summary>
        /// Gets/sets the intensity of the emitted particle [0, +100].
        /// </summary>
        public int Intensity
        {
            get { return intensity; }
            set { intensity = value; }
        }

        /// <summary>
        /// Gets/sets the variation in 3 dimensions for the location of new particles.
        /// </summary>
        public Vector3 Variation
        {
            get { return variation; }
            set { variation = value; }
        }
        #endregion

        #region Constructor
        public ParticleSystem(TransparentObjectManager manager)
        {
            this.transparentObjectManager = manager;
            this.VisibleChanged += new EventHandler(ParticleSystem_VisibleChanged);
            mesh = new SquareMesh(0.2f, 1, 1, 1.0f);
            Initialize();
        }
        #endregion

        #region Dispose
        public override void Dispose()
        {
            foreach (Particle particle in aliveParticles)
            {
                transparentObjectManager.Objects.Remove(particle);
            }
            base.Dispose();
        }
        #endregion

        #region Private event handlers
        /// <summary>
        /// Handles the VisibleChanged event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ParticleSystem_VisibleChanged(object sender, EventArgs e)
        {
            if (visible)
            {
                foreach (Particle particle in aliveParticles)
                {
                    transparentObjectManager.Objects.Add(particle);
                }
            }
            else
            {
                foreach (Particle particle in aliveParticles)
                {
                    transparentObjectManager.Objects.Remove(particle);
                }
            }
        }
        #endregion

        #region Private methods
        private void Initialize()
        {
            foreach (Particle particle in aliveParticles)
            {
                transparentObjectManager.Objects.Remove(particle);
            }
            aliveParticles.Clear();
            deadParticles.Clear();
            for (int i = 0; i < nMaxParticles; i++)
            {
                Particle particle = new Particle(mesh);
                deadParticles.Enqueue(particle);
            }
        }

        /// <summary>
        /// Creates a new particle at the current position.
        /// </summary>
        private void CreateParticle(double creationTime)
        {
            Particle newParticle = deadParticles.Dequeue();
            if (newParticle != null)
            {
                aliveParticles.Enqueue(newParticle);
                newParticle.WorldPosition = this.WorldPosition + GenerateVariation();
                newParticle.Intensity = this.Intensity;
                newParticle.Create(creationTime, lifeTime, this);
                transparentObjectManager.Objects.Add(newParticle);
                lastCreationTime = creationTime;
            }
        }

        /// <summary>
        /// Generates a random vector within Variation boundaries.
        /// </summary>
        /// <returns></returns>
        private Vector3 GenerateVariation()
        {
            Vector3 result = new Vector3();
            if (variation.X != 0)
            {
                int vx = random.Next(-100, 100);
                result.X = result.X + vx*variation.X/100;
            }
            if (variation.Y != 0)
            {
                int vy = random.Next(-100, 100);
                result.Y = result.Y + vy*variation.Y/100;
            }
            if (variation.Z != 0)
            {
                int vz = random.Next(-100, 100);
                result.Z = result.Z + vz*variation.Z/100;
            }
            return result;
        }

        /// <summary>
        /// Destroys the oldest particle.
        /// </summary>
        private void DestroyParticle()
        {
            Particle oldParticle = aliveParticles.Dequeue();
            if (oldParticle != null)
            {
                oldParticle.Destroy();
                deadParticles.Enqueue(oldParticle);
                transparentObjectManager.Objects.Remove(oldParticle);
            }
        }
        #endregion

        #region Overridden GameObject methods
        /// <summary>
        /// Prepares the next frame.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="totalTime"></param>
        /// <param name="elapsedTime"></param>
        public override void OnFrameMove(Device device, double totalTime, float elapsedTime)
        {
            try
            {
                if (aliveParticles.Count > nMaxParticles - 1)
                    DestroyParticle();
                if ((emitting) && (totalTime - lastCreationTime > lifeTime / nMaxParticles))
                    CreateParticle(totalTime);
                base.OnFrameMove(device, totalTime, elapsedTime);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("ParticleSystem.OnFrameMove : " + ex.ToString());
            }
        }

        /// <summary>
        /// Renders the particle system.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="totalTime"></param>
        /// <param name="elapsedTime"></param>
        public override void OnFrameRender(Device device, double totalTime, float elapsedTime)
        {
            base.OnFrameRender(device, totalTime, elapsedTime);
        }
        #endregion
    }
}
