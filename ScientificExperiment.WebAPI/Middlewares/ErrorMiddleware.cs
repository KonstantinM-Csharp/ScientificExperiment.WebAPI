using static WebApi_CSV.Exceptions.CustomExceptions;

namespace WebApi_CSV.Middlewares
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = 419;
                await context.Response.WriteAsJsonAsync(ex.Message);
            }
            catch (CSVException ex)
            {
                context.Response.StatusCode = 419;
                await context.Response.WriteAsJsonAsync(ex.Message);
            }

            catch (ValidationException ex)
            {
                context.Response.StatusCode = 415;
                await context.Response.WriteAsJsonAsync(ex.Message);
            }
            catch (SizeValidationException ex)
            {
                context.Response.StatusCode = 422;
                await context.Response.WriteAsJsonAsync(ex.Message);
            }
        }
    }
    public static class ErrorMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalErrorWrapper(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorMiddleware>();
        }
    }
}
