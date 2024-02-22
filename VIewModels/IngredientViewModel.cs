using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CocktailDBApplication.Models;

namespace CocktailDBApplication.VIewModels
{
    internal class IngredientViewModel
    {
        public static async Task<Ingredient>GetIngredientAsync(string uri, string input)
        {
            Ingredient ingredient = null;

            var client = new HttpClient();

            string baseAdress = "https://www.thecocktaildb.com/api/json/v1/1/";

            client.BaseAddress = new Uri(baseAdress);

            HttpResponseMessage response = await client.GetAsync(uri);

            if(response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                ingredient = JsonSerializer.Deserialize<Ingredient>(responseString);
            }
            return ingredient;
        }
    }
}
