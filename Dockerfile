# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic AS build
WORKDIR /source
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf

# copy csproj and restore as distinct layers
COPY *.sln .
COPY ParagonTestApplication/*.csproj ./ParagonTestApplication/
COPY ParagonTestApplication.Data/*.csproj ./ParagonTestApplication.Data/
COPY ParagonTestApplication.Models/*.csproj ./ParagonTestApplication.Models/
RUN dotnet restore ./ParagonTestApplication/ParagonTestApplication.csproj
RUN dotnet restore ./ParagonTestApplication.Data/ParagonTestApplication.Data.csproj
RUN dotnet restore ./ParagonTestApplication.Models/ParagonTestApplication.Models.csproj

# copy everything else and build app
COPY ParagonTestApplication/. ./ParagonTestApplication/
COPY ParagonTestApplication.Data/. ./ParagonTestApplication.Data/
COPY ParagonTestApplication.Models/. ./ParagonTestApplication.Models/
RUN dotnet publish ./ParagonTestApplication/ParagonTestApplication.csproj -c release -o /app --no-restore 
RUN dotnet publish ./ParagonTestApplication.Data/ParagonTestApplication.Data.csproj -c release -o /app --no-restore 
RUN dotnet publish ./ParagonTestApplication.Models/ParagonTestApplication.Models.csproj -c release -o /app --no-restore 

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "ParagonTestApplication.dll"]