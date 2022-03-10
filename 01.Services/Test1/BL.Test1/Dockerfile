#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["01.Services/Test1/BL.Test1/BL.Test1.csproj", "01.Services/Test1/BL.Test1/"]
COPY ["01.Services/Test1/BL.Test1.Infrastructure/BL.Test1.Infrastructure.csproj", "01.Services/Test1/BL.Test1.Infrastructure/"]
COPY ["01.Services/Test1/BL.Test1.ApplicationServices/BL.Test1.ApplicationServices.csproj", "01.Services/Test1/BL.Test1.ApplicationServices/"]
COPY ["01.Services/Test1/BL.Test1.Domain/BL.Test1.Domain.csproj", "01.Services/Test1/BL.Test1.Domain/"]
COPY ["00.Framework/BL.Framework/BL.Framework.csproj", "00.Framework/BL.Framework/"]
COPY ["00.Framework/BL.Framework.Core/BL.Framework.Core.csproj", "00.Framework/BL.Framework.Core/"]
COPY ["00.Framework/BL.Framework.Persistence.MongoDB/BL.Framework.Persistence.MongoDB.csproj", "00.Framework/BL.Framework.Persistence.MongoDB/"]
COPY ["00.Framework/BL.Framework.Persistence.EntityFrameworkCore/BL.Framework.Persistence.EntityFrameworkCore.csproj", "00.Framework/BL.Framework.Persistence.EntityFrameworkCore/"]
RUN dotnet restore "01.Services/Test1/BL.Test1/BL.Test1.csproj"
COPY . .
WORKDIR "/src/01.Services/Test1/BL.Test1"
RUN dotnet build "BL.Test1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BL.Test1.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BL.Test1.dll"]