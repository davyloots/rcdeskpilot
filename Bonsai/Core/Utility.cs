using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Bonsai.Core.Dialogs;
using System.Collections.Generic;

namespace Bonsai.Core
{
    /// <summary>
    /// Misc utility functionality
    /// </summary>
    public class Utility
    {
        // Constants for search folders
        private const string CurrentFolder = @".\";
        private const string MediaPath = @"Data\";

        // Typical folder locations
        //      .\
        //      ..\
        //      ..\..\
        //      %EXE_DIR%\
        //      %EXE_DIR%\..\
        //      %EXE_DIR%\..\..\
        //      %EXE_DIR%\..\%EXE_NAME%
        //      %EXE_DIR%\..\..\%EXE_NAME%
        //      DXSDK media path
        private static readonly string[] TypicalFolders = new string[] { CurrentFolder, @"..\",
                                                                               @"..\..\", @"{0}\", @"{0}\..\", @"{0}\..\..\", @"{0}\..\{1}\", @"{0}\..\..\{1}\" };
        private Utility() { /* Private Constructor */ }

        public static string FindMediaFile(string filename)
        {
            return FindMediaFile(filename, null);
        }

        /// <summary>
        /// Returns a valid path to a DXSDK media file
        /// </summary>
        /// <param name="path">Initial path to search</param>
        /// <param name="filename">Filename we're searching for</param>
        /// <returns>Full path to the file</returns>
        public static string FindMediaFile(string filename, string folder)
        {
            // Find out the executing assembly information
            System.Reflection.Assembly executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            // Now check the typical folders, before you can do that you'll need to get 
            // the executable name
            string exeName = System.IO.Path.GetFileNameWithoutExtension(executingAssembly.Location);
            // And the executable folder
            string exeFolder = System.IO.Path.GetDirectoryName(executingAssembly.Location);

            // Before throwing an exception, first check to see if the file exists as is
            if (File.Exists(filename))
            {
                return filename;
            }

            string filePath;

            // Search the given folder
            if (folder != null)
            {
                string fullPath = string.Format("{0}{1}", folder, filename);
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }

            // Now you can search the typical folders
            if (SearchTypicalFolders(filename, exeFolder, exeName, out filePath))
            {
                return filePath;
            }

            // The file wasn't found again, search the folders with \media on them
            // Now you can search the typical folders
            if (SearchTypicalFolders(filename + MediaPath, exeFolder, exeName, out filePath))
            {
                return filePath;
            }

            // We still haven't found the file yet, we should search the parents folders now
            if (SearchParentFolders(filename, CurrentFolder, "", out filePath))
            {
                return filePath;
            }
            // We still haven't found the file yet, now search from the exe folder
            if (SearchParentFolders(filename, exeFolder, exeName, out filePath))
            {
                return filePath;
            }

            // We still haven't found the file yet, we should search the parents folders now, but append media
            if (SearchParentFolders(filename, CurrentFolder, MediaPath, out filePath))
            {
                return filePath;
            }
            // We still haven't found the file yet, now search from the exe folder and append media
            if (SearchParentFolders(filename, exeFolder, AppendDirectorySeparator(exeName) + MediaPath, out filePath))
            {
                return filePath;
            }


            // We still haven't found the file yet, the built samples are prefixed with 'cs', so see if that's the case
            if (exeName.ToLower().StartsWith("cs"))
            {
                // Build the new exe name by stripping off the 'cs' prefix and doing the searches again
                string newExeName = exeName.Substring(2, exeName.Length - 2);
                if (SearchParentFolders(filename, exeFolder, newExeName, out filePath))
                {
                    return filePath;
                }
                // We still haven't found the file yet, now search from the exe folder and append media
                if (SearchParentFolders(filename, exeFolder, AppendDirectorySeparator(newExeName) + MediaPath, out filePath))
                {
                    return filePath;
                }
            }

            

            throw new MediaNotFoundException(filename);
        }

        /// <summary>
        /// Returns whether or not the provided media file exists.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool MediaExists(string filename)
        {
            try
            {
                FindMediaFile(filename);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the filename part excluding extension from a path.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetFileNamePart(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return "empty";
            string result = filename;
            if (filename.LastIndexOf('/') > -1)
            {
                result = result.Substring(result.LastIndexOf('/') + 1);
            }
            if (filename.LastIndexOf('\\') > -1)
            {
                result = result.Substring(result.LastIndexOf('\\') + 1);
            }
            if (filename.LastIndexOf('.') > -1)
            {
                result = result.Substring(0, result.LastIndexOf('.'));
            }
            return result;
        }

        /// <summary>
        /// Will search the typical list of folders for the file first
        /// </summary>
        /// <param name="filename">File we are looking for</param>
        /// <param name="exeFolder">Folder of the executable</param>
        /// <param name="exeName">Name of the executable</param>
        /// <param name="fullPath">Returned path if file is found.</param>
        /// <returns>true if the file was found; false otherwise</returns>
        private static bool SearchTypicalFolders(string filename, string exeFolder, string exeName, out string fullPath)
        {
            // First scan through each typical folder and see if we found the file
            for (int i = 0; i < TypicalFolders.Length; i++)
            {
                try
                {
                    FileInfo info = new FileInfo(string.Format(TypicalFolders[i], exeFolder, exeName) + filename);
                    if (info.Exists)
                    {
                        fullPath = info.FullName;
                        return true;
                    }
                }
                catch (NotSupportedException)
                {
                    // This exception will be fired if the filename is not supported
                    continue;
                }
            }

            // We never found any of the files
            fullPath = string.Empty;
            return false;
        }

        /// <summary>
        /// Will search the parents of files looking for media
        /// </summary>
        /// <param name="filename">File we are looking for</param>
        /// <param name="rootNode">Folder of the executable</param>
        /// <param name="leafName">Name of the executable</param>
        /// <param name="fullPath">Returned path if file is found.</param>
        /// <returns>true if the file was found; false otherwise</returns>
        private static bool SearchParentFolders(string filename, string rootNode, string leafName, out string fullPath)
        {
            // Set the out parameter first
            fullPath = string.Empty;
            try
            {
                // Search the root node first
                FileInfo info = new FileInfo(AppendDirectorySeparator(rootNode) + AppendDirectorySeparator(leafName) + filename);
                if (info.Exists)
                {
                    fullPath = info.FullName;
                    return true;
                }
            }
            catch (NotSupportedException)
            {
                // The arguments passed in are not supported, fail now
                return false;
            }

            // Are we in the root yet?
            DirectoryInfo dir = new DirectoryInfo(rootNode);
            if (dir.Parent != null)
            {
                return SearchParentFolders(filename, dir.Parent.FullName, leafName, out fullPath);
            }
            else
            {
                // We never found any of the files
                return false;
            }
        }

        /// <summary>
        /// Returns a valid string with a directory separator at the end.
        /// </summary>
        public static string AppendDirectorySeparator(string pathName)
        {
            if (!pathName.EndsWith(@"\"))
                return pathName + @"\";

            return pathName;
        }

        /// <summary>Returns the view matrix for a cube map face</summary>
        public static Matrix GetCubeMapViewMatrix(CubeMapFace face)
        {
            Vector3 vEyePt = new Vector3(0.0f, 0.0f, 0.0f);
            Vector3 vLookDir = new Vector3();
            Vector3 vUpDir = new Vector3();

            switch (face)
            {
                case CubeMapFace.PositiveX:
                    vLookDir = new Vector3(1.0f, 0.0f, 0.0f);
                    vUpDir = new Vector3(0.0f, 1.0f, 0.0f);
                    break;
                case CubeMapFace.NegativeX:
                    vLookDir = new Vector3(-1.0f, 0.0f, 0.0f);
                    vUpDir = new Vector3(0.0f, 1.0f, 0.0f);
                    break;
                case CubeMapFace.PositiveY:
                    vLookDir = new Vector3(0.0f, 1.0f, 0.0f);
                    vUpDir = new Vector3(0.0f, 0.0f, -1.0f);
                    break;
                case CubeMapFace.NegativeY:
                    vLookDir = new Vector3(0.0f, -1.0f, 0.0f);
                    vUpDir = new Vector3(0.0f, 0.0f, 1.0f);
                    break;
                case CubeMapFace.PositiveZ:
                    vLookDir = new Vector3(0.0f, 0.0f, 1.0f);
                    vUpDir = new Vector3(0.0f, 1.0f, 0.0f);
                    break;
                case CubeMapFace.NegativeZ:
                    vLookDir = new Vector3(0.0f, 0.0f, -1.0f);
                    vUpDir = new Vector3(0.0f, 1.0f, 0.0f);
                    break;
            }

            // Set the view transform for this cubemap surface
            Matrix matView = Matrix.LookAtLH(vEyePt, vLookDir, vUpDir);
            return matView;
        }
        /// <summary>Returns the view matrix for a cube map face</summary>
        public static Matrix GetCubeMapViewMatrix(int face) { return GetCubeMapViewMatrix((CubeMapFace)face); }

        private static bool firstTime = true;
        /// <summary>
        /// Displays the switching to ref device warning, and allows user to quit if they don't want to
        /// </summary>
        public static void DisplaySwitchingToRefWarning(Framework framework, string title)
        {
            DisplaySwitchingToRefWarning(framework, title, null);            
        }

        /// <summary>
        /// Displays the switching to ref device warning, and allows user to quit if they don't want to
        /// </summary>
        public static void DisplaySwitchingToRefWarning(Framework framework, string title, string text)
        {
            if (framework.IsShowingMsgBoxOnError)
            {
                // Read the registry key to see if the warning should be skipped
                int skipWarning = 0;
                try
                {
                    using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(SwitchRefDialog.KeyLocation))
                    {
                        skipWarning = (int)key.GetValue(SwitchRefDialog.KeyValueName, (int)0);
                    }
                }
                catch { } // Ignore any errors
                if ((skipWarning == 0) && (firstTime)) // Show dialog
                {
                    firstTime = false;
                    if (text != null)
                    {
                        using (SwitchRefDialog dialog = new SwitchRefDialog(title, text))
                        {
                            System.Windows.Forms.Application.Run(dialog);
                            if (dialog.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                            {
                                // Shutdown the application
                                System.Windows.Forms.MessageBox.Show("Closing due to user request");
                                framework.Dispose();
                            }
                        }
                    }
                    else
                    {
                        using (SwitchRefDialog dialog = new SwitchRefDialog(title))
                        {
                            System.Windows.Forms.Application.Run(dialog);
                            if (dialog.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                            {
                                // Shutdown the application
                                System.Windows.Forms.MessageBox.Show("Closing due to user request");
                                framework.Dispose();
                            }
                        }
                    }
                }
            }
        }

        public static double Interpolate(List<KeyValuePair<double, double>> knownSamples, double z)
        {
            for (int i = 0; i < knownSamples.Count - 1; i++)
            {
                if ((z >= knownSamples[i].Key) && (z < knownSamples[i + 1].Key))
                {
                    double factor = (z - knownSamples[i].Key) / (knownSamples[i + 1].Key - knownSamples[i].Key);
                    return factor * knownSamples[i + 1].Value + (1 - factor) * knownSamples[i].Value;
                }
            }
            if (z < knownSamples[0].Key)
                return knownSamples[0].Value;
            else
                return knownSamples[knownSamples.Count - 1].Value;
        }

        public static double GetMaximumKey(List<KeyValuePair<double, double>> samples)
        {
            int maxKeyIndex = 0;
            double maxValue = samples[maxKeyIndex].Value;
            for (int i = 1; i < samples.Count; i++)
            {
                if (samples[i].Value > maxValue)
                {
                    maxKeyIndex = i;
                }
            }
            return samples[maxKeyIndex].Key;
        }

        public static double GetMinimumKey(List<KeyValuePair<double, double>> samples)
        {
            int minKeyIndex = 0;
            double minValue = samples[minKeyIndex].Value;
            for (int i = 1; i < samples.Count; i++)
            {
                if (samples[i].Value < minValue)
                {
                    minKeyIndex = i;
                }
            }
            return samples[minKeyIndex].Key;
        }

        public static double NormalizeAngle(double angle)
        {
            if (angle < -Math.PI)
            {
                int nPi = (int)((angle + Math.PI) / (2 * Math.PI)) - 1;
                return angle - 2 * nPi * Math.PI;
            }
            else if (angle >= Math.PI)
            {
                int nPi = (int)((angle + Math.PI) / (2 * Math.PI));
                return angle - 2 * nPi * Math.PI;
            }
            else
                return angle;
        }
       
        public static Vector3 EulerAnglesFromQuaternion(Quaternion quat)
        {
            Vector3 result = new Vector3();

            double r11, r21, r31, r32, r33, r12, r13;
            double q00, q11, q22, q33;
            double tmp;

            q00 = quat.W * quat.W;
            q11 = quat.X * quat.X;
            q22 = quat.Y * quat.Y;
            q33 = quat.Z * quat.Z;

            r11 = q00 + q11 - q22 - q33;
            r21 = 2 * (quat.X * quat.Y + quat.W * quat.Z);
            r31 = 2 * (quat.X * quat.Z - quat.W * quat.Y);
            r32 = 2 * (quat.Y * quat.Z + quat.W * quat.X);
            r33 = q00 - q11 - q22 + q33;

            tmp = Math.Abs(r31);
            if (tmp > 0.999999)
            {
                r12 = 2 * (quat.X * quat.Y - quat.W * quat.Z);
                r13 = 2 * (quat.X * quat.Z + quat.W * quat.Y);

                result.X = 0.0f; //roll
                result.Y = ((float)(-(Math.PI / 2) * r31 / tmp)); // pitch
                result.Z = ((float)Math.Atan2(-r12, -r31 * r13)); // yaw
                return result;
            }

            result.X = ((float)Math.Atan2(r32, r33)); // roll
            result.Y = ((float)Math.Asin(-r31));		 // pitch
            result.Z = ((float)Math.Atan2(r21, r11)); // yaw
            return result;
        }       
    }
}
