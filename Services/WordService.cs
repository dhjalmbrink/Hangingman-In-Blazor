using BlazorHaningman.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlazorHaningman.Services
{
    public class WordService
    {
        [HttpGet]
        public async Task<string> GetRandomWord()
        {
            var randomWord = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.api-ninjas.com/");
                client.DefaultRequestHeaders.Add("X-Api-Key", "FiIFh/QkAzYmwL750HOmVQ==27SlVGBzgnrJxSOX");
                var addUrl = "v1/randomword";

                var result = await client.GetAsync(addUrl);
                var resultContent = await result.Content.ReadAsStringAsync();
                var newRandomWord = JsonConvert.DeserializeObject<Word>(resultContent);
                randomWord = newRandomWord.word;
            }

            return randomWord;
        }
    }
}
