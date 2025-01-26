FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /source

COPY --link . .

RUN dotnet build api
RUN dotnet run --project api
