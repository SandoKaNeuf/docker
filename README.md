## Get started with Docker for Windows

https://docs.docker.com/docker-for-windows/

29/11_ ## Deploy a .NET Core API with Docker ***

29/11_ https://dotnetplaybook.com/deploy-a-net-core-api-with-docker/
## Libère les ports, nécessite un reboot.
netcfg -d

## Url not found 
## Problème de ports 
https://github.com/angular/angular-cli/issues/2375

https://stackoverflow.com/questions/17260766/how-do-i-get-asp-net-web-api-self-hosted-to-listen-on-only-localhost

https://stackoverflow.com/questions/19554996/cant-access-web-api-with-ipport-but-can-with-localhostport-during-vs-debug-mo
1. replace localhost par 0.0.0.0 dans lauchSettings.json
"applicationUrl": "https://localhost:5001;https://0.0.0.0;http://0.0.0.0", // https://localhost:5001;http://localhost:5000

$ dotnet run => ok http://localhost/weatherforecast
$ docker build -t ysance/simpleapi .
$ docker run -p 8080:80 ysance/simpleapi

=> info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:5000
=> info: Microsoft.Hosting.Lifetime[0]
      Now listening on: https://localhost:5001
Hors, j'ai changé http://localhost:5000 dans tout le projet simpleApi, 
Comment se fait-il qu'il réaparaisse au run. 
A moins que ce soit l'image de base du Dockerfile :
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env

=> Test KO http://localhost:8080/weatherforecast 

## Repartir du container :
$ docker exec -i ysance/simpleapi sh # enter the container
$ curl https://localhost:5001/weatherforecast
=> Error certificate
curl: (60) SSL certificate problem: unable to get local issuer certificate
More details here: https://curl.haxx.se/docs/sslcerts.html
$ curl http://0.0.0.0:80/weatherforecast
=> Error Failed to connect to 0.0.0.0 port 80: Connection refused
$ curl http://0.0.0.0/weatherforecast
$ curl http://localhost/weatherforecast
=> Error Failed to connect to localhost port 80: Connection refused
$ curl http://localhost:5001/weatherforecast
=> curl: (52) Empty reply from server

## Refaire l'image avec http au lieu de https
=> Test http://localhost/weatherforecast et localhost:5001/weatherforecast OK navigateur

## Build et Run
$  docker build -t ysance/simpleapi .
$ docker run --rm -p 8080:80 ysance/simpleapi
=> Navigateur tout est KO 
## Repartir du container :
$ docker exec -i 041d9c8a9ccd sh # enter the container
$ curl http://localhost:5001/weatherforecast
=> curl: (52) Empty reply from server
$ curl http://localhost/weatherforecast
=>Failed to connect to localhost port 80: Connection refused

## Sortir du container 
$ curl http://localhost:5001/weatherforecast
=> Failed to connect to localhost port 5001: Connection refused

## Run le container sur le port 5001
Changer dans le Dockerfile EXPOSE 5001
$ docker build -t ysance/simpleapi .
$ docker run --rm -p 8080:5001 -n simpleApi ysance/simpleapi
$ docker exec -i f803c481c1e0 sh # enter the container
$ curl http://localhost:5001/weatherforecast
=> curl: (52) Empty reply from server
## Sortir du container Ctrl C
## Show IP Container :
$ docker inspect -f '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' f803c481c1e0
172.17.0.2
$ curl http://172.17.0.2:5001/weatherforecast
$ curl http://172.17.0.2:5000/weatherforecast
$ curl http://172.17.0.2/weatherforecast
$ curl http://172.17.0.2
=> Error  Failed to connect to 172.17.0.2 port 5001: Timed out

$ curl http://localhost:8080/weatherforecast
=> curl: (52) Empty reply from server

## Dans le container
$ curl -v -b 0.0.0.0:5001 http://localhost:5001/weatherforecast
curl -v -host 0.0.0.0:5001 http://localhost:5001/weatherforecast


