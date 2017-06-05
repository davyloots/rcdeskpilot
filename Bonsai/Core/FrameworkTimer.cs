using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Core.EventArgs;

namespace Bonsai.Core
{
    /// <summary>Stores timer callback information</summary>
    public struct TimerData
    {
        public TimerCallback callback;
        public float TimeoutInSecs;
        public float Countdown;
        public bool IsEnabled;
    }

    public class FrameworkTimer
    {
        #region Instance Data
        private static bool isUsingQPF;
        private static bool isTimerStopped;
        private static long ticksPerSecond;
        private static long stopTime;
        private static long lastElapsedTime;
        private static long baseTime;
        #endregion

        #region Creation
        private FrameworkTimer() { } // No creation
        /// <summary>
        /// Static creation routine
        /// </summary>
        static FrameworkTimer()
        {
            isTimerStopped = true;
            ticksPerSecond = 0;
            stopTime = 0;
            lastElapsedTime = 0;
            baseTime = 0;
            // Use QueryPerformanceFrequency to get frequency of the timer
            isUsingQPF = NativeMethods.QueryPerformanceFrequency(ref ticksPerSecond);
        }
        #endregion

        /// <summary>
        /// Resets the timer
        /// </summary>
        public static void Reset()
        {
            if (!isUsingQPF)
                return; // Nothing to do

            // Get either the current time or the stop time
            long time = 0;
            if (stopTime != 0)
                time = stopTime;
            else
                NativeMethods.QueryPerformanceCounter(ref time);

            baseTime = time;
            lastElapsedTime = time;
            stopTime = 0;
            isTimerStopped = false;
        }

        /// <summary>
        /// Starts the timer
        /// </summary>
        public static void Start()
        {
            if (!isUsingQPF)
                return; // Nothing to do

            // Get either the current time or the stop time
            long time = 0;
            if (stopTime != 0)
                time = stopTime;
            else
                NativeMethods.QueryPerformanceCounter(ref time);

            if (isTimerStopped)
                baseTime += (time - stopTime);
            stopTime = 0;
            lastElapsedTime = time;
            isTimerStopped = false;
        }

        /// <summary>
        /// Stop (or pause) the timer
        /// </summary>
        public static void Stop()
        {
            if (!isUsingQPF)
                return; // Nothing to do

            if (!isTimerStopped)
            {
                // Get either the current time or the stop time
                long time = 0;
                if (stopTime != 0)
                    time = stopTime;
                else
                    NativeMethods.QueryPerformanceCounter(ref time);

                stopTime = time;
                lastElapsedTime = time;
                isTimerStopped = true;
            }
        }

        /// <summary>
        /// Advance the timer a tenth of a second
        /// </summary>
        public static void Advance()
        {
            if (!isUsingQPF)
                return; // Nothing to do

            stopTime += ticksPerSecond / 10;
        }

        /// <summary>
        /// Get the absolute system time
        /// </summary>
        public static double GetAbsoluteTime()
        {
            if (!isUsingQPF)
                return -1.0; // Nothing to do

            // Get either the current time or the stop time
            long time = 0;
            if (stopTime != 0)
                time = stopTime;
            else
                NativeMethods.QueryPerformanceCounter(ref time);

            double absolueTime = time / (double)ticksPerSecond;
            return absolueTime;
        }

        /// <summary>
        /// Get the current time
        /// </summary>
        public static double GetTime()
        {
            if (!isUsingQPF)
                return -1.0; // Nothing to do

            // Get either the current time or the stop time
            long time = 0;
            if (stopTime != 0)
                time = stopTime;
            else
                NativeMethods.QueryPerformanceCounter(ref time);

            double appTime = (double)(time - baseTime) / (double)ticksPerSecond;
            return appTime;
        }

        /// <summary>
        /// get the time that elapsed between GetElapsedTime() calls
        /// </summary>
        public static double GetElapsedTime()
        {
            if (!isUsingQPF)
                return -1.0; // Nothing to do

            // Get either the current time or the stop time
            long time = 0;
            if (stopTime != 0)
                time = stopTime;
            else
                NativeMethods.QueryPerformanceCounter(ref time);

            double elapsedTime = (double)(time - lastElapsedTime) / (double)ticksPerSecond;
            lastElapsedTime = time;
            return elapsedTime;
        }

        /// <summary>
        /// Returns true if timer stopped
        /// </summary>
        public static bool IsStopped
        {
            get { return isTimerStopped; }
        }
    }
}
