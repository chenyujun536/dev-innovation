using System;
using System.Collections.Generic;
using System.Text;

namespace ShootRate.Resources
{
    public class ServiceKey
    {
        public string Key { get; set; }
        public string Region { get; set; }
    }

    public static class Constants
    {
        public const string ServiceKeyFreeTrial = "FreeTry";
        public const string ServiceKeyBGC541 = "BGC-541";
        public const string ServiceKeyBGC541FreeTier = "541Free";

        //public static string CognitiveServicesApiKey = "a171e895338f4f23935c12fc7d93ab5e"; // au2504160-np-t-bgc-541

        //public static string CognitiveServicesApiKey = "6728c345cd984da1a041b71b87396489"; //Azure 订阅 20200413
        public static Dictionary<string, ServiceKey> ServiceKeyMap = new Dictionary<string, ServiceKey>() 
        {
            {ServiceKeyFreeTrial, new ServiceKey{Key= "6728c345cd984da1a041b71b87396489", Region="eastasia" } },//Azure 订阅 20200413
            {ServiceKeyBGC541, new ServiceKey{Key= "a171e895338f4f23935c12fc7d93ab5e", Region="eastasia" } }, // au2504160-np-t-bgc-541
            {ServiceKeyBGC541FreeTier, new ServiceKey{Key= "691519de8c7641608c6d1a61b93a4443", Region="koreacentral" } }// au2504160-np-t-bgc-541 free tier
        };

        public static string SelectedServiceKey { get; set; }
        public static string SelectedServiceRegion { get; set; }

        public const int ServiceCloseInterval = 2000;
    }
}
