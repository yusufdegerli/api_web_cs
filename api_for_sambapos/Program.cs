using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using api_for_sambapos.Models;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.WebSockets;
using System.Text;
using System.Collections.Concurrent;
using api_for_sambapos.Services;


var builder = WebApplication.CreateBuilder(args);

// CORS Politikası
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

// Controller ve JSON Ayarları
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressMapClientErrors = true;
    });

// DbContext Yapılandırması
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
    )
);

// Swagger Konfigürasyonu
builder.Services.AddSwaggerGen();
//builder.Services.AddSignalR();
builder.Services.AddHostedService<TableWatcher>();

// TLS Protokol Ayarları
ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

var app = builder.Build();

// Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseRouting();
app.UseWebSockets();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            await WebSocketHandler.HandleConnectionAsync(webSocket);
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    }
    else
    {
        await next();
    }
});

app.UseCors("AllowAllOrigins");
app.MapControllers();
//app.MapHub<OrderHub>("/orderhub");
//app.MapHub<NotificationHub>("/hubs/notification");

// Port ve IP Ayarları
var port = builder.Configuration.GetValue<string>("Port") ?? "5235";
app.Run($"http://192.168.56.1:{port}"); // Tüm ağ interfacelerini dinle