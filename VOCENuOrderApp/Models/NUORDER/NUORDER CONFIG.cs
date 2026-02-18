namespace VOCENuOrderApp.Models.NUORDER
{
    public class NuOrderVOCEConfig
    {
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string Token { get; set; }
        public string TokenSecret { get; set; }
        public string Version { get; set; } = "1.0";
        public string SignatureMethod { get; set; } = "HMAC-SHA1";
    }
}
