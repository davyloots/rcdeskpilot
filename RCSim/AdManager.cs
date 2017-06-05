using System;
using System.Collections.Generic;
using System.Text;
using RCSim.Utils;
using System.IO;
using System.Drawing;

namespace RCSim
{
    internal class AdManager
    {
        #region Private fields
        private Scenery scenery = null;
        private Program owner = null;
        private string affImage = null;
        private string affUrl = null;
        private Size affSize;
        #endregion

        #region Constructor
        public AdManager(Scenery scenery, Program owner)
        {
            this.scenery = scenery;
            this.owner = owner;
            Downloader.LoadFileFromWeb("http://rcdeskpilot.com/sim/022/", "data/ads/intro.txt", new Downloader.DownloadedHandler(this.IntroDownloadedHandler));
            Downloader.LoadFileFromWeb("http://rcdeskpilot.com/sim/022/ad1.php", "data/ads/ad1.jpg", new Downloader.DownloadedHandler(this.DownloadedHandler));
            Downloader.LoadFileFromWeb("http://rcdeskpilot.com/sim/022/ad2.php", "data/ads/ad2.jpg", new Downloader.DownloadedHandler(this.DownloadedHandler));            
        }
        #endregion

        public void DownloadedHandler(Downloader.FileContext context)
        {
            if (context.Result == Downloader.DownloadResult.OK)
            {
                if (context.Url.Equals("http://rcdeskpilot.com/sim/022/ad1.php"))
                    scenery.ApplyAds("ad.jpg", "ads/ad1.jpg");
                else
                    scenery.ApplyAds("ad2.jpg", "ads/ad2.jpg");
            }
        }

        public void IntroDownloadedHandler(Downloader.FileContext context)
        {
            if (context.Result == Downloader.DownloadResult.OK)
            {
                using (TextReader reader = new StreamReader(context.FileLocation))
                {
                    affImage = reader.ReadLine();
                    affUrl = reader.ReadLine();
                    string affSizeText = reader.ReadLine();
                    string buttonUrl = reader.ReadLine();
                    string buttonText = reader.ReadLine();
                    StringBuilder introtext = new StringBuilder();
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        introtext.Append(line);
                        introtext.Append("\r\n");
                        line = reader.ReadLine();
                    }
                    owner.WelcomeDialog.SetIntroInfo(introtext.ToString(), buttonText, buttonUrl);

                    string[] affSizeParts = affSizeText.Split('x', 'X');
                    affSize = new Size(Convert.ToInt32(affSizeParts[0]), Convert.ToInt32(affSizeParts[1]));

                    Downloader.LoadFileFromWeb(affImage, "data/ads/affiliate.gif", new Downloader.DownloadedHandler(this.AffiliateDownloadHandler));
                }
            }
            else
            {
                owner.WelcomeDialog.SetIntroInfo("No internet connection was found. This game works best with an active internet connection.", "website", "http://rcdeskpilot.com");
            }
        }

        public void AffiliateDownloadHandler(Downloader.FileContext context)
        {
            if (context.Result == Downloader.DownloadResult.OK)
            {
                try
                {
                    Image affImage = new Bitmap(context.FileLocation);
                    affImage.Save("data/ads/affiliate.png", System.Drawing.Imaging.ImageFormat.Png);
                    affImage.Dispose();
                    //owner.MenuDialog.SetAffiliate(context.FileLocation, affUrl, affSize);
                    owner.MenuDialog.SetAffiliate("data/ads/affiliate.png", affUrl, affSize);
                }
                catch
                {
                }
            }
        }
    }
}
