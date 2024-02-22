using CocktailDBApplication.Models;
using CocktailDBApplication.ViewModels;
using System;
using System.Collections.Generic;

namespace CocktailDBApplication
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await SetImageSources();
        }

        private async Task SetImageSources()
        {
               

        }

        private async void OnButtonPressedShowRecepies(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(searchBar.Text))
            {
                string input = searchBar.Text;
                List<Drink> drinks = await DrinkViewModel.GetDrinksAsync("search.php?s=", searchBar.Text);
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

        private void Button1_Clicked(object sender, EventArgs e)
        {

        }
        private void Button2_Clicked(object sender, EventArgs e)
        {

        }
        private void Button3_Clicked(object sender, EventArgs e)
        {

        }
    }
}
