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

            switch (measure)
            {
                case string _ when measure.Contains("oz"):
                    string ozString = measure.Substring(0, measure.IndexOf("oz")).Trim();
                    double oz;
                    if (ozString.Contains('/'))
                    {
                        oz = ParseMixedFraction(ozString);
                    }
                    else
                    {
                        oz = double.Parse(ozString, CultureInfo.InvariantCulture);
                    }

                    double ml = Math.Round(oz * 30, 2);
                    return $"{ml} ml";

                case string _ when measure.Contains("cl"):
                    if (measure.Contains("-"))
                    {
                        string[] parts = measure.Replace("cl", "").Split('-');
                        if (parts.Length == 2 && double.TryParse(parts[0].Trim(), out double lower) && double.TryParse(parts[1].Trim(), out double upper))
                        {
                            double total = lower + upper;
                            double average = total / 2;
                            double cl = average * 10;
                            return $"{cl} ml";
                        }
                        else
                        {
                            return "Invalid format";
                        }
                    }
                    else
                    {
                        double cl = double.Parse(measure.Replace(".", ",").Replace("cl", "").Trim());
                        ml = cl * 10;
                        return $"{ml} ml";
                    }

                case string _ when measure.Contains("shot"):
                    if (double.TryParse(measure.Split(' ')[0], out double numberOfShots))
                    {
                        double totalMl = Math.Round(numberOfShots * 30, 2);
                        return $"{totalMl} ml";
                    }
                    else
                    {
                        return "30 ml";
                    }

                case string _ when measure.Contains("parts"):
                    return measure;

                case string _ when measure.Contains("dash"):
                    char[] chars = measure.ToCharArray();
                    foreach (char c in chars)
                    {
                        if (char.IsDigit(c))
                        {
                            int numberOfDashes = int.Parse(c.ToString());
                            return $"{numberOfDashes} dash{(numberOfDashes > 1 ? "es" : "")}";
                        }
                    }
                    return "1 dash";

                default:
                    return measure;
            }
        }


        private double ParseMixedFraction(string value)
        {
            if (value.Contains('/'))
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

            return double.Parse(value);
        }

        private async void OnClickedMainPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }
    }
}