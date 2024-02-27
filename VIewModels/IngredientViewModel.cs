using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CocktailDBApplication.Models;
using Newtonsoft.Json;

namespace CocktailDBApplication.VIewModels
{
    internal class IngredientViewModel
    {

        public static async Task<List<Ingredient>> GetIngredientsAsync(string param, string value)
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

                        var ingredientInfo = JsonConvert.DeserializeObject<IngredientListResponse>(json);

                        return ingredientInfo?.Ingredients;
                    }
                    else
                    {
                        Console.WriteLine($"Failed to retrieve ingredients: {response.StatusCode}");
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
        public static async Task<Ingredient> GetSingularIngredientAsync(string value)
        {
            var apiUrl = $"https://www.thecocktaildb.com/api/json/v1/1/search.php?i={value}";

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<IngredientListResponse>(json);

                        if (result != null && result.Ingredients != null && result.Ingredients.Count > 0)
                        {
                            return result.Ingredients[0];
                        }
                        else
                        {
                            Console.WriteLine("No ingredient found.");
                            return null;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Failed to retrieve ingredient: {response.StatusCode}");
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
    }
}
