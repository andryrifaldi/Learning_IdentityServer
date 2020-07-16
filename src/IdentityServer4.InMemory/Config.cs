// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer4.InMemory
{
    public static class Config
    {
          public static IEnumerable<IdentityResource> GetIdentityResources() =>
    new List<IdentityResource>
    {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
    };

        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource> {
                new ApiResource("api1"),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
                    new List<ApiScope>
                    {
                    new ApiScope("api1", "My API")
                    };
        public static IEnumerable<Client> GetClients() =>
            new List<Client> {
                new Client {
                    ClientId = "mvc",
                    ClientSecrets = { new Secret("secret".ToSha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,

                    RedirectUris = { "https://localhost:44385/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:44385/Home/Index" },

                    AllowedScopes = {
                        "api1",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    },

                    // puts all the claims in the id token
                    //AlwaysIncludeUserClaimsInIdToken = true,
                    AllowOfflineAccess = true,
                    RequireConsent = false,
                }
            };
    }
}