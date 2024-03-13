using CocktailDBApplication.Helpers;
using CocktailDBApplication.Models;

namespace CocktailDBApplication.Views
{
    public partial class DisplayDrinksChoicePage : ContentPage
    {
        public DisplayDrinksChoicePage(List<Drink> drinks)
        {
            InitializeComponent();

            foreach (var drink in drinks)
            {
                var imageButton = new ImageButton
                {
                    Source = drink.strDrinkThumb,
                    Aspect = Aspect.AspectFit,
                    HeightRequest = 200,
                    WidthRequest = 200,
                    Margin = new Thickness(5)
                };

                var label = new Label
                {
                    Text = drink.strDrink,
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
                        await Navigation.PushAsync(new DisplayDrinkPage(drink));
                    })
                });

                drinksFlexLayout.Children.Add(stackLayout);
            }

            var backButton = PageHelper.CreateBackButton(this);
            backButton.VerticalOptions = LayoutOptions.End;
            drinksFlexLayout.Children.Add(backButton);
        }
    }
}
