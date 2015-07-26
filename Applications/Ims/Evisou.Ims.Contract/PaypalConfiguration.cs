using System.Collections.Generic;

namespace Evisou.Ims.Contract
{
    public static class Configuration
    {
        // Creates a configuration map containing credentials and other required configuration parameters
        public static Dictionary<string, string> GetAcctAndConfig()
        {
            Dictionary<string, string> configMap = new Dictionary<string, string>();

            configMap = GetConfig();

            // Signature Credential
            /*configMap.Add("account1.apiUsername", "vson.mail_api1.gmail.com");
            configMap.Add("account1.apiPassword", "SXRM84PQDRDF2NN8");
            configMap.Add("account1.apiSignature", "AiPC9BjkCyDFQXbSkoZcgqH3hpacAVh7dGhDzP86zF0PmJmF3H74KeiX");*/
            // Optional
            // configMap.Add("account1.Subject", "");

            // Sample Certificate Credential
            //configMap.Add("account2.apiUsername", "vson.mail_api1.gmail.com");
            //configMap.Add("account2.apiPassword", "RYY2EZN82UVZL99B");
            // configMap.Add("account2.apiCertificate", "~/APICalls/paypal_cert.p12");
            // configMap.Add("account2.privateKeyPassword", "123456");
            // Optional
            // configMap.Add("account2.Subject", "");
            return configMap;
        }

        // Creates a configuration map containing mode and other required configuration parameters
        public static Dictionary<string, string> GetConfig()
        {
            Dictionary<string, string> configMap = new Dictionary<string, string>();

            // Endpoints are varied depending on whether sandbox OR live is chosen for mode
            configMap.Add("mode", "live");

            // These values are defaulted in SDK. If you want to override default values, uncomment it and add your value.
            // configMap.Add("connectionTimeout", "5000");
            // configMap.Add("requestRetries", "2");

            return configMap;
        }
    }
}
