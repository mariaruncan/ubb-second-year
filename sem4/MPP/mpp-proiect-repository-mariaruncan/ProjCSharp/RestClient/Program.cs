using ConsoleTables;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RestClient
{
    class Program
    {
        static HttpClient Client = new HttpClient();
        static string Url = "http://localhost:8080/app/games";

        public static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // get by id
            Console.WriteLine("Get by id...");
            Game aux = await GetByIdAsync(Url + "/1");
            Console.WriteLine(aux);

            // create
            Console.WriteLine("Create...");
            Game newGame = new Game
            {
                team1 = aux.team1,
                team2 = aux.team2,
                description = "testRestClientC#",
                date = aux.date.AddDays(100),
                totalNoOfTickets = 20,
                soldTickets = 0,
                pricePerTicket = 10
            };
            aux = await AddGameAsync(Url, newGame);
            Console.WriteLine(aux);

            // find all
            Console.WriteLine("Find all...");
            Game[] games = await GetAllAsync(Url);
            PrintAsTable(games);

            // update
            Console.WriteLine("Update...");
            Game updateGame = games.Last();
            updateGame.description = "testRestC#";
            string response = await UpdateGameAsync(Url + "/" + updateGame.id, updateGame);
            Console.WriteLine("Update response: " + response);


            //find all
            Console.WriteLine("Find all...");
            games = await GetAllAsync(Url);
            PrintAsTable(games);

            // delete
            Console.WriteLine("Delete...");
            response = await DeleteGameAsync(Url + "/" + updateGame.id.ToString());
            Console.WriteLine("Delete response: " + response);

            // find all
            Console.WriteLine("Find all...");
            games = await GetAllAsync(Url);
            PrintAsTable(games);

        }

        static async Task<Game> GetByIdAsync(string path)
        {
            Game product = null;
            HttpResponseMessage response = await Client.GetAsync(path);
            if(response.IsSuccessStatusCode)
                product = await response.Content.ReadAsAsync<Game>();
            return product;
        }

        static async Task<Game> AddGameAsync(string path, Game game)
        {
            Game product = null;
            HttpResponseMessage response = await Client.PostAsJsonAsync(path, game);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<Game>();
            }
            return product;
        }

        static async Task<Game[]> GetAllAsync(string path)
        {
            Game[] product = null;
            HttpResponseMessage response = await Client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<Game[]>();
            }
            return product;
        }

        static async Task<string> UpdateGameAsync(string path, Game game)
        {
            HttpResponseMessage response = await Client.PutAsJsonAsync(path, game);
            if (!response.IsSuccessStatusCode)
            {
                return "can not update!";
            }
            return "ok";
        }

        static async Task<string> DeleteGameAsync(string path)
        {
            HttpResponseMessage response = await Client.DeleteAsync(path);
            if (!response.IsSuccessStatusCode)
            {
                return "can not delete!";
            }
            return "ok";
        }

        private static void PrintAsTable(Game[] games)
        {
            var table = new ConsoleTable("Id", "Team1", "Team2", "Description", "Total tickets", "Sold tickets", "Date");
            foreach (Game g in games)
                table.AddRow(g.id, g.team1.id, g.team2.id, g.description, g.totalNoOfTickets, g.soldTickets, g.date.ToString().Split(' ')[0]);
            table.Write();
        }
    }

    public class Team
    {
        public long id { get; set; }
        public string name { get; set; }
    }

    public class Game
    {
        public long id { get; set; }
        public Team team1 { get; set; }
        public Team team2 { get; set; }
        public DateTime date { get; set; }
        public string description { get; set; }
        public int totalNoOfTickets { get; set; }
        public int soldTickets { get; set; }
        public float pricePerTicket { get; set; }

        public override string ToString()
        {
            return "Game: " + id + " " + date + " " + team1.name + " vs. " + team2.name +
                "\n" + description + "\nTickets available: " + (totalNoOfTickets - soldTickets) +
                " for only " + pricePerTicket + "$ per ticket";
        }
    }
}
