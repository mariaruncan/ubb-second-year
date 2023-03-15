using System;
using System.Collections.Generic;
using Model;

namespace Networking
{
    public interface Response { }

    [Serializable]
    public class OkResponse : Response { }

    [Serializable]
    public class ErrorResponse : Response
    {
        public string Message { get; }
        public ErrorResponse(string message)
        {
            Message = message;
        }
    }

    [Serializable]
    public class GetGamesResponse : Response
    {
        public List<Game> games { get; }
        public GetGamesResponse(List<Game> games)
        {
            this.games = games;
        }
    }

    [Serializable]
    public class GameResponse : Response
    {
        public Game game { get; }
        public GameResponse(Game game)
        {
            this.game = game;
        }
    }
}
