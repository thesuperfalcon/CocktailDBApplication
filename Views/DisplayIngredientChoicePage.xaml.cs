using CocktailDBApplication.Models;
using CocktailDBApplication.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace CocktailDBApplication.Views;

public partial class DisplayIngredientChoicePage : ContentPage
{
	public DisplayIngredientChoicePage(List<Ingredient> ingredients)
    {
        InitializeComponent();

        toggleSwitch.Toggled += async (sender, e) =>
        {
            // Handle toggle state change here
            // You can define the actions for ON and OFF states here
        };


        foreach (var ingredient in ingredients)
        {
            if (ingredient != null)
            {
                var ingredientName = ingredient.strIngredient;
                var imageButton = new ImageButton
                {
                    Source = $"https://www.thecocktaildb.com/images/ingredients/{ingredient.strIngredient}-Medium.png",
                    Aspect = Aspect.AspectFit,
                    HeightRequest = 200,
                    WidthRequest = 200,
                    Margin = new Thickness(5)
                };

                var label = new Label
                {
                    Text = ingredient.strIngredient,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Start
                };
                var stackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    Children = { imageButton, label },
                    Margin = new Thickness(5)
                };
                // Use FFImageLoading to enable asynchronous image loading and caching

                // Add a TapGestureRecognizer to the ImageButton
                imageButton.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async () =>
                    {
                        if (toggleSwitch.IsToggled)
                        {
                            var drinkList = await DrinkViewModel.GetDrinksAsync("filter.php?i=", ingredientName);
                            if (drinkList != null && drinkList.Any())
                            {
                                var specificDrinks = await DrinkViewModel.GetDrinksByIngredientAsync(drinkList);
                                await Navigation.PushAsync(new Views.DisplayDrinksChoicePage(specificDrinks));
                            }
                        }
                        else
                        {
                            await Navigation.PushAsync(new Views.DisplayIngredientPage(ingredient));
                        }
                    })
                });

                ingredientFlexLayout.Children.Add(stackLayout);
            }
        }
	}
}