using Poc.MonitorK8sPod.Application.Events.Handlers;
using Poc.MonitorK8sPod.Worker;
using Poc.MonitorK8sPod.Worker.Config;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
{
    Args = args,
    EnvironmentName = environment
});

builder.Services.AddMediatR(config => 
    config.RegisterServicesFromAssemblies([
        typeof(MessagingPodEventHandler).Assembly,
        typeof(Program).Assembly
    ]));

builder.Services.AddHostedService<Worker>();

builder.Services.RegisterDI(builder.Configuration)
                .RegisterHttpClient(builder.Configuration)
                .RegisterMapperObjects();

var host = builder.Build();
host.Run();
