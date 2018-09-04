using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace Saned.Asnan.AuthenticationApi.Dtos
{
    class ExternalLoginData
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string UserName { get; set; }
        public string ExternalAccessToken { get; set; }

        public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
        {
            Claim providerKeyClaim = identity?.FindFirst(ClaimTypes.NameIdentifier);

            //if (providerKeyClaim == null || string.IsNullOrEmpty(providerKeyClaim.Issuer) || String.IsNullOrEmpty(providerKeyClaim.Value))
            if (string.IsNullOrEmpty(providerKeyClaim?.Issuer) || string.IsNullOrEmpty(providerKeyClaim.Value))
                return null;

            if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                return null;

            return new ExternalLoginData
            {
                LoginProvider = providerKeyClaim.Issuer,
                ProviderKey = providerKeyClaim.Value,
                UserName = identity.FindFirstValue(ClaimTypes.Name),
                ExternalAccessToken = identity.FindFirstValue("ExternalAccessToken"),
            };
        }


    }
}