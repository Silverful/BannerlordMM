using BL.API.Core.Domain.Player;
using BL.API.Services.Matches.Commands;
using BL.API.Services.Players.Commands;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

//https://docs.google.com/spreadsheets/d/1Z3GBcvfprEquNRHRa2ceiA0U1qBSslrFyBLbqbjV8hU/edit#gid=1879936255 - current
//https://docs.google.com/spreadsheets/d/1pHS_4OCTB5deOWygl9YOV2KLtZsJJVkT7SdkatIbXRk/edit#gid=405046885 - old

namespace BL.API.Migration
{
    class Program
    {
        private static RestClient _httpClient;
        private static readonly string _spreadsheetId = "1Z3GBcvfprEquNRHRa2ceiA0U1qBSslrFyBLbqbjV8hU";
        private static readonly string _oldSpreadsheetId = "1pHS_4OCTB5deOWygl9YOV2KLtZsJJVkT7SdkatIbXRk";
        private static SheetsService _service;

        static async Task Main(string[] args)
        {
            //_httpClient = new RestClient("https://bannerlordmm.com/");
            _httpClient = new RestClient("https://localhost:44365/");

            string[] Scopes = { SheetsService.Scope.Spreadsheets };
            string ApplicationName = "MM Migration Script";

            GoogleCredential credential;
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                     .CreateScoped(Scopes);
            };

            _service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            await LoadPlayers();
            Console.WriteLine("Players Finished");
            await LoadMatches(_oldSpreadsheetId, "Лист2!A2:M"); //old
            Console.WriteLine("Old screens Finished");
            await LoadMatches(_spreadsheetId, "Screens!A2:M"); //new
            Console.WriteLine("Finished");
            Console.Read();
        }

        private static async Task LoadPlayers()
        {
            string range = "Stats!A2:V";

            var values = GetSpreadsheetResponse(_spreadsheetId, range);

            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    var cmd = new AddPlayerCommand
                    {
                        Nickname = row[1].ToString(),
                        Country = row[2].ToString(),
                        IGL = string.IsNullOrEmpty(row[3].ToString()) ? false : true,
                        Clan = row[4].ToString(),
                        MainClass = row[5].ToString(),
                        SecondaryClass = row[6].ToString(),
                        DiscordId = Convert.ToInt64(row[21])
                    };
                    await Upload("Players", cmd);
                }
            }
        }

        private static async Task LoadMatches(string sheetId, string range)
        {
            var players = (await GetData<IEnumerable<Player>>("Players")).ToList();

            var values = GetSpreadsheetResponse(sheetId, range);

            if (values != null && values.Count > 0)
            {
                var match = new UploadMatchCommand();
                foreach (var row in values)
                {
                    if (match.ScreenshotLink == null)
                    {
                        CreateNewMatch(match, row);
                    }

                    if (match.ScreenshotLink == row[11].ToString())
                    {
                        AddNewRecord(match, row, players);
                    } 
                    else
                    {
                        await Upload("Matches", match);
                        match = new UploadMatchCommand();
                        CreateNewMatch(match, row);
                        AddNewRecord(match, row, players);
                    }
                }
            }
        }

        private static void CreateNewMatch(UploadMatchCommand match, IList<object> row)
        {
            match.ScreenshotLink = row[11].ToString();
            match.MatchDate = Convert.ToDateTime(row[0]);
            match.RoundsPlayed = Convert.ToByte(row[4]);
            match.Team1Records = new List<UploadMatchCommand.MatchRecord>();
            match.Team2Records = new List<UploadMatchCommand.MatchRecord>();
        }

        private static void AddNewRecord(UploadMatchCommand match, IList<object> row, IEnumerable<Player> players)
        {
            var record = ParseMatchRecord(row, players);

            if (record != null)
            {
                if (match.Team1Records.Count() == 0 || match.Team1Records.First().RoundsWon == GetRoundsWon(row))
                {
                    ((IList<UploadMatchCommand.MatchRecord>)match.Team1Records).Add(record);
                }
                else
                {
                    ((IList<UploadMatchCommand.MatchRecord>)match.Team2Records).Add(record);
                }
            }
        }

        private static UploadMatchCommand.MatchRecord ParseMatchRecord(IList<object> row, IEnumerable<Player> players)
        {
            var playerId = players.Where(p => p.Nickname == row[1].ToString()).FirstOrDefault()?.Id;

            return playerId.HasValue ?
                new UploadMatchCommand.MatchRecord
                {
                    PlayerId = players.Where(p => p.Nickname == row[1].ToString()).First().Id,
                    RoundsWon = GetRoundsWon(row),
                    Kills = Convert.ToSByte(row[5]),
                    Assists = Convert.ToSByte(row[6]),
                    Score = Convert.ToInt32(row[7]),
                    MVPs = Convert.ToByte(row[8]),
                    Faction = row[9].ToString(),
                }
                :
                null;
        }

        private static byte GetRoundsWon(IList<object> row)
        {
            return (byte)(row[3].ToString() == "1" ? 3 : Convert.ToByte(row[4]) - 3);
        }

        private static IList<IList<Object>> GetSpreadsheetResponse(string id, string range)
        {
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    _service.Spreadsheets.Values.Get(id, range);

            ValueRange response = request.Execute();
            return response.Values;
        }

        private static async Task Upload(string path, object cmd)
        {
            try
            {

                var request = new RestRequest(path, DataFormat.Json)
                    .AddJsonBody(cmd);

                await _httpClient.PostAsync<string>(request);
            }
            catch(Exception ex)
            {

            }
        }

        private static async Task<T> GetData<T>(string path)
        {
            var request = new RestRequest(path, DataFormat.Json);

           return await _httpClient.GetAsync<T>(request);
        }
    }
}
