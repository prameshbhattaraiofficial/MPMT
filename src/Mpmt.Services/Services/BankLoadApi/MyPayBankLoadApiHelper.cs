using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace Mpmt.Services.Services.BankLoadApi
{
    public static class MyPayBankLoadApiHelper
    {
        public static string ToJsonString(object obj)
        {
            var outputString = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                //ContractResolver = new DefaultContractResolver
                //{
                //    NamingStrategy = new CamelCaseNamingStrategy
                //    {
                //        ProcessDictionaryKeys = false,
                //        OverrideSpecifiedNames = false
                //    }
                //},
                ContractResolver = null,
                Formatting = Formatting.None
            });

            return outputString;
        }

        public static StringContent ToStringContent(string input)
        {
            return new StringContent(input, Encoding.UTF8, "application/json");
        }
    }
}
