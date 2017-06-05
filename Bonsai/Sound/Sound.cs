using System;
using System.Runtime.InteropServices;
using Microsoft.DirectX.DirectSound;
using Buffer = Microsoft.DirectX.DirectSound.Buffer;
using Bonsai.Utils;
using Bonsai.Core;

namespace Bonsai.Sound
{
    public class Sound : IDisposable
    {
        #region Protected fields
        protected Buffer buffer;
        protected float length = 0.0f;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets whether the sound is currently playing.
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                if (buffer != null)
                {
                    return buffer.Status.Playing;
                }
                else
                    return false;
            }
        }
        /// <summary>
        /// Gets the length of the sound in seconds.
        /// </summary>
        public float Length
        {
            get
            {
                return length;
            }
        }
        #endregion

        #region Constructors
        public Sound(string fileName) : this(fileName, null)
        {
            
        }

        public Sound(string fileName, string folder)
        {
            string path = Utility.FindMediaFile(fileName, folder);
            Tracer.Trace("Sound Constructor", string.Format("creating sound {0}", fileName));
            if (fileName.EndsWith(".ogg"))
            {
                LoadOggFile(path);
            }
            else
                LoadWavFile(path);
            SoundManager.AddSound(this);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Cleans up resources
        /// </summary>
        public void Dispose()
        {
            SoundManager.RemoveSound(this);
            if (buffer != null)
            {
                buffer.Dispose();
                buffer = null;
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Play the sound.
        /// </summary>
        /// <param name="looping">Play sound in loop.</param>
        public virtual void Play(bool looping)
        {
            if (looping)
                buffer.Play(0, BufferPlayFlags.Looping);
            else
                buffer.Play(0, BufferPlayFlags.Default);
        }

        /// <summary>
        /// Stop the playback of the sound.
        /// </summary>
        public void Stop()
        {
            buffer.Stop();
        }
        #endregion

        #region DllImport's
        [DllImport("PlayingInTheDark.Native.dll")]
        private static extern int DecodeVorbisFile(
                string fileName,
                byte[] buffer,
                int bufferSize,
                ref int samplesPerSec,
                ref int channels);

        [DllImport("PlayingInTheDark.Native.dll")]
        private static extern int DecodedVorbisSize(
                string fileName,
                ref int bufferSize);
        #endregion

        #region Protected methods
        /// <summary>
        /// Method meant to be overridden by deriving classes.
        /// </summary>
        /// <param name="desc"></param>
        /// <remarks>Normally we would use an event, but since this is done in 
        /// the constructor, an event cannot be used.</remarks>
        protected virtual void SetDescriptionFlags(ref BufferDescription desc)
        {
            // nothing special here.
        }
        /// <summary>
        /// Method meant to be overridden by deriving classes.
        /// </summary>
        /// <remarks>Normally we would use an event, but since this is done in 
        /// the constructor, an event cannot be used.</remarks>
        protected virtual void BufferLoaded()
        {
            this.length = (float)(buffer.Caps.BufferBytes) / buffer.Format.AverageBytesPerSecond;
        }
        #endregion

        #region Private methods
        private void LoadWavFile(string fileName)
        {
            Tracer.Trace("Sound.LoadWavFile", string.Format("Loading '{0}'", fileName));
            BufferDescription desc = new BufferDescription();
            SetDescriptionFlags(ref desc);
            buffer = new SecondaryBuffer(fileName, desc, SoundManager.Device);
            BufferLoaded();
        }
        private void LoadOggFile(string fileName)
        {
            Tracer.Trace("Sound.LoadOggFile", string.Format("Loading '{0}'", fileName));
            int bufferSize = 0;
            DecodedVorbisSize(fileName, ref bufferSize);
            byte[] tempBuffer = new byte[bufferSize];
            int samplesPerSec = 0;
            int channels = 0;
            int result = DecodeVorbisFile(fileName, tempBuffer, bufferSize, ref samplesPerSec, ref channels);
            WaveFormat waveFormat = new WaveFormat();
            waveFormat.Channels = (short)channels;
            waveFormat.BitsPerSample = 16;
            waveFormat.SamplesPerSecond = samplesPerSec;
            waveFormat.AverageBytesPerSecond = samplesPerSec * 2 * channels;
            waveFormat.BlockAlign = (short)(2 * channels);
            waveFormat.FormatTag = WaveFormatTag.Pcm;
            BufferDescription bufferDesc = new BufferDescription(waveFormat);

            bufferDesc.BufferBytes = bufferSize;
            SetDescriptionFlags(ref bufferDesc);
            //bufferDesc.Flags = BufferDescriptionFlags.ControlVolume;
            buffer = new SecondaryBuffer(bufferDesc, SoundManager.Device);
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream(tempBuffer))
            {
                buffer.Write(0, stream, bufferSize, LockFlag.EntireBuffer);
            }
            BufferLoaded();
        }
        #endregion
    }
}
