using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace VOCENuOrderApp.Utilities
{
    public class NuOrderConfig
    {
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string Token { get; set; }
        public string TokenSecret { get; set; }
        public string Version { get; set; }
        public string SignatureMethod { get; set; }
    }

    /* web service wrapper */

    public class NuOrderWebService
    {
        private NuOrderConfig Configuration;

        public string Timestamp { get; set; }
        public string Nonce { get; set; }
        public string Signature { get; set; }

        public string Callback { get; set; }
        public string ApplicationName { get; set; }
        public string VerificationCode { get; set; }

        private bool IsInitRequest;
        private bool IsVerifyRequest;

        public NuOrderWebService(NuOrderConfig Configuration)
        {
            this.Configuration = Configuration;
        }

        /* PUBLIC METHODS */

        public HttpWebResponse ExecuteRequest(string RequestMethod, string EndPoint)
        {
            return ExecuteRequest(RequestMethod, EndPoint, null);
        }

        public HttpWebResponse ExecuteRequest(string RequestMethod, string EndPoint, string Data)
        {
            try
            {
                this.Nonce = GenerateNonce();
                this.Timestamp = GenerateTimestamp().ToString();
                this.Signature = GenerateSignature(RequestMethod, EndPoint);

                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(EndPoint);

                string authorizationHeader = "OAuth";
                foreach (var header in GetRequestHeaders())
                    authorizationHeader += header.Key + "=\"" + header.Value + "\",";
                authorizationHeader = authorizationHeader.Substring(0, authorizationHeader.Length - 1);
                req.Headers.Add(HttpRequestHeader.Authorization, authorizationHeader);
                req.Method = RequestMethod;

                if ((RequestMethod == "POST" || RequestMethod == "PUT") && Data != null)
                {
                    req.ContentType = "application/json";

                    using (StreamWriter writer = new StreamWriter(req.GetRequestStream()))
                    {
                        writer.Write(Data);
                    }
                }

                HttpWebResponse response = (HttpWebResponse)req.GetResponse();

                return response;
            }
            catch (WebException ex)
            {
                // catch error here -- note the response code (i.e. 401, 404, 409, etc) and the response body for more information

                return null;
            }
        }

        public void SetInitRequest(string ApplicationName, string Callback)
        {
            this.IsInitRequest = true;
            this.ApplicationName = ApplicationName;
            this.Callback = Callback;
        }

        public void SetVerifyRequest(string VerificationCode)
        {
            this.IsVerifyRequest = true;
            this.VerificationCode = VerificationCode;
        }

        /* SUPPORT METHODS */

        private Dictionary<string, string> GetRequestHeaders()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("oauth_token", Configuration.Token);
            headers.Add("oauth_consumer_key", Configuration.ConsumerKey);
            headers.Add("oauth_timestamp", Timestamp);
            headers.Add("oauth_nonce", Nonce);
            headers.Add("oauth_version", Configuration.Version);
            headers.Add("oauth_signature_method", Configuration.SignatureMethod);
            headers.Add("oauth_signature", Signature);

            if (IsInitRequest)
            {
                headers.Add("oauth_callback", Callback);
                headers.Add("application_name", ApplicationName);
            }

            if (IsVerifyRequest)
                headers.Add("oauth_verifier", VerificationCode);

            return headers;
        }

        private Random rnd = new Random();
        private const string _characters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        private string GenerateNonce()
        {
            char[] buffer = new char[16];
            for (int i = 0; i < 16; i++)
                buffer[i] = _characters[rnd.Next(_characters.Length)];
            return new string(buffer);
        }

        private int GenerateTimestamp()
        {
            TimeSpan span = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            return Convert.ToInt32(span.TotalSeconds);
        }

        private string GenerateSignature(string RequestMethod, string EndPoint)
        {
            string baseSignatureString = RequestMethod + EndPoint +
                "?oauth_consumer_key=" + Configuration.ConsumerKey + "&" +
                "oauth_token=" + Configuration.Token + "&" +
                "oauth_timestamp=" + Timestamp + "&" +
                "oauth_nonce=" + Nonce + "&" +
                "oauth_version=" + Configuration.Version + "&" +
                "oauth_signature_method=" + Configuration.SignatureMethod;

            if (IsInitRequest) baseSignatureString += "&oauth_callback=" + Callback;
            if (IsVerifyRequest) baseSignatureString += "&oauth_verifier=" + VerificationCode;

            string key = Configuration.ConsumerSecret + "&" + Configuration.TokenSecret;

            return GenerateSHA1Hash(key, baseSignatureString);
        }

        private string GenerateSHA1Hash(string key, string value)
        {
            byte[] keyBytes = ConvertStringToByteArray(key);
            byte[] valueBytes = ConvertStringToByteArray(value);
            byte[] hash = null;
            using (HMACSHA1 hmac = new HMACSHA1(keyBytes))
            {
                hash = hmac.ComputeHash(valueBytes);
            }
            return ConvertByteArrayToHexString(hash);
        }

        private byte[] ConvertStringToByteArray(string str)
        {
            return System.Text.Encoding.ASCII.GetBytes(str);
        }

        private string ConvertByteArrayToHexString(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
