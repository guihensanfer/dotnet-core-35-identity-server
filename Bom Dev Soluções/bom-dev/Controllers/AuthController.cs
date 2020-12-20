using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;

namespace Bom_Dev.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthController : Controller
    {
        public class TokenViewModel
        {
            public string access_token { get; set; }
        }

        //public IActionResult Index()
        //{
        //    // get access token
        //    var serverClient = new HttpClient();
        //    serverClient.DefaultRequestHeaders.Accept.Add
        //(new MediaTypeWithQualityHeaderValue("application/text"));

        //    Dictionary<string, string> param =
        //new Dictionary<string, string>();
        //    param.Add("client_id", "client1");
        //    param.Add("client_secret", "client1_secret_code");
        //    param.Add("grant_type", "password");
        //    param.Add("username", "user1");
        //    param.Add("password", "password1");
        //    param.Add("scope", "employeesWebApi roles");

        //    var content = new FormUrlEncodedContent(param);
        //    var serverResponse = serverClient.PostAsync
        //("http://localhost:55565/connect/token", content).Result;
        //    string jsonData = serverResponse.Content.
        //ReadAsStringAsync().Result;

        //    var accessToken = JsonConvert.DeserializeObject<TokenViewModel>
        //(jsonData);

        //    // call web api

        //    var apiClient = new HttpClient();
        //    apiClient.DefaultRequestHeaders.Accept.Add
        //(new MediaTypeWithQualityHeaderValue("application/json"));

        //    apiClient.SetBearerToken(accessToken.access_token);

        //    var apiResponse = apiClient.GetAsync
        //("https://localhost:44302/auth").Result;
        //    var jsonApiData = apiResponse.Content.
        //ReadAsStringAsync().Result;            

        //    return View(jsonApiData);
        //}

        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add
        (new MediaTypeWithQualityHeaderValue("application/json"));

            var accessToken = HttpContext.GetTokenAsync
        (OpenIdConnectParameterNames.AccessToken).Result;

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                client.SetBearerToken(accessToken);
            }
            var response = client.GetAsync
        ("http://localhost:5020/Employees").Result;
            var jsonData = response.Content.
        ReadAsStringAsync().Result;
            List<string> data = JsonConvert.DeserializeObject<List<string>>(jsonData);
            return View(data);
        }
    }
}
