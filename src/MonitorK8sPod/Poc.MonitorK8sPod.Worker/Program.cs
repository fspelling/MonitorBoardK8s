using Poc.MonitorK8sPod.Application.Events.Handlers;
using Poc.MonitorK8sPod.IoC.DI;
using Poc.MonitorK8sPod.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMediatR(config => 
    config.RegisterServicesFromAssemblies([
        typeof(MessagingPodEventHandler).Assembly,
        typeof(Program).Assembly
    ]));

builder.Services.AddHostedService<Worker>();
builder.Services.RegisterServices(builder.Configuration);

var host = builder.Build();
host.Run();
