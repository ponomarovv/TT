namespace TT.API.Extensions;

public static class WebApplicationExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<TT.API.Middleware.ExceptionMiddleware>();
    }
}
