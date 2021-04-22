using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Security.Claims;

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
                  ClientId = "cms",
                  ClientSecrets = new[] { new Secret("secret".Sha256()) },
                  AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                  AllowedScopes = new List<string> {"cms.read"}
                },
                new Client
                {
                    ClientId = "oidcClient",
                    ClientName = "Example Client Application",
                    ClientSecrets = new List<Secret> {new Secret("SuperSecretPassword".Sha256())}, // change me!
    
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = new List<string> {Environment.GetEnvironmentVariable("REDIRECT_URL")},
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "role",
                        "cms.read"
                    },
                    RequirePkce = true,
                    AllowPlainTextPkce = false
                }
        };

        public static List<TestUser> GetApiUsers => new List<TestUser> {
             new TestUser
             {
                SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                Username = "shahid",
                Password = "password",
                Claims = new List<Claim> {
                    new Claim(JwtClaimTypes.Email, "shahidkochak@gmail.com"),
                    new Claim(JwtClaimTypes.Role, "admin")
                }
             }
        };

        public static IEnumerable<ApiScope> GetApiScopes => new List<ApiScope> {
            new ApiScope("cms.read", "Read Access to API #1"),
            new ApiScope("cms.write", "Write Access to API #1")
        };
    }
}
