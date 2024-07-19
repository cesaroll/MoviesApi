build:
	docker compose build

up:
	docker compose up -d --remove-orphans

down:
	docker compose down;

db:
	docker compose up postgresql -d --remove-orphans

run:
	dotnet run --project Movies.Api/Movies.Api.csproj

identity:
	dotnet run --project Identity.Api/Identity.Api.csproj
