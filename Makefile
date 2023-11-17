# ─────────────────────────────────────────────────────
# Scripts para o ambiente Docker de desenvolvimento

compose_up_dev: # Ex: make compose_up_dev
	docker-compose -f docker-compose.dev.yml up -d

compose_down_dev: # Ex: make compose_down_dev
	docker-compose -f docker-compose.dev.yml down --remove-orphans

compose_rm_images_dev: # Ex: make compose_rm_images_dev
	docker image rm dev_trybe-games test_trybe-games evaluator-test_trybe-games

# ─────────────────────────────────────────────────────
# Scripts para o ambiente Docker de produção

compose_up_prod: # Ex: make compose_up_prod
	docker-compose -f docker-compose.prod.yml up -d

compose_down_prod: # Ex: make compose_down_prod
	docker-compose -f docker-compose.prod.yml down --remove-orphans

compose_rm_images_prod: # Ex: make compose_rm_images_prod
	docker image rm trybe-games

# ─────────────────────────────────────────────────────

start: # Ex: make start
	dotnet run --project ./src/TrybeGames/TrybeGames.csproj

dev: # Ex: make dev
	dotnet watch run --project ./src/TrybeGames/TrybeGames.csproj

restore: # Ex: make restore
	dotnet restore ./src

format-file: # Ex: make file-path=src/recipes-api/Controllers/RecipesController.cs format-file
	dotnet format ./src/src.sln --verify-no-changes --report ./dotnet-format.json --include ./$(file-path) diagnostic

test: # Ex: make test
	dotnet test ./src/src.sln

test-req: # Ex: make req-number=1 test-req
	dotnet test ./src/src.sln --filter 'TestReq$(req-number)'