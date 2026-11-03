using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TerraJourn.Services
{
    public class GeminiService
    {
        public async Task<string> GenerateRouteAsync(string userPreferencesPrompt)
        {
            try
            {
                // Web.config'den API key'i okuyoruz
                string apiKey = ConfigurationManager.AppSettings["GeminiApiKey"];

                // Gemini modeli
                string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3.5-flash-lite:generateContent?key={apiKey}";

                using (HttpClient client = new HttpClient())
                {
                    // JSON'ı tamamen düz metin (string) olarak hazırlıyoruz - Hiçbir tip çakışması yaşanmaz!
                    // Metin içindeki tırnakların bozulmaması için prompt'u escape ediyoruz veya doğrudan string interpolation kullanıyoruz.
                    string escapedPrompt = userPreferencesPrompt.Replace("\"", "\\\"");

                    string jsonPayload = "{ \"contents\": [ { \"parts\": [ { \"text\": \"" + escapedPrompt + "\" } ] } ] }";

                    HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        JObject json = JObject.Parse(jsonResponse);

                        // Gelen yanıttan yapay zekanın yazdığı metni çekiyoruz
                        string aiText = json["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();
                        return aiText ?? "Yapay zekadan bir yanıt alınamadı.";
                    }
                    else
                    {
                        string errorDetails = await response.Content.ReadAsStringAsync();
                        return $"API bağlantı hatası ({response.StatusCode}): {errorDetails}";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Beklenmeyen bir hata oluştu: " + ex.Message;
            }
        }
    }
}