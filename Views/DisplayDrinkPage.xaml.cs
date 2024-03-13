using CocktailDBApplication.Helpers;
using CocktailDBApplication.Models;
using System.Globalization;

namespace CocktailDBApplication.Views
{
    public partial class DisplayDrinkPage : ContentPage
    {
        public DisplayDrinkPage(Drink specificDrink)
        {
            InitializeComponent();
            BindingContext = specificDrink;
            NonNullMeasurementsAndIngredients = GetNonNullMeasurementsAndIngredients(specificDrink);
            ShowIngredientAndMeasurement.ItemsSource = NonNullMeasurementsAndIngredients;

            var backButton = PageHelper.CreateBackButton(this);
            backButtonContainer.Children.Add(backButton);
        }

        public List<string> NonNullMeasurementsAndIngredients { get; set; }

        private List<string> GetNonNullMeasurementsAndIngredients(Drink drink)
        {
            List<string> result = new List<string>();
            List<string> nonDigitIngredients = new List<string>();

            for (int i = 1; i <= 15; i++)
            {
                var ingredientProperty = typeof(Drink).GetProperty($"strIngredient{i}");
                var measureProperty = typeof(Drink).GetProperty($"strMeasure{i}");

                var ingredient = (string)ingredientProperty.GetValue(drink);
                var measure = (string)measureProperty.GetValue(drink);

                if (!string.IsNullOrEmpty(ingredient) && measure != null)
                {
                    measure = ConvertMeasurementToMl(measure.ToLower());
                    result.Add($"{measure} {ingredient}");
                }
                else if (!string.IsNullOrEmpty(ingredient))
                {
                    if (ingredient.Any(char.IsDigit))
                    {
                        result.Add(ingredient);
                    }
                    nonDigitIngredients.Add(ingredient);
                }
            }


            result = result.OrderByDescending(x =>
            {
                double.TryParse(x.Split(' ')[0], out double measureValue);
                return measureValue;
            }).ToList();

            result.AddRange(nonDigitIngredients);

            return result;

        }

        private string ConvertMeasurementToMl(string measure)
        {
            if (string.IsNullOrEmpty(measure))
                return "";

            double ml = 0;

            if (measure.Contains("oz"))
            {
                string ozString = measure.Substring(0, measure.IndexOf("oz")).Trim();
                double oz;
                if (measure.Contains("-"))
                {
                    oz = CalculateAverage(measure);
                }
                else
                {
                    oz = ozString.Contains('/') ? ParseMixedFraction(ozString) : double.Parse(ozString, CultureInfo.InvariantCulture);
                }
                ml = Math.Round(oz * 30, 2);
            }
            else if (measure.Contains("cl"))
            {
                double cl = measure.Contains("-") ? CalculateAverage(measure) : double.Parse(measure.Replace(".", ",").Replace("cl", "").Trim());
                ml = cl * 10;
            }
            else if (measure.Contains("shot"))
            {
                ml = double.TryParse(measure.Split(' ')[0], out double numberOfShots) ? Math.Round(numberOfShots * 30, 2) : 30;
            }
            else if (measure.Contains("dash"))
            {
                int numberOfDashes = 1;
                foreach (char c in measure)
                {
                    if (char.IsDigit(c))
                    {
                        numberOfDashes = int.Parse(c.ToString());
                        break;
                    }
                }
                return $"{numberOfDashes} dash{(numberOfDashes > 1 ? "es" : "")}";
            }
            else if (measure.Contains("parts"))
            {
                return measure;
            }
            else
            {
                return measure;
            }

            return $"{ml} ml";
        }

        private double ParseMixedFraction(string value)
        {
            var parts = value.Split(' ');
            double wholeNumber = 0.0;
            double numerator = 0.0;
            double denominator = 1.0;

            foreach (var part in parts)
            {
                if (part.Contains('/'))
                {
                    string[] fractionParts = part.Split('/');
                    if (fractionParts.Length == 2 && double.TryParse(fractionParts[0], out double num) && double.TryParse(fractionParts[1], out double denom))
                    {
                        numerator = num;
                        denominator = denom;
                    }
                    else
                    {
                        throw new FormatException("Invalid mixed fraction format.");
                    }
                }
                else
                {
                    if (double.TryParse(part, out double num))
                    {
                        wholeNumber = num;
                    }
                    else
                    {
                        throw new FormatException("Invalid mixed fraction format.");
                    }
                }
            }
            return wholeNumber + (numerator / denominator);
        }

        private double CalculateAverage(string measure)
        {
            string[] parts = measure.Split('-');
            double sum = 0;
            int count = 0;

            foreach (string part in parts)
            {
                string[] splitPart = part.Trim().Split(' ');
                if (double.TryParse(splitPart[0], out double value))
                {
                    sum += value;
                    count++;
                }
            }

            if (count > 0)
            {
                double average = sum / count;
                return average;
            }
            else
            {
                throw new FormatException("Invalid format");
            }
        }


        private async void OnClickedMainPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }
    }
}