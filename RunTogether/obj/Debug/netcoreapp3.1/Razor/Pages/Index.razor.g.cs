#pragma checksum "C:\Users\mads\OneDrive\C\Dokumenter\GitHub\RunTogether\RunTogether\Pages\Index.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0f8377e3fb2a25a8e3369e0b5eb23cc83b7c079b"
// <auto-generated/>
#pragma warning disable 1591
namespace RunTogether.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\mads\OneDrive\C\Dokumenter\GitHub\RunTogether\RunTogether\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\mads\OneDrive\C\Dokumenter\GitHub\RunTogether\RunTogether\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\mads\OneDrive\C\Dokumenter\GitHub\RunTogether\RunTogether\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\mads\OneDrive\C\Dokumenter\GitHub\RunTogether\RunTogether\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\mads\OneDrive\C\Dokumenter\GitHub\RunTogether\RunTogether\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\mads\OneDrive\C\Dokumenter\GitHub\RunTogether\RunTogether\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\mads\OneDrive\C\Dokumenter\GitHub\RunTogether\RunTogether\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\mads\OneDrive\C\Dokumenter\GitHub\RunTogether\RunTogether\_Imports.razor"
using RunTogether;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\mads\OneDrive\C\Dokumenter\GitHub\RunTogether\RunTogether\_Imports.razor"
using RunTogether.Shared;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/")]
    public partial class Index : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<h1>Map</h1>\r\n\r\n\r\n");
            __builder.OpenElement(1, "div");
            __builder.AddAttribute(2, "onmousemove", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 11 "C:\Users\mads\OneDrive\C\Dokumenter\GitHub\RunTogether\RunTogether\Pages\Index.razor"
                                Mouse_Move

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(3, "onmousedown", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 11 "C:\Users\mads\OneDrive\C\Dokumenter\GitHub\RunTogether\RunTogether\Pages\Index.razor"
                                                          CreateMarker

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(4, "id", "mapid");
            __builder.AddElementReferenceCapture(5, (__value) => {
#nullable restore
#line 11 "C:\Users\mads\OneDrive\C\Dokumenter\GitHub\RunTogether\RunTogether\Pages\Index.razor"
           mapid = __value;

#line default
#line hidden
#nullable disable
            }
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(6, "\r\n\r\n");
            __builder.OpenElement(7, "p");
            __builder.AddContent(8, 
#nullable restore
#line 13 "C:\Users\mads\OneDrive\C\Dokumenter\GitHub\RunTogether\RunTogether\Pages\Index.razor"
    Coordinates

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 16 "C:\Users\mads\OneDrive\C\Dokumenter\GitHub\RunTogether\RunTogether\Pages\Index.razor"
       

    ElementReference mapid;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JsRunTime.InvokeVoidAsync("Main.leaflet_start");
            firstRender = false;
        }

    }

    protected string Coordinates { get; set; }

    protected void Mouse_Move(MouseEventArgs e)
    {
        Coordinates = $"X = {e.ClientX } Y = {e.ClientY}";
    }

    private async void CreateMarker()
    {

        await JsRunTime.InvokeVoidAsync("Main.onMapClick");

    }


#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JsRunTime { get; set; }
    }
}
#pragma warning restore 1591
