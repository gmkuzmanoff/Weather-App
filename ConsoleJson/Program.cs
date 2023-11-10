using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;


namespace ConsoleJson
{
    class Program
    {
        
        public void GetWeather()
        {
            string city_name;
            string apiKey = Key.API_KEY;
            string wind_dir;

            do
            {
                Console.Write("Enter Location (City) : ");
                city_name = Convert.ToString(Console.ReadLine());
            }
            while (city_name.Length == 0);
            if (city_name == "exit")
            {
                Console.WriteLine("Are you sure? y/n");
                string yes = Console.ReadLine();
                if (yes == "y") Environment.Exit(0);
            }
            //Uri uri = new Uri("http://api.openweathermap.org/data/2.5/weather?lat=" + lat + "&lon=" + lon + "&appid=" + apiKey);
            //Uri uri = new Uri("http://api.openweathermap.org/data/2.5/weather?zip=" + zip +",Bg" + "&appid=" + apiKey);
            Uri uri = new Uri($"http://api.openweathermap.org/data/2.5/weather?q={city_name}&appid={apiKey}");
            WebRequest webRequest = WebRequest.Create(uri);

            try
            {
                webRequest.GetResponse();
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message + "\n");
                return;
            }


            WebResponse response = webRequest.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream());
            string responseData = streamReader.ReadToEnd();

            var outObject = JsonConvert.DeserializeObject<WeatherData.RootObject>(responseData);
            string sunrise, sunset;
            GetSunriseAndSunset(outObject, out sunrise, out sunset);
            wind_dir = GetWindDirection(outObject);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n(Latitude) : {outObject.coord.lat}");
            Console.WriteLine($"(Longitute) : {outObject.coord.lon}");
            Console.WriteLine($"(Location) : {outObject.name},{outObject.sys.country}");
            Console.WriteLine($"(Sunrise) : {sunrise}");
            Console.WriteLine($"(Sunset) : {sunset}");
            Console.WriteLine($"(Temperature) : {(int)(outObject.main.temp - 273.15)}°C");
            Console.WriteLine($"(Wind speed) : {(int)(outObject.wind.speed)} m/s , {wind_dir} ({outObject.wind.deg} deg)");
            Console.WriteLine($"(Pressure) : {outObject.main.pressure} hpa");
            Console.WriteLine($"(Humidity) : {outObject.main.humidity}%");
            Console.WriteLine($"(Cloudiness) : {outObject.weather[0].description}\n");
            Console.ForegroundColor = ConsoleColor.Cyan;

        }

        private static string GetWindDirection(WeatherData.RootObject outObject)
        {
            string wind_dir;
            if (outObject.wind.deg > 348.75 || outObject.wind.deg < 11.25) wind_dir = "North";
            else if (outObject.wind.deg > 11.25 && outObject.wind.deg < 33.75) wind_dir = "North-Northeast";
            else if (outObject.wind.deg > 33.75 && outObject.wind.deg < 56.25) wind_dir = "Northeast";
            else if (outObject.wind.deg > 56.25 && outObject.wind.deg < 78.75) wind_dir = "East-Northeast";
            else if (outObject.wind.deg > 78.75 && outObject.wind.deg < 101.25) wind_dir = "East";
            else if (outObject.wind.deg > 101.25 && outObject.wind.deg < 123.75) wind_dir = "East-Southeast";
            else if (outObject.wind.deg > 123.75 && outObject.wind.deg < 146.25) wind_dir = "Southeast";
            else if (outObject.wind.deg > 146.25 && outObject.wind.deg < 168.75) wind_dir = "South-Southeast";
            else if (outObject.wind.deg > 168.75 && outObject.wind.deg < 191.25) wind_dir = "South";
            else if (outObject.wind.deg > 191.25 && outObject.wind.deg < 213.75) wind_dir = "South-Southwest";
            else if (outObject.wind.deg > 213.75 && outObject.wind.deg < 236.25) wind_dir = "Southwest";
            else if (outObject.wind.deg > 236.25 && outObject.wind.deg < 258.75) wind_dir = "West-Southwest";
            else if (outObject.wind.deg > 258.75 && outObject.wind.deg < 281.25) wind_dir = "West";
            else if (outObject.wind.deg > 281.25 && outObject.wind.deg < 303.75) wind_dir = "West-Northwest";
            else if (outObject.wind.deg > 303.75 && outObject.wind.deg < 326.25) wind_dir = "Northwest";
            else if (outObject.wind.deg > 326.25 && outObject.wind.deg < 348.75) wind_dir = "North-Northwest";
            else wind_dir = "";
            return wind_dir;
        }

        private static void GetSunriseAndSunset(WeatherData.RootObject outObject, out string sunrise, out string sunset)
        {
            sunrise = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(outObject.sys.sunrise).ToLocalTime().ToShortTimeString();
            sunset = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(outObject.sys.sunset).ToLocalTime().ToShortTimeString();
        }

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Weather. Powered by Openweathermap.org\n");
            Console.Write("Copyright (C) GMKuzmanoff. All rights reserved.");
            Console.Write("\n_______________________________________________\n\n");
            Program program = new Program();
            while (true)
                program.GetWeather();
        }
    }
}         
            
            
        
            
          
            
    


