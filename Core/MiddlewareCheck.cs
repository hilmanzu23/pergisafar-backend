

using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

public class JwtTokenExpirationMiddleware
{
    private readonly RequestDelegate _next;

    public JwtTokenExpirationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var authenticationService = context.RequestServices.GetRequiredService<IAuthenticationService>();
        var result = await authenticationService.AuthenticateAsync(context, JwtBearerDefaults.AuthenticationScheme);
        Console.WriteLine(result.Principal);
        if (!result.Succeeded)
        {
            // Authentication failed, handle it accordingly
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        // Check token expiration
        if (result.Principal != null)
        {
            var expirationClaim = result.Principal.FindFirst(JwtRegisteredClaimNames.Exp);
            if (expirationClaim != null && DateTime.TryParse(expirationClaim.Value, out var expiration))
            {
                if (DateTime.UtcNow >= expiration)
                {
                    // Token has expired, send a custom error response
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync("Token has expired");
                    return;
                }
            }
        }

        // Token is valid, proceed with the request
        await _next(context);
    }


}

