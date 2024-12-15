using TT.API.Middleware;

namespace TT.API.Extensions;

public static class WebApplicationExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CustomExceptionMiddleware>();
    }
}
