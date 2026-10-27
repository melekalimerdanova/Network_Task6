using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Network_Task6.Services
{
    public class WeatherResponseModel
    {
        [JsonProperty("location")]
        public CityLocation LocationInfo { get; set; }

        [JsonProperty("current")]
        public WeatherDetails CurrentDetails { get; set; }

        public override string ToString()
        {
            return $@"
================Hava proqnozu================
Məkan: {LocationInfo?.Name}, {LocationInfo?.Country}
Istilik: {CurrentDetails?.TemperatureC} °C
Hiss olunan: {CurrentDetails?.ApparentTempC} °C
Vəziyyət: {CurrentDetails?.SkyCondition?.Description}
Rütubət səviyyəsi: {CurrentDetails?.HumidityRate}%
Külək sürəti: {CurrentDetails?.WindSpeedKph} km/s
==============================================
";
        }
    }

    public class CityLocation
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
    }

    public class WeatherDetails
    {
        [JsonProperty("temp_c")]
        public double TemperatureC { get; set; }
        [JsonProperty("feelslike_c")]
        public double ApparentTempC { get; set; }
        [JsonProperty("humidity")]
        public int HumidityRate { get; set; }
        [JsonProperty("wind_kph")]
        public double WindSpeedKph { get; set; }
        [JsonProperty("condition")]
        public SkyCondition SkyCondition { get; set; }
    }

    public class SkyCondition
    {
        [JsonProperty("text")]
        public string? Description { get; set; }
    }

    public class ClimateManager
    {
        private readonly string tokenKey = "e50dd5c835bb435bbfb205010262610";
        private readonly string apiEndpoint = "https://api.weatherapi.com/v1/current.json";
        private readonly HttpClient client = new HttpClient();

        public async Task<WeatherResponseModel?> FetchClimateDataAsync(string locationName)
        {
            try
            {
                string requestUri = $"{apiEndpoint}?key={tokenKey}&q={Uri.EscapeDataString(locationName)}";
                HttpResponseMessage responseMessage = await client.GetAsync(requestUri);

                if (!responseMessage.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Sorğu uğursuz oldu. Status: {(int)responseMessage.StatusCode}");
                    return null;
                }

                string payloadJson = await responseMessage.Content.ReadAsStringAsync();
                WeatherResponseModel parsedData = JsonConvert.DeserializeObject<WeatherResponseModel>(payloadJson);

                return parsedData;
            }
            catch (HttpRequestException netEx)
            {
                Console.WriteLine($"Şəbəkə xətası: {netEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gözlənilməz xəta: {ex.Message}");
                return null;
            }
        }
    }
}
