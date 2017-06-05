using System;
using System.Collections.Generic;
using Bonsai.Core.Interfaces;
using Microsoft.DirectX;
using Bonsai.Objects.Meshes;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core;
using System.Drawing;

namespace Bonsai.Objects.Particles
{
    public class Particle : ITransparentObject
    {
        #region Private fields
        private Vector3 position;
        private SquareMesh mesh;
        private Vector3 scale = new Vector3(1,1,1);
        private bool alive = false;
        private Matrix transformMatrix = new Matrix();
        private double creationTime;
        private double lifeTime;
        private int intensity = 100;
        private int number;
        private static Vector3 up = new Vector3(0, 1, 0);
        private static int totalNumber = 0;
        private static Material material = new Material();
        private static Material defaultMaterial = new Material();
        private int random = 0;
        private ParticleSystem particleSystem = null;
        private static Random rnd = new Random();
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/sets whether the particle is alive.
        /// </summary>
        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }

        /// <summary>
        /// Gets/sets the intensity particle [0, +100].
        /// </summary>
        public int Intensity
        {
            get { return intensity; }
            set { intensity = value; }
        }
        #endregion

        #region ITransparentObject Members
        /// <summary>
        /// Gets the position of the particle.
        /// </summary>
        public Vector3 WorldPosition
        {
            get { return position; }
            set { position = value; }
        }

        #endregion

        #region public constructor
        public Particle(SquareMesh mesh)
        {
            defaultMaterial.Diffuse = System.Drawing.Color.White;
            defaultMaterial.Ambient = System.Drawing.Color.White;

            this.mesh = mesh;
            this.number = totalNumber++;            
        }
        #endregion

        #region Public methods
        public void Create(double creationTime, double lifeTime, ParticleSystem system)
        {
            this.creationTime = creationTime;
            this.lifeTime = lifeTime;
            this.alive = true;
            this.random = rnd.Next(10);
            this.particleSystem = system;
        }

        public void Destroy()
        {
            this.alive = false;
        }
        #endregion

        #region IFrameworkCallback Members
        /// <summary>
        /// Prepares the next frame.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="totalTime"></param>
        /// <param name="elapsedTime"></param>
        public void OnFrameMove(Device device, double totalTime, float elapsedTime)
        {
            if (alive) 
            {
                try
                {
                    // move in the wind
                    if (particleSystem != null)
                    {
                        this.WorldPosition += elapsedTime*particleSystem.WindVector;
                    }
                    double life = (totalTime - creationTime) / lifeTime;
                    scale = new Vector3(0.5f + 4 * (float)life, 0.5f + 4 * (float)life, 0.5f + 4 * (float)life);
                    Vector3 diff = Framework.Instance.CurrentCamera.LookFrom - WorldPosition;
                    float lengthSq = diff.LengthSq();
                    if (lengthSq < 0.0001f)
                        diff = Framework.Instance.CurrentCamera.LookAt - Framework.Instance.CurrentCamera.LookFrom;
                    else
                        diff *= (float)(1.0f / Math.Sqrt(lengthSq));

                    // uncomment the following line to rotate along the viewaxis
                    //Vector3 crossed = Vector3.Cross(Matrix.Trans Framework.Instance.CurrentCamera.Up, diff);

                    Vector3 upVector = Framework.Instance.CurrentCamera.Up;
                    upVector.TransformCoordinate(Matrix.RotationAxis(Framework.Instance.CurrentCamera.LookAt - Framework.Instance.CurrentCamera.LookFrom,
                        (float)((number % 2 == 0 ? 1 : -1) * (number + totalTime / (random + 1)))));
                    Vector3 crossed = Vector3.Cross(upVector, diff);

                    crossed.Normalize();
                    Vector3 final = Vector3.Cross(diff, crossed);

                    transformMatrix.M11 = final.X;
                    transformMatrix.M12 = final.Y;
                    transformMatrix.M13 = final.Z;
                    transformMatrix.M14 = 0;
                    transformMatrix.M21 = crossed.X;
                    transformMatrix.M22 = crossed.Y;
                    transformMatrix.M23 = crossed.Z;
                    transformMatrix.M24 = 0;
                    transformMatrix.M31 = diff.X;
                    transformMatrix.M32 = diff.Y;
                    transformMatrix.M33 = diff.Z;
                    transformMatrix.M34 = 0;
                    transformMatrix.M41 = position.X;
                    transformMatrix.M42 = position.Y;
                    transformMatrix.M43 = position.Z;
                    transformMatrix.M44 = 1.0f;
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("particle.OnFrameMove : " + ex.ToString());
                }
            }
        }

        /// <summary>
        /// Renders the particle.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="totalTime"></param>
        /// <param name="elapsedTime"></param>
        public void OnFrameRender(Device device, double totalTime, float elapsedTime)
        {
            if (alive && Framework.Instance.CurrentCamera != null)
            {
                try
                {
                    double life = (totalTime - creationTime);// / lifeTime;
                    if (life < lifeTime)
                    {
                        int alpha = 0;
                        if (life < lifeTime / 2)
                        {
                            alpha = 128 - (int)(100 * 2 * (lifeTime / 2 - life) / lifeTime);
                        }
                        else
                        {
                            alpha = 128 - (int)(128 * 2 * (life - lifeTime / 2) / lifeTime);
                        }
                        alpha = Math.Min(alpha * intensity / 100, 255);
                        material.Diffuse = material.Ambient = Color.FromArgb(alpha, Color.White);
                        material.Emissive = Color.Gray;
                        device.Material = material;
                        // Move
                        device.Transform.World = Matrix.Scaling(scale) * transformMatrix;
                        // Render
                        mesh.OnParticleFrameRender(device, totalTime, elapsedTime);
                        //mesh.OnFrameRender(device, totalTime, elapsedTime);
                        // Reset transformation
                        device.Transform.World = Matrix.Identity;
                        // Reset the material
                        device.Material = defaultMaterial;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("particle.OnFrameRender : " + ex.ToString());
                }
            }
        }
        #endregion
    }
}
