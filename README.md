# Web Project Developed in .NET Core 3.5 with Authentication Using Identity Server

This repository contains a solution developed with .NET Core 3.5, consisting of a Web MVC project integrated with IdentityServer4 for authentication via OAuth2/OpenID Connect. The system performs automatic login using the Authorization Code flow.

## Technologies Used

- .NET Core 3.5
- ASP.NET Core MVC
- IdentityServer4
- Microsoft SQL Server
- OAuth2 / OpenID Connect
- Cookie-based Authentication

## Project Structure

The solution is organized as follows:

- **BomDev.Web**: ASP.NET Core MVC application that acts as a client to IdentityServer4.
- **BomDev.IdentityServer**: Identity server responsible for authentication and token issuance.
