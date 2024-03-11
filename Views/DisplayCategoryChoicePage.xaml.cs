using CocktailDBApplication.Models;
using CocktailDBApplication.ViewModels;

namespace CocktailDBApplication.Views;

public partial class DisplayCategoryChoicePage : ContentPage
{
    public DisplayCategoryChoicePage()
    {
        InitializeComponent();
        InitializeFlexGridLayout();
    }
    public async Task InitializeFlexGridLayout()
    {
        var task = InitializeFiltersAsync();
        var list = await task;

        if (list != null)
        {
            var stackLayout = new StackLayout(); 

            foreach (var filter in list)
            {
                var categoryName = filter.strCategory;
                var alcoholic = filter.strAlcoholic;
                var button = new Button
                {
                    Text = !string.IsNullOrEmpty(alcoholic) ? alcoholic : categoryName,
                    Margin = 5
                };

                string apiParam = !string.IsNullOrEmpty(alcoholic) ? "a" : "c";

                button.Clicked += (sender, e) => ButtonClicked(sender, e, !string.IsNullOrEmpty(alcoholic) ? alcoholic : categoryName, apiParam);

                stackLayout.Children.Add(button); 
            }

            Content = stackLayout; 
        }
    }


    private async void ButtonClicked(object sender, EventArgs e, string value, string apiParam)
    {
        var button = (Button)sender;

        var drinks = await DrinkViewModel.GetDrinksAsync($"filter.php?{apiParam}=", value);
        var specificList = new List<Drink>();

        if (drinks != null)
        {
            var specificDrinks = await DrinkViewModel.GetDrinksByIngredientAsync(drinks);
            if (specificDrinks != null)
            {
                specificList.AddRange(specificDrinks);
            }
        }
        await Navigation.PushAsync(new Views.DisplayDrinksChoicePage(specificList));
    }

    private async Task<List<Drink>> InitializeFiltersAsync()
    {
        var filterList = new List<Drink>();

        var categoryListTask = GetList("list.php?c=", "list");
        var alcoholicListTask = GetList("list.php?a=", "list");

        var categoryList = await categoryListTask;
        var alcoholicList = await alcoholicListTask;

        if (categoryList != null && alcoholicList != null)
        {
            filterList.AddRange(categoryList);
            filterList.AddRange(alcoholicList);
        }

        return filterList;
    }

    public async Task<List<Drink>> GetList(string param, string value)
    {
        var filterDrinks = await DrinkViewModel.GetDrinksAsync(param, value);

        return filterDrinks;
    }
}