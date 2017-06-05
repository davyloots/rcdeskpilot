using System;
using System.Collections.Generic;

using Microsoft.DirectX;
using Microsoft.DirectX.DirectSound;
using System.Windows.Forms;


namespace Bonsai.Sound
{
    /// <summary>
    /// Static class maintaining the currently loaded sounds.
    /// </summary>
    public static class SoundManager
    {
        private static List<Sound> soundList = new List<Sound>();
        private static Device device = null;
        private static int counter = 0;
        private static Sound3D.Listener listener = null;
        private static int volume = 100;

        #region Public properties
        public static Device Device
        {
            get { return device; }
            set { device = value; }
        }

        /// <summary>
        /// Gets/Sets the listener for 3D sounds.
        /// </summary>
        public static Sound3D.Listener Listener
        {
            get { return listener; }
            set { listener = value; }
        }

        /// <summary>
        /// Gets/Sets the global volume.
        /// </summary>
        public static int Volume
        {
            get { return volume; }
            set { volume = value; }
        }
        #endregion

        #region Internal sound management methods
        internal static void AddSound(Sound sound)
        {
            soundList.Add(sound);
        }
        internal static void RemoveSound(Sound sound)
        {
            soundList.Remove(sound);
        }
        #endregion

        #region Public methods
        public static void Initialize(Control owner)
        {
            device = new Device();
            device.SetCooperativeLevel(owner, CooperativeLevel.Priority);
            listener = new Sound3D.Listener();
        }

        public static void DisposeAllSounds()
        {
            foreach (Sound sound in soundList)
            {
                sound.Dispose();
            }
            soundList.Clear();
        }

        public static void Update3DSounds()
        {
            counter++;
            foreach (Sound sound in soundList)
            {
                if (sound is Sound3D)
                {
                    Sound3D sound3D = sound as Sound3D;
                    if (counter % 2 == 0)
                        sound3D.Location = new Vector3(sound3D.Location.X, sound3D.Location.Y + 0.1f, sound3D.Location.Z);
                    else
                        sound3D.Location = new Vector3(sound3D.Location.X, sound3D.Location.Y - 0.1f, sound3D.Location.Z);
                }
            }
        }
        #endregion
    }
}
