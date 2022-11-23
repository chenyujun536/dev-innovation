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
        
        public static Dictionary<string, ServiceKey> ServiceKeyMap = new Dictionary<string, ServiceKey>() 
        {
            {ServiceKeyFreeTrial, new ServiceKey{Key= "Fake_key_1", Region="eastasia" } },//Azure 订阅 20200413
            {ServiceKeyBGC541, new ServiceKey{Key= "Fake_key_2", Region="eastasia" } }, // au2504160-np-t-bgc-541
            {ServiceKeyBGC541FreeTier, new ServiceKey{Key= "Fake_key_3", Region="koreacentral" } }// au2504160-np-t-bgc-541 free tier
        };

        public static string SelectedServiceKey { get; set; }
        public static string SelectedServiceRegion { get; set; }

        public const int ServiceCloseInterval = 2000;
    }
}
