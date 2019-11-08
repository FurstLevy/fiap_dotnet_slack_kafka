FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /build
COPY ["src/Webhook.Slack.Worker/Webhook.Slack.Worker.csproj", "src/Webhook.Slack.Worker/"]
RUN dotnet restore "src/Webhook.Slack.Worker/Webhook.Slack.Worker.csproj"
COPY . .
WORKDIR /build/src/Webhook.Slack.Worker

FROM build AS publish
RUN dotnet publish "Webhook.Slack.Worker.csproj" -c Release -o /publish

FROM mcr.microsoft.com/dotnet/core/runtime:3.0 AS app
RUN sed -i 's/CipherString = DEFAULT@SECLEVEL=2/CipherString = DEFAULT@SECLEVEL=1/' /etc/ssl/openssl.cnf
WORKDIR /app
COPY --from=publish /publish .
ENTRYPOINT ["dotnet", "Webhook.Slack.Worker.dll"]
