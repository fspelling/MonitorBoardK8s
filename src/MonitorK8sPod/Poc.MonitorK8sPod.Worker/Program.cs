using Poc.MonitorK8sPod.Application.Events.Handlers;
using Poc.MonitorK8sPod.Worker;
using Poc.MonitorK8sPod.Worker.Config;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMediatR(config => 
    config.RegisterServicesFromAssemblies([
        typeof(MessagingPodEventHandler).Assembly,
        typeof(Program).Assembly
    ]));

builder.Services.AddHostedService<Worker>();

builder.Services.RegisterDI(builder.Configuration)
                .RegisterHttpClient()
                .RegisterMapperObjects();

var host = builder.Build();
host.Run();
