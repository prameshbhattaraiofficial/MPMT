using Microsoft.AspNetCore.Mvc;

namespace Mpmt.Agent.Models
{
    public class EnableAuthenticatorModel
    {

            public string SharedKey { get; set; }

            public string AuthenticatorUri { get; set; }

            [TempData]
            public string[] RecoveryCodes { get; set; }

            [TempData]
            public string StatusMessage { get; set; }

        
    }
}
