using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailDBApplication.Helpers
{
    public class PageHelper
    {
        public static Button CreateBackButton(Page page)
        {
            var backButton = new Button
            {
                Text = "Back",
                WidthRequest = 150,
                HeightRequest = 50,
                Margin = new Thickness(0, 25, 0, 0)
            };
            backButton.Clicked += async (sender, e) => await page.Navigation.PopAsync();
            return backButton;
        }
    }
}
