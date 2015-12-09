using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Web;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Newtonsoft.Json;
using RestSharp;
using System.Configuration;

namespace MP_WService
{
    public partial class MyService : ServiceBase
    {
        protected System.Timers.Timer gTimer = new System.Timers.Timer();
        private log4net.ILog gInfoLogger = log4net.LogManager.GetLogger("MP_WS_info");
        private log4net.ILog gDebugLogger = log4net.LogManager.GetLogger("MP_WS_debug");
        private static System.Configuration.Configuration config =
         ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private System.Configuration.AppSettingsSection appSettings = (System.Configuration.AppSettingsSection)config.GetSection("appSettings");


        public MyService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //setup the timer interval
            gTimer.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["TIMER_INTERVAL"]);//1 hour
            
            //Enable the timer
            gTimer.Enabled = true;
            //Start the timer
            gTimer.Start();
            if (gInfoLogger.IsInfoEnabled == true)
            {
                gInfoLogger.Info("Timer Started at: " + DateTime.Now);
            }
            //Fire the elapsed event
            gTimer.Elapsed += new System.Timers.ElapsedEventHandler(gtimer_elapsed);


        }

        protected override void OnStop()
        {
        }

        //This is fired when the timer is elapsed
        protected void gtimer_elapsed(Object sender, System.Timers.ElapsedEventArgs e)
        {
            if (gInfoLogger.IsInfoEnabled == true)
            {
                gInfoLogger.Info("Timer Elapsed");
            }
            //stop the timer
            gTimer.Stop();

            // Do some real work
            if (gInfoLogger.IsInfoEnabled == true)
            {
                gInfoLogger.Info("Stop the timer and do some real work");
            }

            // Run this 4 times only

            for (int i = 0; i <= 3; i++)
            {
            gInfoLogger.Info("Delete the batch : " + i);

            try
            {

   
            
            Account account = new Account(ConfigurationManager.AppSettings["MY_CLOUD_NAME"], ConfigurationManager.AppSettings["MY_API_KEY"], ConfigurationManager.AppSettings["MY_API_SECRET"]);

            var cloudinary = new Cloudinary(account);
                  

            var listParams = new ListResourcesParams();
            listParams.MaxResults = 500;
            var results = new ListResourcesResult();
            //List Resources
            results = cloudinary.ListResources(listParams);


            //  string[] public_ids= {};
            List<string> public_ids = new List<string>();
            //var publicids="";

            //var delparams = new DeletionParams("");
           
           // gInfoLogger.Info(results.Resources);
            foreach (var resultResource in results.Resources)
            {
     
                gInfoLogger.Info("Delete the image: " + resultResource.PublicId);
                
                var client = new RestClient(ConfigurationManager.AppSettings["CLOUDINARY_REST_URL"]);
                client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(ConfigurationManager.AppSettings["MY_API_KEY"], ConfigurationManager.AppSettings["MY_API_SECRET"]);
                var request = new RestRequest("v1_1/"+ ConfigurationManager.AppSettings["MY_CLOUD_NAME"] + "/resources/image/fetch", Method.DELETE);
                request.AddParameter("public_ids", resultResource.PublicId);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("X-DDConsumer", "hj45tuGehrw89fy34hiufGheroifu903u4r89yh4igjo3498DS3475983rk3h893");
                request.AddHeader("HTTP.CONTENT_TYPE", "application/json");
                request.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                request.AddHeader("Accept-Language", "en-US,en;q=0.8");
                request.AddHeader("Cookie", "km_ai=ChCiEOdkw46cNwJulKDZMT41Uiw%3D; km_lv=x; km_ni=IT%40OO.COM.AU; __ar_v4=MZT4ZZYCPNAMTJWQJTYVME%3A20150916%3A1%7C5EKLDOVHANF4TIB4CKJC66%3A20150908%3A18%7CFV4HSQCY3JGRNAPBR5KL5B%3A20150908%3A18%7CLP7HHWUA7BCGPHUPNSVUJP%3A20150908%3A18; olfsk=olfsk3097110278904438; hblid=Su3kXoWoJomtTr22887p09Z3JFEQQNIE; _hjUserId=aeba0b19-35b9-302a-871f-ec29950a9a8c; _ga=GA1.2.173876344.1441753175; __ar_v4=LP7HHWUA7BCGPHUPNSVUJP%3A20150908%3A39%7CFV4HSQCY3JGRNAPBR5KL5B%3A20150908%3A39%7C5EKLDOVHANF4TIB4CKJC66%3A20150908%3A38%7CMZT4ZZYCPNAMTJWQJTYVME%3A20150916%3A1; km_uq=; _mkto_trk=id:396-LRB-524&token:_mch-cloudinary.com-1441753174733-86648; __utma=136482402.173876344.1441753175.1443581601.1443594288.39; __utmc=136482402; __utmz=136482402.1443513841.35.4.utmcsr=google|utmccn=(organic)|utmcmd=organic|utmctr=(not%20provided); kvcd=1443596052135");
                client.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.99 Safari/537.36";

                IRestResponse response = client.Execute(request);
                var content = response.Content;
            }
            }
                catch (Exception ex)
            {
                gInfoLogger.Debug("Exception: " + ex);
            }

            gInfoLogger.Info("Delete completed");

            }

           // gInfoLogger.Info(delResult.JsonObj);
            if (gInfoLogger.IsInfoEnabled == true)
            {
                gInfoLogger.Info("Timer Re-Started");
            }
            //Happy - Restart the timer 
            gTimer.Start();
            
        }
    }
}
