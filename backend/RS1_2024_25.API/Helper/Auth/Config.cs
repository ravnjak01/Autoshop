namespace RS1_2024_25.API.Helper.Auth
{
    using Duende.IdentityServer.Models;

    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
            new ApiScope("autoshop.api", "Auto Shop API")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
            new ApiResource("autoshop.api")
            {
                Scopes = { "autoshop.api" }
            }
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
            new Client
            {
                ClientId = "autoshop.client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = { "autoshop.api" }
            }
            };
    }

}