$ docker ps
CONTAINER ID        IMAGE               COMMAND                  CREATED             STATUS              PORTS                    NAMES
f803c481c1e0        ysance/simpleapi    "dotnet SimpleAPI.dll"   13 minutes ago      Up 13 minutes       0.0.0.0:8080->5001/tcp   vigilant_mccarthy


## J'ai changé dans Dockerfile 
# Generate runtime image
FROM mcr.microsoft.com/dotnet/core/sdk:3.0
par
# Generate runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0

$ docker run --rm -P -h 0.0.0.0 --name simpleapi -p 8080:80 ysance/simpleapi
$ docker exec -i simpleapi sh # enter the container

  Fonctionne également avec :
  $ docker run --rm --name simpleapi -p 8080:80 ysance/simpleapi

curl http://localhost:5001/weatherforecast
=> curl: (7) Failed to connect to localhost port 5001: Connection refused

curl http://localhost/weatherforecast
=> YESSSSSSS!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
curl http://localhost:5001/weatherforecast
curl http://localhost:5001/weatherforecast
curl http://localhost:5001/weatherforecast

Donc au final :
1er erreur La version du sdk trop ancienne 
=> monter de version dans le dockerfile
2e erreur La runtime image dotnet/core/aspnet:3.0 et non dotnet/core/sdk:3.0 

## Reprise du tuto...
Déploiment dur Azure

$ dotnet new webapi -n SimpleAPI

## A comparer avec (en local) ASP.NET Tutorial - Hello World in 10 minutes

https://dotnet.microsoft.com/learn/aspnet/hello-world-tutorial/intro


_03/12 ## Dockerize a .NET Core application

https://docs.docker.com/v17.12/engine/examples/dotnetcore/
=> Ok Run on navigateur :
http://localhost:8080/Home/Contact

_03/12 ## Running a .NET Core Web Application in Docker container using Docker Desktop for Windows

https://devblogs.microsoft.com/premier-developer/running-a-net-core-web-application-in-docker-container-using-docker-desktop-for-windows/
=> Même doc que docker mais un peu plus détaillé

_03/12 ## Complément sur les webapi .NET Core

https://dotnetplaybook.com/develop-a-rest-api-with-net-core/
Création d'une api rest et explication du fonctionnement de l'api, workflow très détaillé.***

## Details on ActionResult Type
https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-2.2


## Error on install Entity frameworkCore => version of F has to be the same as NetCore version 3.0.0
$ dotnet add package Microsoft.EntityFrameworkCore.Tools --version 3.0.0
$ dotnet tool install --global dotnet-ef --version 3.0.0
$ dotnet ef

## Instead of create a SQL Servr instance, I want to use a MariaDb in a container
$ docker run --name some-mariadb -e MYSQL_ROOT_PASSWORD=passe -d mariadb:10.3
$ docker run --rm -p 3306:3306 --name some-mariadb -e MYSQL_ROOT_PASSWORD=passe -d mariadb:10
$ docker exec -i some-mariadb bash
$ mysql --user="root" --password="passe" --execute='CREATE DATABASE 'abcxtest';'
=> Error ERROR 1045 (28000): Access denied for user 'root'@'localhost' (using password: YES)

Stop at Revisit the Startup Class

## Using .NET and Docker Together

https://devblogs.microsoft.com/dotnet/using-net-and-docker-together/

## Azure

https://azure.microsoft.com/en-au/services/kubernetes-service/

## 

https://medium.com/net-core
https://medium.com/net-core/how-to-build-a-restful-api-with-asp-net-core-fb7dd8d3e5e3

## Guide .NET Core

https://docs.microsoft.com/fr-fr/dotnet/core/

## Entity Framework Core

https://docs.microsoft.com/fr-fr/ef/core/

=>ToDO 
Container dotnet 
Container mariadb
Unity C# appelle service dotnet via docker

Kubernetes

