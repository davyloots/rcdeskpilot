using System;

using Microsoft.DirectX;
using Microsoft.DirectX.DirectSound;

namespace Bonsai.Sound
{
    /// <summary>
    /// A class representing a sound that is fully controllable in 3D space.
    /// </summary>
    public class Sound3D : Sound
    {
        #region Private fields
        private Buffer3D buffer3D = null;
        #endregion

        #region public Listener class
        public class Listener
        {
            private Listener3D listener = null;
            private Listener3DOrientation orientation;
            //private Listener3DSettings listenerSettings;
            private double direction = 0.0;

            /// <summary>
            /// Gets/sets the position of the listener.
            /// </summary>
            public Vector3 Position
            {
                get
                {
                    if (listener != null)
                        return listener.Position;
                    else
                        return Vector3.Empty;
                }
                set
                {
                    if (listener != null)
                    {
                        listener.Position = value;
                        listener.CommitDeferredSettings();
                    }
                }
            }

            /// <summary>
            /// Gets/sets the direction in which the listener is looking (in radials).
            /// </summary>
            public double Direction
            {
                get
                {
                    // We could use goniometry to calculate this, but this is easier :-)
                    return direction;
                }
                set
                {
                    if (listener != null)
                    {
                        direction = value;
                        orientation.Front = new Vector3(
                            (float)Math.Cos(value),
                            0,
                            (float)Math.Sin(value));
                        orientation.Top = new Vector3(0, 1, 0);
                        listener.Orientation = orientation;
                    }
                }
            }

            /// <summary>
            /// Gets/Sets the amount a sound diminishes over distance.
            /// </summary>
            public float DistanceFactor
            {
                get
                {
                    return listener.DistanceFactor;
                }
                set
                {
                    listener.DistanceFactor = value;
                }
            }

            public Listener()
            {
                // Create the listener
                BufferDescription desc = new BufferDescription();
                desc.PrimaryBuffer = true;
                desc.Control3D = true;
                listener = new Listener3D(
                    new Microsoft.DirectX.DirectSound.Buffer(desc, SoundManager.Device));
                // Set the listener orientation
                orientation = new Listener3DOrientation();
                orientation.Front = new Vector3(0, 0, 1);
                orientation.Top = new Vector3(0, 1, 0);
                listener.Orientation = orientation;
            }
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/sets the location of the sound in 3D space.
        /// </summary>
        public Vector3 Location
        {
            get
            {
                if ((buffer3D != null) && (buffer3D.Position != null))
                    return buffer3D.Position;
                else
                    return Vector3.Empty;
            }
            set
            {
                if (buffer3D != null)
                {
                    buffer3D.Position = value;
                }
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a 3D capable sound.
        /// </summary>
        /// <param name="fileName">The filename of the sound.</param>
        /// <remarks>The sound must be mono!</remarks>
        public Sound3D(string fileName)
            : base(fileName)
        {

        }
        #endregion

        #region Overriden public Sound methods
        /// <summary>
        /// Play the sound.
        /// </summary>
        /// <param name="looping">Play sound in loop.</param>
        public override void Play(bool looping)
        {
            if (looping)
                buffer.Play(0, BufferPlayFlags.Looping);
            else
                buffer.Play(0, BufferPlayFlags.Default);
        }
        #endregion

        #region Overriden protected Sound methods
        protected override void SetDescriptionFlags(ref Microsoft.DirectX.DirectSound.BufferDescription desc)
        {
            desc.Control3D = true;
            desc.Mute3DAtMaximumDistance = true;
            desc.Guid3DAlgorithm = DSoundHelper.Guid3DAlgorithmHrtfFull;
            base.SetDescriptionFlags(ref desc);
        }
        protected override void BufferLoaded()
        {
            buffer3D = new Buffer3D(buffer);
            //buffer3D.Mode = Mode3D.HeadRelative;
            buffer3D.Deferred = false;
            base.BufferLoaded();
        }
        #endregion
    }
}
