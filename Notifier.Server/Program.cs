using Newtonsoft.Json.Serialization;
using Notifier.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR()
    .AddNewtonsoftJsonProtocol(options =>
    {
        // options.PayloadSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        options.PayloadSerializerSettings.ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        };
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