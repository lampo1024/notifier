using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Notifier.Server.DataAccess;
using Notifier.Server.Extensions;
using Notifier.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddRouting(options =>
//{
//    options.LowercaseUrls = true;
//});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(c =>
    {
        c.LoginPath = "/account/login";
    });
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
})
    .AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR()
    .AddNewtonsoftJsonProtocol(options =>
    {
        options.PayloadSerializerSettings.ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        };
    });

builder.Services.AddDbContext<NtfDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("MySQL"), MySqlServerVersion.LatestSupportedServerVersion);
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(builder =>
{
    builder
    .AllowAnyHeader()
    .SetIsOriginAllowed(o => true)
    .WithMethods("GET", "POST")
    .AllowCredentials();
});
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<NotificationHub>("/hub/notifier");
app.Run();