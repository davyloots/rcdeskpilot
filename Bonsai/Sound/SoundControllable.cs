using System;
using System.Collections.Generic;
using System.Text;

namespace Bonsai.Sound
{
    public class SoundControllable : Sound
    {
        #region Private fields
        private int internalVolume = 100;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/sets the left/right volume of the sound [-10000, +10000].
        /// </summary>
        public int Pan
        {
            get { return buffer.Pan; }
            set
            {
                try
                {
                    buffer.Pan = value;
                }
                catch (ArgumentException)
                {

                }
            }
        }
        /// <summary>
        /// Gets/sets the volume of the sound [0, 100].
        /// </summary>
        public int Volume
        {
            get
            {
                return internalVolume; 
            }
            set 
            {
                internalVolume = value;
                if (SoundManager.Volume < 1)
                    buffer.Volume = Convert.ToInt32(Microsoft.DirectX.DirectSound.Volume.Min);
                else
                {
                    int newValue = (100 - (value * Convert.ToInt32(100 * Math.Log10(SoundManager.Volume)) / 200)) * (Convert.ToInt32(Microsoft.DirectX.DirectSound.Volume.Min)) / 100;
                    if (newValue < Convert.ToInt32(Microsoft.DirectX.DirectSound.Volume.Min))
                        buffer.Volume = Convert.ToInt32(Microsoft.DirectX.DirectSound.Volume.Min);
                    else
                        buffer.Volume = newValue;
                }
            }
        }
        /// <summary>
        /// Gets/sets the frequency of the sound.
        /// </summary>
        public int Frequency
        {
            get { return buffer.Frequency; }
            set
            {
                try
                {
                    if (value < (int)Microsoft.DirectX.DirectSound.Frequency.Min)
                        buffer.Frequency = (int)Microsoft.DirectX.DirectSound.Frequency.Min;
                    else if (value > (int)(int)Microsoft.DirectX.DirectSound.Frequency.Max)
                        buffer.Frequency = (int)Microsoft.DirectX.DirectSound.Frequency.Max;
                    else
                        buffer.Frequency = value;
                }
                catch (ArgumentException)
                {
                    // Too low or too high
                }
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a sound with controllable frequency, panning and volume.
        /// </summary>
        /// <param name="fileName">The filename of the sound to load.</param>
        /// <remarks>The sound must be mono!</remarks>
        public SoundControllable(string fileName)
            : base(fileName)
        {
            Volume = 100;
        }

        /// <summary>
        /// Creates a sound with controllable frequency, panning and volume.
        /// </summary>
        /// <param name="fileName">The filename of the sound to load.</param>
        /// <remarks>The sound must be mono!</remarks>
        public SoundControllable(string fileName, string folder)
            : base(fileName, folder)
        {
            Volume = 100;
        }
        #endregion

        #region Overriden protected Sound methods
        protected override void SetDescriptionFlags(ref Microsoft.DirectX.DirectSound.BufferDescription desc)
        {
            desc.ControlFrequency = true;
            desc.ControlPan = true;
            desc.ControlVolume = true;
            base.SetDescriptionFlags(ref desc);
        }
        #endregion
    }
}
