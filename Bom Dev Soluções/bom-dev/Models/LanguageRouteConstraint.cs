using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bom_Dev.Models
{
    public class LanguageRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey("culture"))
            {
                return false;
            }

            var lang = values["culture"]?.ToString().ToLower();

            return lang == "pt-br" || lang == "en-us";
        }
    }
}
