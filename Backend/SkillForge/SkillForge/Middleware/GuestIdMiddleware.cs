namespace SkillForge.Middleware;

public class GuestIdMiddleware
{
    private readonly RequestDelegate _next;

    public GuestIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string? consent = context.Request.Cookies["cookie_consent"];

        if (consent == "true")
        {
            if (!context.Request.Cookies.ContainsKey("guest_id"))
            {
                var guestId = Guid.NewGuid().ToString();

                context.Response.Cookies.Append("guest_id", guestId, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddYears(1)
                });
            }
        }

        await _next(context);
    }
}
