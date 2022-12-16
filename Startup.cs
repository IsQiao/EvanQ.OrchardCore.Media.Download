using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using OrchardCore.Modules;

namespace EvanQ.OrchardCore.Media.Download
{
    public class Startup : StartupBase
    {
        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            app.UseMediaDownloadMiddleware();
        }
    }
}
