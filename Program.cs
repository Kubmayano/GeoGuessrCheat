using System.Net;
using Newtonsoft.Json;

namespace GeoGuessrCheat
{
    class hasMain
    {
        public static async Task RequestJsonData(string token) //Makes a request to geoguessr and downloads the JSON data into "JsonData.json"
        {
            string url = "https://www.geoguessr.com/api/v3/games/" + token;
            var client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(url);
            string data = await response.Content.ReadAsStringAsync();

            File.WriteAllText("JsonData.json", FormatJson(data));
        }

        public static string FormatJson(string data) //Formats data to be more readable
        {
            dynamic? parsedJson = JsonConvert.DeserializeObject(data);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }

        public static string[] GetLocation()
        {
            string[] location = new string[2]; //We only need coordinates + country code so size is 2
            string data = File.ReadAllText("JsonData.json");

            dynamic? JsonData = JsonConvert.DeserializeObject(data);

            if (JsonData != null)
            {
                int currentRound = (Convert.ToInt32(JsonData["round"]) -1);
                location[0] = "Coordinates: " + JsonData["rounds"][currentRound]["lat"] + " " + JsonData["rounds"][currentRound]["lng"];
                location[1] = "Country code: " + JsonData["rounds"][currentRound]["streakLocationCode"];
            }

            return location;
        }

        public static async Task Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Blue;

            while(true)
            {
                await RequestJsonData("uG9mJWXmetDC7Oet");
                foreach(var obj in GetLocation())
                {
                    System.Console.WriteLine(obj);
                }

                Console.ReadKey();
            }
        }
    }
}