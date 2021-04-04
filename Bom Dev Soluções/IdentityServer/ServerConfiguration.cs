using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public class ServerConfiguration
    {
        private string _clientBomDevURLBase;

        public ServerConfiguration(string clientBomDevURLBase)
        {
            if (string.IsNullOrEmpty(clientBomDevURLBase))
                throw new System.ArgumentNullException(nameof(clientBomDevURLBase), "Standard client Bom Dev is null.");

            _clientBomDevURLBase = clientBomDevURLBase;
        }

        public List<IdentityResource> IdentityResources {
            get
            {
                List<IdentityResource> idResources =
                new List<IdentityResource>();
                        idResources.Add(new IdentityResources.OpenId());
                        idResources.Add(new IdentityResources.Profile());
                        idResources.Add(new IdentityResources.Email());
                        idResources.Add(new IdentityResources.Phone());
                        idResources.Add(new IdentityResources.Address());
                        idResources.Add(new IdentityResource("roles",
                "User roles", new List<string> { "role" }));

                return idResources;
            }
        }
        public List<ApiScope> ApiScopes {
            get
            {
                List<ApiScope> apiScopes =
                    new List<ApiScope>();
                            apiScopes.Add(new ApiScope
                    ("employeesWebApi", "Employees Web API"));

                return apiScopes;
            }
        }
        public List<ApiResource> ApiResources {
            get
            {
                ApiResource apiResource1 = new
                ApiResource("employeesWebApiResource",
                "Employees Web API")
                {
                    Scopes = { "employeesWebApi" },
                    UserClaims = { "role",
                            "given_name",
                            "family_name",
                            "email",
                            "phone",
                            "address"
                            }
                };

                List<ApiResource> apiResources = new
                List<ApiResource>();
                        apiResources.Add(apiResource1);

                return apiResources;
            }
        }
        public List<Client> Clients {
            get
            {                
                Client client1 = new Client
                {
                    ClientId = "client1",
                    ClientName = "Client 1",
                    ClientSecrets = new[] {
                        new Secret("client1_secret_code".Sha512()) },
                    AllowedGrantTypes = GrantTypes.
                    Hybrid,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                        "employeesWebApi",
                        "roles"
                    },
                    RedirectUris = new List<string> {
                        $"{_clientBomDevURLBase}/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string> {
                         $"{_clientBomDevURLBase}/signout-callback-oidc"
                    },
                    RequirePkce = false,
                    RequireConsent = false,
                };

                Client client2 = new Client
                {                    
                    ClientName = "Client 2",
                    ClientId = "client2",
                    ClientSecrets = {
                        new Secret("client2_secret_code".Sha512())
                    },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                        "employeesWebApi",
                        "roles"
                    },                    
                    RedirectUris = new List<string> {
                        $"{_clientBomDevURLBase}/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string> {
                         $"{_clientBomDevURLBase}/signout-callback-oidc"
                    },                    
                    RequirePkce = false,
                    RequireConsent = true,
                    //AllowedCorsOrigins = new[] {  $"{MVCClientHTTPSBaseURL}/signin-oidc",  $"{MVCClientHTTPSBaseURL}/signout-callback-oidc" }
                };

                List<Client> clients = new List<Client>();
                clients.Add(client1);
                clients.Add(client2);

                return clients;
            }
        }        
    }
}
