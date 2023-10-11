### GIT FILTER-REPO ###

## NÃO EXECUTE ESSE SCRIPT DIRETAMENTE
## Esse script foi feito para uso do
## script 'trybe-publisher' fornecido 
## pela Trybe. 

[[ $# == 1 ]] && \
[[ $1 == "trybe-security-parameter" ]] && \
git filter-repo \
    --path .trybe \
    --path .github \
    --path trybe.yml \
    --path trybe-filter-repo.sh \
    --path src/TrybeGames.Test.Test/Test.cs \
    --path src/TrybeGames.Test.Test/TestTestTrybeGamesDatabase.cs \
    --path src/TrybeGames.Test.Test/TrybeGames.Teste.Test.csproj \
    --path src/TrybeGames.Test.Test/Usings.cs \
    --path src/TrybeGames.Test.Test \
    --path img/complete-diagram.png \
    --path img/diagram-only-models.png \
    --path README.md \
    --invert-paths --force