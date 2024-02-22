using CocktailDBApplication.Models;
using CocktailDBApplication.ViewModels;
using System;
using System.Collections.Generic;

namespace CocktailDBApplication
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false); // Hide the navigation bar
        }


        private async void OnButtonPressedShowRecepies(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(searchBar.Text))
            {
                string input = searchBar.Text;
                List<Drink> drinks = await DrinkViewModel.GetDrinkAsync("search.php?s=", searchBar.Text);
                DisplayDrink(drinks);
            }
        }


        private async void DisplayDrink(List<Drink> drinks)
        {
            drinkListView.ItemsSource = drinks;
        }

        private async void DrinkSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var selectedDrink = e.SelectedItem as Drink;
            await Navigation.PushAsync(new Views.DisplayDrinkPage(selectedDrink));
            ((ListView)sender).SelectedItem = null;
        }
    }
}
