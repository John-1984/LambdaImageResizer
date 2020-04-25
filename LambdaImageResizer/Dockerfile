FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY LambdaImageResizer/LambdaImageResizer.csproj LambdaImageResizer/
RUN dotnet restore LambdaImageResizer/LambdaImageResizer.csproj
COPY . .
WORKDIR /src/LambdaImageResizer
RUN dotnet build LambdaImageResizer.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish LambdaImageResizer.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "LambdaImageResizer.dll"]
