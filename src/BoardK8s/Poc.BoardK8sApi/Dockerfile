FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 7171

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /build
COPY ["src/BoardK8s/Poc.BoardK8sApi/Poc.BoardK8sApi.csproj", "BoardK8s/Poc.BoardK8sApi/"]
RUN dotnet restore "BoardK8s/Poc.BoardK8sApi/Poc.BoardK8sApi.csproj"
COPY . .
WORKDIR /build/src/BoardK8s/Poc.BoardK8sApi
RUN dotnet build "Poc.BoardK8sApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Poc.BoardK8sApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN groupadd --gid $USER_GID $USERNAME \
    && useradd --uid $USER_UID --gid $USER_GID -m $USERNAME
USER $USERNAME
ENTRYPOINT ["dotnet", "Poc.BoardK8sApi.dll"]