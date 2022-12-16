using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Net.Http.Headers;
using OrchardCore.Media;

namespace EvanQ.OrchardCore.Media.Download;

public class MediaDownloadMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMediaFileStore _mediaFileStore;

    public MediaDownloadMiddleware(RequestDelegate next, IMediaFileStore mediaFileStore)
    {
        _next = next;
        _mediaFileStore = mediaFileStore;
    }

    public Task Invoke(HttpContext httpContext)
    {
        if (httpContext.Request.Method != HttpMethods.Get)
        {
            return _next.Invoke(httpContext);
        }

        var mediaUrlPrefix = _mediaFileStore.MapPathToPublicUrl("");
        var mediaUrlPrefixPath = new Uri(mediaUrlPrefix).AbsolutePath;
        var currentPath = UriHelper.BuildRelative(httpContext.Request.PathBase, httpContext.Request.Path);
        if (!currentPath.ToLower().StartsWith(mediaUrlPrefixPath.ToLower()))
        {
            return _next.Invoke(httpContext);
        }

        var download = httpContext.Request.Query["download"].ToString();
        if (download != "true" && download != "1")
        {
            return _next.Invoke(httpContext);
        }

        var fileName = Path.GetFileName(httpContext.Request.Path);
        var contentDispositionHeader = new ContentDispositionHeaderValue("attachment");
        contentDispositionHeader.SetHttpFileName(fileName);
        var contentDispositionHeaderString = contentDispositionHeader.ToString();

        var watch = new Stopwatch();
        watch.Start();
        httpContext.Response.OnStarting(state =>
        {
            watch.Stop();
            var ctx = (HttpContext)state;
            ctx.Response.Headers.Add(HeaderNames.ContentDisposition, contentDispositionHeaderString);
            return Task.CompletedTask;
        }, httpContext);

        return _next.Invoke(httpContext);
    }
}

public static class MediaDownloadMiddlewareExtensions
{
    public static IApplicationBuilder UseMediaDownloadMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MediaDownloadMiddleware>();
    }
}