using CocktailDBApplication.Models;
using CocktailDBApplication.ViewModels;
using CocktailDBApplication.VIewModels;

namespace CocktailDBApplication
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnButtonPressedShowRecepies(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(searchBar.Text))
            {
                string input = searchBar.Text;

                List<Drink> drinks = await DrinkViewModel.GetDrinksAsync("search.php?s=", searchBar.Text);
                List<Ingredient> ingredients = await IngredientViewModel.GetIngredientsAsync("search.php?i=", searchBar.Text);

                List<DisplayItem> combinedResults = new List<DisplayItem>();

                if (drinks != null && drinks.Any())
                {
                    combinedResults.Add(new DisplayItem { Item = null, Name = "Drink:" });
                    foreach (var drink in drinks)
                    {
                        combinedResults.Add(new DisplayItem { Item = drink, Name = drink.strDrink });
                    }
                }

                if (ingredients != null && ingredients.Any())
                {
                    combinedResults.Add(new DisplayItem { Item = null, Name = "Ingredient:" });
                    foreach (var ingredient in ingredients)
                    {
                        combinedResults.Add(new DisplayItem { Item = ingredient, Name = ingredient.strIngredient });
                    }
                }

                listView.ItemsSource = combinedResults;
            }
        }

        private async void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var selectedItem = e.SelectedItem;

            if (selectedItem is DisplayItem displayItem)
            {
                if (displayItem.Item is Drink)
                {
                    var selectedDrink = displayItem.Item as Drink;
                    await Navigation.PushAsync(new Views.DisplayDrinkPage(selectedDrink));
                }
                else if (displayItem.Item is Ingredient)
                {
                    var selectedIngredient = displayItem.Item as Ingredient;
                    await Navigation.PushAsync(new Views.DisplayIngredientPage(selectedIngredient));
                }
            }

    ((ListView)sender).SelectedItem = null;
        }
    }
}
