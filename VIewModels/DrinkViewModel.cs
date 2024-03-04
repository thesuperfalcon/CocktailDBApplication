using CocktailDBApplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CocktailDBApplication.ViewModels
{
    internal class DrinkViewModel
    {
        public static async Task<List<Drink>> GetDrinksAsync(string param, string value)
        {
            var apiUrl = $"https://www.thecocktaildb.com/api/json/v1/1/{param}{value}";

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();

                        var cocktailInfo = JsonConvert.DeserializeObject<DrinkListResponse>(json);

                        return cocktailInfo?.Drinks;
                    }
                    else
                    {
                        Console.WriteLine($"Failed to retrieve drinks: {response.StatusCode}");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return null;
                }
            }
        }
        public static async Task<List<Drink>> GetDrinksByIngredientAsync(List<Drink> drinks)
        {
            List<Drink> foundedDrinks = new List<Drink>();

            try
            {
                foreach (var drink in drinks)
                {
                    var foundedDrink = await GetDrinksAsync("lookup.php?i=", drink.idDrink);
                    foundedDrinks.Add(foundedDrink.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return foundedDrinks;
        }

    }
}
