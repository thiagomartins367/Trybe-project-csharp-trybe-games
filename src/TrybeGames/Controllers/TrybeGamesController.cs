using System.Globalization;

namespace TrybeGames;
public class TrybeGamesController
{
    public TrybeGamesDatabase database;

    public IConsole Console;

    public TrybeGamesController(TrybeGamesDatabase database, IConsole console)
    {
        this.database = database;
        this.Console = console;
    }

    public void RemovePlayerFromGame(Game game)
    {
        var playersPlayingGame = database.Players.Where(p => game.Players.Contains(p.Id)).ToList();
        var player = SelectPlayer(playersPlayingGame);
        if (player == null)
        {
            Console.WriteLine("Pessoa jogadora não encontrado!");
            return;
        }
        game.RemovePlayer(player);
        Console.WriteLine("Pessoa jogadora removido com sucesso!");
    }

    public void AddPlayerToGame(Game gameToAdd)
    {
        var playersNotPlayingGame = database.Players.Where(p => !gameToAdd.Players.Contains(p.Id)).ToList();
        var player = SelectPlayer(playersNotPlayingGame);
        if (player == null)
        {
            Console.WriteLine("Pessoa jogadora não encontrada!");
            return;
        }
        gameToAdd.AddPlayer(player);
        Console.WriteLine("Pessoa jogadora adicionada com sucesso!");
    }

    public void QueryGamesFromStudio()
    {
        var gameStudio = SelectGameStudio(database.GameStudios);
        if (gameStudio == null)
        {
            Console.WriteLine("Opção inválida! Tente novamente.");
            return;
        }
        try {
            var games = database.GetGamesDevelopedBy(gameStudio);
            Console.WriteLine("Jogos do estúdio de jogos " + gameStudio.Name + ":");
            foreach (var game in games)
            {
                Console.WriteLine(game.Name);
            }
        }
        catch (NotImplementedException exception)
        {
            Console.WriteLine(exception.Message);
            Console.WriteLine("Ainda não é possível realizar essa funcionalidade!");
            return;
        }
    }

    public void QueryGamesPlayedByPlayer()
    {
        var player = SelectPlayer(database.Players);
        if (player == null)
        {
            Console.WriteLine("Pessoa jogadora não encontrada!");
            return;
        }
        try {
            var games = database.GetGamesPlayedBy(player);
            if (games.Count() == 0)
            {
                Console.WriteLine("Pessoa jogadora não jogou nenhum jogo!");
                return;
            }
            Console.WriteLine("Jogos jogados pela pessoa jogadora " + player.Name + ":");
            foreach (var game in games)
            {
                Console.WriteLine(game.Name);
            }
        }
        catch (NotImplementedException exception)
        {
            Console.WriteLine(exception.Message);
            Console.WriteLine("Ainda não é possível realizar essa funcionalidade!");
            return;
        }

    }

    public void QueryGamesBoughtByPlayer()
    {
        var player = SelectPlayer(database.Players);
        if (player == null)
        {
            Console.WriteLine("Pessoa jogadora não encontrada!");
            return;
        }
        try {
            var games = database.GetGamesOwnedBy(player);
            Console.WriteLine("Jogos comprados pela pessoa jogadora " + player.Name + ":");
            foreach (var game in games)
            {
                Console.WriteLine(game.Name);
            }
        }
        catch (NotImplementedException exception)
        {
            Console.WriteLine(exception.Message);
            Console.WriteLine("Ainda não é possível realizar essa funcionalidade!");
            return;
        }
    }

    private bool IsValidBasicInput(string? input, string errorMessage)
    {
        if (input != null && input.Length > 0) return true;
        Console.WriteLine(errorMessage);
        return false;
    }

    // 1. Crie a funcionalidde para adicionar uma nova pessoa jogadora ao banco de dados
    public void AddPlayer()
    {
        Console.Write("Informe o nome da pessoa jogadora: ");
        var input = Console.ReadLine();
        var isValidInput = IsValidBasicInput(input, "\n❌ Nome inválido! Tente novamente.\n");
        if (!isValidInput) return;
        List<Player> players = database.Players;
        Player newPlayer = new();
        if (players.Count == 0)
        {
            newPlayer.Id = 1;
            newPlayer.Name = input;
        }
        else
        {
            var nextId = players[players.Count - 1].Id + 1;
            newPlayer.Id = nextId;
            newPlayer.Name = input;
        }
        database.Players.Add(newPlayer);
        Console.WriteLine($"\n✅ Pessoa jogadora \x1b[1m{newPlayer.Name}\x1b[0m adicionada com sucesso!\n");
    }

    // 2. Crie a funcionalidade de adicionar um novo estúdio de jogos ao banco de dados
    public void AddGameStudio()
    {
        Console.Write("Informe o nome do estúdio de jogos: ");
        var input = Console.ReadLine();
        var isValidInput = IsValidBasicInput(input, "\n❌ Nome inválido! Tente novamente.\n");
        if (!isValidInput) return;
        List<GameStudio> gameStudios = database.GameStudios;
        GameStudio gameStudio = new();
        if (gameStudios.Count == 0)
        {
            gameStudio.Id = 1;
            gameStudio.Name = input;
        }
        else
        {
            var nextId = gameStudios[gameStudios.Count - 1].Id + 1;
            gameStudio.Id = nextId;
            gameStudio.Name = input;
        }
        database.GameStudios.Add(gameStudio);
        Console.WriteLine($"\n✅ Estúdio de jogos \x1b[1m{gameStudio.Name}\x1b[0m adicionado com sucesso!\n");
    }

