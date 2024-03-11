using CocktailDBApplication.Models;
using CocktailDBApplication.ViewModels;

namespace CocktailDBApplication
{
    public partial class MainPage : ContentPage
    {
        /*Har varken Singleton eller Facade i detta projekt då det inte finns någon funktion med att ha dessa design-möster
         Finns inget syfte med Singleton för att dels så finns det ingen inloggningsfunktion, vilket innebär att en användare behöver inte föras över till olika sidor.
        Dels så är det att det ända som skickas över till olika sidor är antingen objekten Drink, Ingredient eller en lista på dem.
        Facade finns det inget syfte då jag inte har någon metod med kedjefunktion som används flera gånger. Enda som skulle kunna göras till en Facade om den används
        mer än en gång är GetDrinksByIngredientAsync på DrinkViewModel. Den skulle kunna göras om till en Facade om den används flera gånger, men i detta fall används den bara en gång
        i applikationen.
         */
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

            if (drinkIngredients != null && drinkIngredients.Any())
            {
                var tasks = drinkIngredients.Select(async drinkIngredient =>
                {
                    var realIngredient = await IngredientViewModel.GetSingularIngredientAsync(drinkIngredient.strIngredient1);
                    ingredients.Add(realIngredient);
                });

                await Task.WhenAll(tasks);
            }

            await Navigation.PushAsync(new Views.DisplayIngredientChoicePage(ingredients));
        }


        private async void OnClickedShowCategories(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.DisplayCategoryChoicePage());
        }
    }
}
