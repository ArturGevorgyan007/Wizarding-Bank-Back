#docker build -t [name of image] .
#docker run -d -p 3450:80  [name of image]
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS BUILD

WORKDIR /app

COPY API ./API
COPY DataAccess ./DataAccess
COPY Services ./Services

RUN dotnet publish API -o dist

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS RUN

WORKDIR /app

COPY --from=BUILD app/dist .

CMD [ "dotnet", "API.dll" ]