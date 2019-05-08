##BUILDER
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS builder
WORKDIR /builder
COPY . .
RUN dotnet test VotingApp.Tests
RUN dotnet publish VotingApp.Api -o /build 

#RUNTIME IMAGE
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=builder /build .
ENTRYPOINT [ "dotnet",  "VotingApp.Api.dll" ]
ENV ASPNETCORE_ENVIRONMENT=Development