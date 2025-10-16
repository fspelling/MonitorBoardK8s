FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev
WORKDIR /app
EXPOSE 80
EXPOSE 7171

ARG USERNAME=dev
ARG USER_UID=1000
ARG USER_GID=1000

RUN groupadd --gid $USER_GID $USERNAME \
    && useradd --uid $USER_UID --gid $USER_GID -m $USERNAME \
    && apt-get update && apt-get install -y git curl vim

USER $USERNAME

CMD ["tail", "-f", "/dev/null"]
