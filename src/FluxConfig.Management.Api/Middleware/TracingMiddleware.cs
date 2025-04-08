namespace FluxConfig.Management.Api.Middleware;

public class TracingMiddleware
{
    private readonly RequestDelegate _next;

    public TracingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        context.TraceIdentifier = Guid.NewGuid().ToString();
        context.Response.Headers["X-Trace-Id"] = context.TraceIdentifier;
        await _next.Invoke(context);
    }
}