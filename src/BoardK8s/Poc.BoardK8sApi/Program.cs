using Carter;
using FluentValidation;
using Poc.BoardK8sApi.Features.Pods;
using Poc.BoardK8sApi.Infra;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

builder.Services.AddCarter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(assembly));
builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddSingleton<KubernetesClient>();
builder.Services.AddSingleton(sp => sp.GetRequiredService<KubernetesClient>().Client);

builder.Services.AddScoped<GetPods.KubernetesService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapCarter();
app.UseHttpsRedirection();

app.Run();