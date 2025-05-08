# Bom Dev - Solução de Aplicações

Este repositório contém uma solução desenvolvida com .NET Core 3.5, composta por um projeto Web MVC que integra com o IdentityServer4 para autenticação via OAuth2/OpenID Connect. O sistema realiza login automático utilizando o fluxo Authorization Code.

## Tecnologias Utilizadas

- .NET Core 3.5
- ASP.NET Core MVC
- IdentityServer4
- OAuth2 / OpenID Connect
- Autenticação baseada em Cookies

## Estrutura do Projeto

A solução está organizada da seguinte forma:

- **BomDev.Web**: Aplicação ASP.NET Core MVC que atua como cliente do IdentityServer4.
- **BomDev.IdentityServer**: Servidor de identidade responsável pela autenticação e emissão de tokens.

## No projeto MVC, a autenticação é configurada no Startup.cs utilizando o middleware de OpenID Connect:

services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("Cookies")
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = "https://localhost:5001";
    options.ClientId = "mvc";
    options.ClientSecret = "secret";
    options.ResponseType = "code";
    options.SaveTokens = true;
});
