#pragma checksum "C:\Users\guihe\source\repos\bom-dev\Bom Dev Soluções\bom-dev\Views\Shared\Error.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fec634ae0b7b0fe2f0c74f7448053e075cd272b6"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Error), @"mvc.1.0.view", @"/Views/Shared/Error.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\guihe\source\repos\bom-dev\Bom Dev Soluções\bom-dev\Views\_ViewImports.cshtml"
using Bom_Dev;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\guihe\source\repos\bom-dev\Bom Dev Soluções\bom-dev\Views\_ViewImports.cshtml"
using Bom_Dev.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\guihe\source\repos\bom-dev\Bom Dev Soluções\bom-dev\Views\_ViewImports.cshtml"
using Data.Identity;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fec634ae0b7b0fe2f0c74f7448053e075cd272b6", @"/Views/Shared/Error.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"aec36cc0cbda5b9a0a5903b72fef2cbedd7e09a2", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Error : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ErrorViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 4 "C:\Users\guihe\source\repos\bom-dev\Bom Dev Soluções\bom-dev\Views\Shared\Error.cshtml"
  
    ViewData["Title"] = Localizer["PageTitle"];

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1 class=\"text-danger\">Localizer[\"PageTitle\"]</h1>\r\n<h2 class=\"text-danger\">Localizer[\"PageDescription\"]</h2>\r\n\r\n");
#nullable restore
#line 11 "C:\Users\guihe\source\repos\bom-dev\Bom Dev Soluções\bom-dev\Views\Shared\Error.cshtml"
 if (Model.ShowRequestId)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <p>\r\n        <strong>Request ID:</strong> <code>");
#nullable restore
#line 14 "C:\Users\guihe\source\repos\bom-dev\Bom Dev Soluções\bom-dev\Views\Shared\Error.cshtml"
                                      Write(Model.RequestId);

#line default
#line hidden
#nullable disable
            WriteLiteral("</code>\r\n    </p>\r\n");
#nullable restore
#line 16 "C:\Users\guihe\source\repos\bom-dev\Bom Dev Soluções\bom-dev\Views\Shared\Error.cshtml"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public Microsoft.AspNetCore.Mvc.Localization.IViewLocalizer Localizer { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ErrorViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
