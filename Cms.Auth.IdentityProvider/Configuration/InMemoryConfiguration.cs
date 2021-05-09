using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Cms.Auth.IdentityProvider.Configuration
{
    public class InMemoryConfiguration
    {
        public static IEnumerable<ApiResource> GetApiResources => new List<ApiResource> {
         new ApiResource
            {
                Name = "cms",
                DisplayName = "cms #1",
                Description = "Allow the application to access API #1 on your behalf",
                Scopes = new List<string> {"cms.read", "cms.write"},
                ApiSecrets = new List<Secret> {new Secret("secret".Sha256())},
                UserClaims = new List<string> {"role"}
            }
        };
        public static IEnumerable<IdentityResource> GetIdentityResources => new[]
            {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource
            {
                Name = "role",
                UserClaims = new List<string> {"role"}
            }
        };
        public static IEnumerable<Client> GetApiClients => new List<Client> {
                new Client
                {
                    ClientId = "oidcClient",
                    ClientName = "Example Client Application",
                    ClientSecrets = new List<Secret> {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    AccessTokenLifetime = 120,
                    RedirectUris =          { Environment.GetEnvironmentVariable("CLIENT_REDIRECT_URL") },//callback
                    AllowedCorsOrigins =    { Environment.GetEnvironmentVariable("CLIENT_URL") },
                    PostLogoutRedirectUris = { Environment.GetEnvironmentVariable("CLIENT_POST_LOGOUT_URL") },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
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
                    new Claim(ClaimTypes.Email, "shahidkochak@gmail.com"),
                    new Claim(ClaimTypes.Role, "admin"),
                    new Claim(ClaimTypes.Name, "shahidkochak@gmail.com")
                }
             },
        };

        public static IEnumerable<ApiScope> GetApiScopes => new List<ApiScope> {
            new ApiScope("cms.read", "Read Access to API #1"),
            new ApiScope("cms.write", "Write Access to API #1")
        };

        public static X509Certificate2 GetX509Certificate2()
        {
            //string fileName = @"C:\openssl\bin\certificate.p12";
            //byte[] data = System.IO.File.ReadAllBytes(fileName);
            //string base64 = Convert.ToBase64String(data);
            string base64Data = Environment.GetEnvironmentVariable("CERTIFICATE_PATH");
            byte[] fileData = Convert.FromBase64String(base64Data);
            return new X509Certificate2(fileData, Environment.GetEnvironmentVariable("CERTIFICATE_PASSWORD"));
        }
    }
}
