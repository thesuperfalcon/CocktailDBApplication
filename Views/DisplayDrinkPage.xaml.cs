using CocktailDBApplication.Models;

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

            for (int i = 1; i <= 15; i++)
            {
                var ingredientProperty = typeof(Drink).GetProperty($"strIngredient{i}");
                var measureProperty = typeof(Drink).GetProperty($"strMeasure{i}");

                var ingredient = (string)ingredientProperty.GetValue(drink);
                var measure = (string)measureProperty.GetValue(drink);

                if (!string.IsNullOrEmpty(ingredient))
                {
                    measure = ConvertMeasurementToMl(measure);
                    result.Add($"{measure} {ingredient}");
                }
            }

            return result;
        }

        private string ConvertMeasurementToMl(string measure)
        {
            if (string.IsNullOrEmpty(measure))
                return "";

            if (measure.Contains("oz"))
            {
                double oz = ParseMixedFraction(measure.Replace("oz", "").Trim());
                double ml = Math.Round(oz * 30, 2); // 1 oz = 30 ml
                return $"{ml} ml";
            }
            else if (measure.Contains("cl"))
            {
                double cl = ParseMixedFraction(measure.Replace("cl", "").Trim());
                double ml = cl * 10; // 1 cl = 10 ml
                return $"{ml} ml";
            }
            else if (measure.Contains("shot"))
            {
                if (double.TryParse(measure.Split(' ')[0], out double numberOfShots))
                {
                    double totalMl = Math.Round(numberOfShots * 30, 2); // Assuming 1 shot = 30 ml
                    return $"{totalMl} ml";
                }
                else
                {
                    return "30 ml"; // Default to 30 ml if the number of shots cannot be determined
                }
            }
            else if (measure.Contains("parts"))
            {
                return measure; // Keep it as parts
            }
            else
            {
                return measure; // Return unchanged if not recognized
            }
        }

        private double ParseMixedFraction(string value)
        {
            if (value.Contains('/'))
            {
                var parts = value.Split(' ');
                double result = 0.0;

                foreach (var part in parts)
                {
                    if (part.Contains('/'))
                    {
                        string[] fractionParts = part.Split('/');
                        if (fractionParts.Length == 2 && double.TryParse(fractionParts[0], out double numerator) && double.TryParse(fractionParts[1], out double denominator))
                        {
                            result += numerator / denominator;
                        }
                        else
                        {
                            throw new FormatException("Invalid mixed fraction format.");
                        }
                    }
                    else
                    {
                        if (double.TryParse(part, out double wholeNumber))
                        {
                            result += wholeNumber;
                        }
                        else
                        {
                            throw new FormatException("Invalid mixed fraction format.");
                        }
                    }
                }

                return result;
            }

            return double.Parse(value); 
        }
    }
}