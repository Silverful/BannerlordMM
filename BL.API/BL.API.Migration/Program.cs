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
//https://docs.google.com/spreadsheets/d/1fOsewSdJJiY3gOEaVhusc2CbF8Pmr9nQPuML_pm0RYo/edit - copyold

namespace BL.API.Migration
{
    class Program
    {
        private static RestClient _httpClient;
        private static readonly string _spreadsheetId = "1Z3GBcvfprEquNRHRa2ceiA0U1qBSslrFyBLbqbjV8hU";
        private static readonly string _oldSpreadsheetId = "1pHS_4OCTB5deOWygl9YOV2KLtZsJJVkT7SdkatIbXRk";
        private static readonly string _testSpreadsheetId = "1fOsewSdJJiY3gOEaVhusc2CbF8Pmr9nQPuML_pm0RYo";
        private static SheetsService _service;

        static async Task Main(string[] args)
        {
            try
            {
            //_httpClient = new RestClient("https://bannerlordmm.com/api");
            _httpClient = new RestClient("http://localhost:5000/api/eu");

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

            await LoadPlayers(_spreadsheetId);
            Console.WriteLine("Players Finished");
            await LoadMatches(_testSpreadsheetId, "Screens!A2:M"); //old
            Console.WriteLine("Old screens Finished");
            //await LoadMatches(_spreadsheetId, "Screens!A2:M"); //new
            Console.WriteLine("Finished");
            Console.Read();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
            }
        }

        private static async Task LoadPlayers(string spreadsheetId)
        {
            var discordId = 1;
            string range = "Stats!A2:W";

            var values = GetSpreadsheetResponse(spreadsheetId, range);

            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    try
                    {
                        var cmd = new AddPlayerCommand
                        {
                            Nickname = row[1].ToString(),
                            Country = row[2].ToString(),
                            IGL = string.IsNullOrEmpty(row[3].ToString()) ? false : true,
                            MainClass = row[5].ToString(),
                            SecondaryClass = row[6].ToString(),
                            DiscordId = discordId
                        };
                        await Upload("Players", cmd);

                        discordId++;
                    }
                    catch(Exception ex)
                    {

                    }
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
                    try
                    {
                        if (match.ScreenshotLink == null)
                        {
                            CreateNewMatch(match, row);
                        }

                        if (match.ScreenshotLink == row[11].ToString() + "2")
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
                    catch (Exception ex)
                    {

                    }
                }
                await Upload("Matches", match);
            }
        }

        private static void CreateNewMatch(UploadMatchCommand match, IList<object> row)
        {
            match.ScreenshotLink = row[11].ToString() + "2";
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
            var playerId = players.Where(p => p.Nickname.ToLower() == row[1].ToString().ToLower()).FirstOrDefault()?.Id;

            return new UploadMatchCommand.MatchRecord
                {
                    PlayerId = playerId,
                    RoundsWon = GetRoundsWon(row),
                    Kills = Convert.ToSByte(row[5]),
                    Assists = Convert.ToSByte(row[6]),
                    Score = Convert.ToInt32(row[7]),
                    MVPs = Convert.ToByte(row[8]),
                    Faction = row[9].ToString(),
                };
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
            var isSuccessful = false;
            while (!isSuccessful)
            {
                try
                {

                    var request = new RestRequest(path, DataFormat.Json)
                        .AddJsonBody(cmd);

                    var response = await _httpClient.PostAsync<string>(request);

                    //if (!Guid.TryParse(response, out Guid res))
                    //{

                    //}

                    var json = System.Text.Json.JsonSerializer.Serialize(cmd);
                    isSuccessful = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static async Task<T> GetData<T>(string path)
        {
            var request = new RestRequest(path, DataFormat.Json);

           return await _httpClient.GetAsync<T>(request);
        }
    }
}
