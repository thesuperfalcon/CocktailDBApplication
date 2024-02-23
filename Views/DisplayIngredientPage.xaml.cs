using CocktailDBApplication.Models;

namespace CocktailDBApplication.Views;

public partial class DisplayIngredientPage : ContentPage
{
	public DisplayIngredientPage(Ingredient ingredient)
	{
		InitializeComponent();
        BindingContext = ingredient;
	}
}