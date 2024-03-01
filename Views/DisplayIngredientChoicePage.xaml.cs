using CocktailDBApplication.Models;
using CocktailDBApplication.ViewModels;
using CocktailDBApplication.VIewModels;
using System.Collections.Generic;

namespace CocktailDBApplication.Views;

public partial class DisplayIngredientChoicePage : ContentPage
{
	public DisplayIngredientChoicePage(List<Ingredient> ingredients)
    {
        InitializeComponent();

        foreach(var ingredient in ingredients)
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
                imageButton.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async () =>
                    {
                        var drinkList = await DrinkViewModel.GetDrinksAsync("filter.php?i=", ingredientName);
                        if (drinkList != null && drinkList.Any())
                        {
                            var specificDrinks = await DrinkViewModel.GetDrinksByIngredientAsync(drinkList);
                            await Navigation.PushAsync(new Views.DisplayDrinksChoicePage(specificDrinks));
                        }
                    })
                });

                ingredientFlexLayout.Children.Add(stackLayout);
            }
        }
	}
}