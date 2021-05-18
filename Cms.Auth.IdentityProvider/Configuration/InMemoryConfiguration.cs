using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace Cms.Auth.IdentityProvider.Configuration
{
    public class InMemoryConfiguration
    {
        public static IEnumerable<ApiResource> GetApiResources => new List<ApiResource> {
          new ApiResource("api1","My API")
        };
        public static IEnumerable<IdentityResource> GetIdentityResources => new List<IdentityResource>
            {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
             };
        public static IEnumerable<Client> GetApiClients => new List<Client> {
                new Client
                {
                    ClientId = "oidcClient",
                    ClientName = "content management",
                    ClientSecrets = new List<Secret> {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris =          { Environment.GetEnvironmentVariable("CLIENT_REDIRECT_URL") },//callback
                    AllowedCorsOrigins =    { Environment.GetEnvironmentVariable("CLIENT_URL") },
                    PostLogoutRedirectUris = { Environment.GetEnvironmentVariable("CLIENT_POST_LOGOUT_URL") },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    },
                }
        };

        public static List<TestUser> GetApiUsers => new List<TestUser> {
             new TestUser
             {
                SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                Username = "shahid",
                Password = "password",
                Claims = new List<Claim> {
                    new Claim(JwtClaimTypes.Subject, "5BE86359-073C-434B-AD2D-A3932222DABE"),
                    new Claim(JwtClaimTypes.PreferredUserName, "shahidkochak@gmail.com"),
                    new Claim(JwtClaimTypes.Email, "shahidkochak@gmail.com"),
                }
             },
        };

        public static IEnumerable<ApiScope> GetApiScopes => new List<ApiScope> {
           new ApiScope("api1", "My API")
        };

        public static X509Certificate2 GetX509Certificate2()
        {
            string base64Data = Environment.GetEnvironmentVariable("CERTIFICATE_PATH");
            byte[] fileData = Convert.FromBase64String(base64Data);
            return new X509Certificate2(fileData, Environment.GetEnvironmentVariable("CERTIFICATE_PASSWORD"));
        }
    }
}
