namespace BlogAPI.Filters
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder app)
            => app.UseMiddleware<GlobalExceptionHandler>();
    }
}
