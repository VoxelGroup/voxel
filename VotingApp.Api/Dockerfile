##BUILDER
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS builder
WORKDIR /builder

COPY VotingApp.Api/VotingApp.Api.csproj ./VotingApp.Api/
COPY VotingApp.Lib/VotingApp.Lib.csproj ./VotingApp.Lib/
COPY ClassicLogging/ClassicLogging.csproj ./ClassicLogging/
COPY VotingApp.Tests/VotingApp.Tests.csproj ./VotingApp.Tests/

RUN dotnet restore VotingApp.Api
RUN dotnet restore VotingApp.Tests

COPY . .

RUN dotnet test VotingApp.Tests
RUN dotnet publish VotingApp.Api -o /build 

#RUNTIME IMAGE
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=builder /build .
ENTRYPOINT [ "dotnet",  "VotingApp.Api.dll" ]
ENV ASPNETCORE_ENVIRONMENT=Development