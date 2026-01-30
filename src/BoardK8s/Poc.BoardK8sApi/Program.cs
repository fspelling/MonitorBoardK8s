using Carter;
using FluentValidation;
using Poc.BoardK8sApi.Features.Pods;
using Poc.BoardK8sApi.Infra;
using Scalar.AspNetCore;
using Poc.BoardK8sApi.Mappers;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddSignalR();
builder.Services.AddCarter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(assembly));
builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddSingleton<KubernetesClient>();
builder.Services.AddSingleton(sp => sp.GetRequiredService<KubernetesClient>().Client);

builder.Services.AddScoped<GetPods.KubernetesService>();

MapperConfig.Register();

var app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        var baseUrl = Environment.GetEnvironmentVariable("OPENAPI_BASE_URL") ?? "http://localhost:8080";
        options.Servers = new List<ScalarServer> { new ScalarServer(baseUrl) };
    });
}

app.MapHub<NotificationPod.NotificationPodHub>("notification-pod");
app.MapCarter();
app.UseHttpsRedirection();

app.Run();

// TODO: somente teste 4
