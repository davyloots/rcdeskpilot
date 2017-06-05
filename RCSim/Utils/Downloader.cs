using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Net;
using System.Threading;
using System.IO;
using Bonsai.Core;

namespace RCSim.Utils
{
    internal static class Downloader
    {
        const int TIMEOUT = 30000;

        public enum DownloadResult
        {
            OK,
            Failed
        }

        public class FileContext
        {
            public string Url;
            public string FileLocation;
            public DownloadedHandler Handler;
            public DownloadResult Result = DownloadResult.OK;
        }

        public delegate void DownloadedHandler(FileContext context);

        public static void LoadFileFromWeb(string url, string fileLocation, DownloadedHandler handler)
        {
            FileContext context = new FileContext();
            context.Url = url;
            context.FileLocation = fileLocation;
            context.Handler = handler;
            ThreadPool.QueueUserWorkItem(new WaitCallback(LoadFileFromWebRun), context);
        }

        public static void LoadFileFromWebRun(object rawContext)
        {
            FileContext context = rawContext as FileContext;
            if (context != null)
            {
                try
                {                    
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(context.Url);
                    request.Timeout = TIMEOUT;
                    request.Proxy = new WebProxy();
                    request.KeepAlive = false;
                    //request.AllowAutoRedirect = true;
                    using (WebResponse response = request.GetResponse())
                    {
                        Stream responseStream = response.GetResponseStream();
                        byte[] read = new byte[1024];
                        byte[] buffer = new byte[1024];
                        int current = 0;
                        string dirName = new FileInfo(context.FileLocation).DirectoryName;
                        if (!Directory.Exists(dirName))
                            Directory.CreateDirectory(dirName);
                        try
                        {
                            using (FileStream file = new FileStream(context.FileLocation, FileMode.Create))
                            {                                
                                int count = responseStream.Read(buffer, 0, 1024);
                                while (count > 0)
                                {
                                    current += count;
                                    file.Write(buffer, 0, count);
                                    count = responseStream.Read(buffer, 0, 1024);
                                }
                                file.Close();
                            }
                            responseStream.Close();
                            response.Close();
                        }
                        catch (IOException)
                        {
                            try
                            {
                                File.Delete(context.FileLocation);
                            }
                            catch
                            {
                            }
                        }
                        if ((Framework.Instance != null) && (Framework.Instance.Window != null))
                            Framework.Instance.Window.Invoke(context.Handler, context);
                    }
                }
                catch (WebException)
                {
                    context.Result = DownloadResult.Failed;
                    if ((Framework.Instance != null) && (Framework.Instance.Window != null))
                        Framework.Instance.Window.Invoke(context.Handler, context);
                }
            }
        }

        
    }
}
