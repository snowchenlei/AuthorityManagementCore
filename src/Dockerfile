FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Snow.AuthorityManagement.Web.Mvc/Snow.AuthorityManagement.Web.Mvc.csproj", "Snow.AuthorityManagement.Web.Mvc/"]
COPY ["Anc.Web.Mvc/Anc.AspNetCore.csproj", "Anc.Web.Mvc/"]
COPY ["Anc/Anc.csproj", "Anc/"]
COPY ["Snow.AuthorityManagement.Repository/Snow.AuthorityManagement.Repository.csproj", "Snow.AuthorityManagement.Repository/"]
COPY ["Snow.AuthorityManagement.Core/Snow.AuthorityManagement.Core.csproj", "Snow.AuthorityManagement.Core/"]
COPY ["Snow.AuthorityManagement.Data/Snow.AuthorityManagement.Data.csproj", "Snow.AuthorityManagement.Data/"]
COPY ["Anc.Dapper/Anc.Dapper.csproj", "Anc.Dapper/"]
COPY ["Anc.EntityFrameworkCore/Anc.EntityFrameworkCore.csproj", "Anc.EntityFrameworkCore/"]
COPY ["Snow.AuthorityManagement.Common/Snow.AuthorityManagement.Common.csproj", "Snow.AuthorityManagement.Common/"]
COPY ["Snow.AuthorityManagement.Web.Core/Snow.AuthorityManagement.Web.Core.csproj", "Snow.AuthorityManagement.Web.Core/"]
COPY ["Snow.AuthorityManagement.Application/Snow.AuthorityManagement.Application.csproj", "Snow.AuthorityManagement.Application/"]
RUN dotnet restore "Snow.AuthorityManagement.Web.Mvc/Snow.AuthorityManagement.Web.Mvc.csproj"
COPY . .
WORKDIR "/src/Snow.AuthorityManagement.Web.Mvc"
RUN dotnet build "Snow.AuthorityManagement.Web.Mvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Snow.AuthorityManagement.Web.Mvc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Snow.AuthorityManagement.Web.Mvc.dll"]