    private string? GetGameNameInput()
    {
        Console.Write("Informe o nome do jogo: ");
        var gameNameInput = Console.ReadLine();
        var isValidGameNameInput = IsValidBasicInput(
            gameNameInput,
            "\n❌ Nome inválido! Tente novamente.\n"
        );
        if (!isValidGameNameInput) return null;
        return gameNameInput;
    }

    private DateTime? GetGameReleaseDateInput()
    {
        Console.Write("Informe a data de lançamento: ");
        var releaseDateInput = Console.ReadLine();
        var isDateTime = DateTime.TryParseExact(
            releaseDateInput,
            "dd/MM/yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out DateTime releaseDate
        );
        if (isDateTime) return releaseDate;
        Console.WriteLine("\n❌ Data de lançamento inválida! Tente novamente.\n");
        return null;
    }

    private GameType? GetGameTypeInput()
    {
        Console.WriteLine("Informe o tipo do jogo:");
        GameType[] gameTypes = (GameType[])Enum.GetValues(typeof(GameType));
        foreach (var item in gameTypes) Console.WriteLine((int)item + " - " + item);
        Console.Write("Tipo de jogo: ");
        var gameTypeInput = Console.ReadLine();
        if (int.TryParse(gameTypeInput, out int gameTypeInt)) return (GameType)gameTypeInt;
        Console.WriteLine("\n❌ Tipo de jogo inválido! Tente novamente.\n");
        return null;
    }


    // 3. Crie a funcionalidade de adicionar novo Jogo ao Banco de dados
    public void AddGame()
    {
        var gameName = GetGameNameInput();
        if (gameName == null) return;
        var releaseDate = GetGameReleaseDateInput();
        if (releaseDate == null) return;
        var gameType = GetGameTypeInput();
        if (gameType == null) return;
        List<Game> games = database.Games;
        Game game = new();
        if (games.Count == 0)
            game.Id = 1;
        else
        {
            var nextId = games[games.Count - 1].Id + 1;
            game.Id = nextId;
        }
        game.Name = gameName;
        game.ReleaseDate = (DateTime)releaseDate;
        game.GameType = (GameType)gameType;
        games.Add(game);
        Console.WriteLine($"\n✅ Jogo \x1b[1m{gameName}\x1b[0m adicionado com sucesso!\n");
    }

    public void ChangeGameStudio(Game game)
    {
        var gameStudio = SelectGameStudio(database.GameStudios);
        if (gameStudio == null)
        {
            Console.WriteLine("Opção inválida! Tente novamente.");
            return;
        }
        game.DeveloperStudio = gameStudio.Id;
        Console.WriteLine("Estúdio de jogos alterado com sucesso!");
    }

    public void AddGameStudioToFavorites(Player player)
    {
        var notFavoriteStudios = database.GameStudios.Where(s => !player.FavoriteGameStudios.Contains(s.Id)).ToList();
        var gameStudio = SelectGameStudio(notFavoriteStudios);
        if (gameStudio == null)
        {
            Console.WriteLine("Nenhum estúdio de jogos encontrado!");
            return;
        }
        player.AddGameStudioToFavorites(gameStudio);
    }

    public void BuyGame(Player player)
    {
        var gamesNotOwned = database.Games.Where(g => !player.GamesOwned.Contains(g.Id)).ToList();
        var game = SelectGame(gamesNotOwned);
        if (game == null)
        {
            Console.WriteLine("Jogo não encontrado!");
            return;
        }
        player.BuyGame(game);
        Console.WriteLine("Jogo comprado com sucesso!");
    }

    public Player SelectPlayer(List<Player> players)
    {
        Console.WriteLine("Selecione o jogador:");
        PrintPlayers(players);
        var playerId = int.Parse(Console.ReadLine() ?? "0");
        Player? result = players.FirstOrDefault(p => p.Id == playerId);
        return result!;
    }

    public Game SelectGame(List<Game> games)
    {
        Console.WriteLine("Selecione o jogo:");
        PrintGames(games);
        var gameId = int.Parse(Console.ReadLine() ?? "0");
        return games.FirstOrDefault(g => g.Id == gameId)!;
    }

    public GameStudio SelectGameStudio(List<GameStudio> gameStudios)
    {
        Console.WriteLine("Selecione o estúdio de jogos:");
        PrintGameStudios(gameStudios);
        var gameStudioId = int.Parse(Console.ReadLine() ?? "0");
        return gameStudios.FirstOrDefault(gs => gs.Id == gameStudioId)!;
    }

    public void PrintGames(List<Game> games)
    {
        foreach (var game in games)
        {
            Console.WriteLine(game.Id + " - " + game.Name);
        }
    }

    public void PrintGameStudios(List<GameStudio> gameStudios)
    {
        foreach (var gameStudio in gameStudios)
        {
            Console.WriteLine(gameStudio.Id + " - " + gameStudio.Name);
        }
    }

    public void PrintPlayers(List<Player> players)
    {
        foreach (var player in players)
        {
            Console.WriteLine(player.Id + " - " + player.Name);
        }
    }

    public void PrintGameTypes()
    {
        Console.WriteLine("1 - Ação");
        Console.WriteLine("2 - Aventura");
        Console.WriteLine("3 - Puzzle");
        Console.WriteLine("4 - Estratégia");
        Console.WriteLine("5 - Simulação");
        Console.WriteLine("6 - Esportes");
        Console.WriteLine("7 - Outro");
    }
}