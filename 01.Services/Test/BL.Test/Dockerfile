#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["01.Services/Test/BL.Test/BL.Test.csproj", "01.Services/Test/BL.Test/"]
COPY ["01.Services/Test/BL.Test.Infrastructure/BL.Test.Infrastructure.csproj", "01.Services/Test/BL.Test.Infrastructure/"]
COPY ["01.Services/Test/BL.Test.ApplicationServices/BL.Test.ApplicationServices.csproj", "01.Services/Test/BL.Test.ApplicationServices/"]
COPY ["01.Services/Test/BL.Test.Domain/BL.Test.Domain.csproj", "01.Services/Test/BL.Test.Domain/"]
COPY ["00.Framework/BL.Framework/BL.Framework.csproj", "00.Framework/BL.Framework/"]
COPY ["00.Framework/BL.Framework.Core/BL.Framework.Core.csproj", "00.Framework/BL.Framework.Core/"]
COPY ["00.Framework/BL.Framework.Persistence.MongoDB/BL.Framework.Persistence.MongoDB.csproj", "00.Framework/BL.Framework.Persistence.MongoDB/"]
COPY ["00.Framework/BL.Framework.Persistence.EntityFrameworkCore/BL.Framework.Persistence.EntityFrameworkCore.csproj", "00.Framework/BL.Framework.Persistence.EntityFrameworkCore/"]
RUN dotnet restore "01.Services/Test/BL.Test/BL.Test.csproj"
COPY . .
WORKDIR "/src/01.Services/Test/BL.Test"
RUN dotnet build "BL.Test.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BL.Test.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BL.Test.dll"]