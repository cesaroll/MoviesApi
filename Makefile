build:
	docker compose build

up:
	docker compose up -d --remove-orphans

down:
	docker compose down;

db:
	docker compose up postgresql -d --remove-orphans
