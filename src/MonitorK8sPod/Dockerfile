FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /build

COPY Poc.MonitorK8sPod.Application/Poc.MonitorK8sPod.Application.csproj Poc.MonitorK8sPod.Application/
COPY Poc.MonitorK8sPod.Domain/Poc.MonitorK8sPod.Domain.csproj Poc.MonitorK8sPod.Domain/
COPY Poc.MonitorK8sPod.Shared/Poc.MonitorK8sPod.Shared.csproj Poc.MonitorK8sPod.Shared/
COPY Poc.MonitorK8sPod.Worker/Poc.MonitorK8sPod.Worker.csproj Poc.MonitorK8sPod.Worker/
COPY Poc.MonitorK8sPod.Infra/Poc.MonitorK8sPod.Infra.csproj Poc.MonitorK8sPod.Infra/
COPY Poc.MonitorK8sPod.IoC/Poc.MonitorK8sPod.IoC.csproj Poc.MonitorK8sPod.IoC/

RUN dotnet restore "Poc.MonitorK8sPod.Worker/Poc.MonitorK8sPod.Worker.csproj"

COPY . .

WORKDIR /build/Poc.MonitorK8sPod.Worker
RUN dotnet build "Poc.MonitorK8sPod.Worker.csproj" -c Release -o /app/

FROM build AS publish
RUN dotnet publish "Poc.MonitorK8sPod.Worker.csproj" -c Release -o /app/publish

FROM base AS final

ARG USERNAME=dev
ARG USER_UID=1000
ARG USER_GID=1000

WORKDIR /app
COPY --from=publish /app/publish .

RUN groupadd --gid $USER_GID $USERNAME \
    && useradd --uid $USER_UID --gid $USER_GID -m $USERNAME
USER $USERNAME

ENTRYPOINT ["dotnet", "Poc.MonitorK8sPod.Worker.dll"]