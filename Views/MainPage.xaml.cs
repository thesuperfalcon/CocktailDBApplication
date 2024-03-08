using CocktailDBApplication.Models;
using CocktailDBApplication.ViewModels;

namespace CocktailDBApplication
{
    public partial class MainPage : ContentPage
    {
        //Har varken Singleton eller Facade i detta projekt (Skriv förklaring också)
        public MainPage()
        {
            InitializeComponent();
            GenerateAlphabetButtons();
        }

        private void GenerateAlphabetButtons()
        {
            for (char c = 'A'; c <= 'Z'; c++)
            {
                var button = new Button
                {
                    Text = c.ToString(),
                    BackgroundColor = Color.FromRgb(173, 216, 230) // RGB values for LightBlue
            };
                button.Clicked += AlphabetButtonClicked;
                flexLayout.Children.Add(button);
            }
        }

        private async void ShowRecepiesMatch(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(searchBar.Text))
            {
                List<Drink> drinks = await DrinkViewModel.GetDrinksAsync("search.php?s=", searchBar.Text);

                if (drinks != null && drinks.Any())
                {
                    await Navigation.PushAsync(new Views.DisplayDrinksChoicePage(drinks));
                }
            }
        }

        private async void AlphabetButtonClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;

            List<Drink> drinks = await DrinkViewModel.GetDrinksAsync("search.php?f=", button.Text);

            await Navigation.PushAsync(new Views.DisplayDrinksChoicePage(drinks));
        }

        private async void OnClickedShowIngredients(object sender, EventArgs e)
        {
            List<Drink> drinkIngredients = await DrinkViewModel.GetDrinksAsync("list.php?i=", "list");
            List<Ingredient> ingredients = new List<Ingredient>();
            if(drinkIngredients != null && drinkIngredients.Any())
            {
                foreach(var drinkIngredient in drinkIngredients)
                {
                    var realIngredient = await IngredientViewModel.GetSingularIngredientAsync(drinkIngredient.strIngredient1);
                    ingredients.Add(realIngredient);
                }
            }
            await Navigation.PushAsync(new Views.DisplayIngredientChoicePage(ingredients));
        }

        private async void OnClickedShowCategories(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.DisplayFilterPage());
        }
    }
}
