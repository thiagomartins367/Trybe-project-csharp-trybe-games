namespace TrybeGames;

public class TrybeGamesDatabase
{
    public List<Game> Games = new List<Game>();

    public List<GameStudio> GameStudios = new List<GameStudio>();

    public List<Player> Players = new List<Player>();

    // 4. Crie a funcionalidade de buscar jogos desenvolvidos por um estúdio de jogos
    public List<Game> GetGamesDevelopedBy(GameStudio gameStudio)
    {
        var gamesDeveloped = from game in Games
                             where game.DeveloperStudio == gameStudio.Id
                             select game;
        return gamesDeveloped.ToList();
    }

    // 5. Crie a funcionalidade de buscar jogos jogados por uma pessoa jogadora
    public List<Game> GetGamesPlayedBy(Player player)
    {
        var gamesPlayed = from game in Games
                          where game.Players.Contains(player.Id)
                          select game;
        return gamesPlayed.ToList();
    }

    // 6. Crie a funcionalidade de buscar jogos comprados por uma pessoa jogadora
    public List<Game> GetGamesOwnedBy(Player playerEntry)
    {
        var gamesOwned = from game in Games
                         where playerEntry.GamesOwned.Contains(game.Id)
                         select game;
        return gamesOwned.ToList();
    }

    // 7. Crie a funcionalidade de buscar todos os jogos junto do nome do estúdio desenvolvedor
    public List<GameWithStudio> GetGamesWithStudio()
    {
        return Games.Join(
            GameStudios,
            Game => Game.DeveloperStudio,
            GameStudio => GameStudio.Id,
            (Game, GameStudio) => new GameWithStudio
            {
                GameName = Game.Name,
                StudioName = GameStudio.Name,
                NumberOfPlayers = Game.Players.Count
            }
        ).ToList();
    }

    // 8. Crie a funcionalidade de buscar todos os diferentes Tipos de jogos dentre os jogos cadastrados
    public List<GameType> GetGameTypes()
    {
        // Implementar
        throw new NotImplementedException();
    }

    // 9. Crie a funcionalidade de buscar todos os estúdios de jogos junto dos seus jogos desenvolvidos com suas pessoas jogadoras
    public List<StudioGamesPlayers> GetStudiosWithGamesAndPlayers()
    {
        // Implementar
        throw new NotImplementedException();
    }

}
