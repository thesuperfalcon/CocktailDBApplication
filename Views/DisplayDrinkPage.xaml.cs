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
                    else
                    {
                        nonDigitIngredients.Add(ingredient);
                    }
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
                    double oz = ParseMixedFraction(measure.Substring(0, measure.IndexOf("oz")).Trim());
                    double ml = Math.Round(oz * 30, 2); // 1 oz = 30 ml
                    return $"{ml} ml";

                case string _ when measure.Contains("cl"):
                    if (measure.Contains("-"))
                    {
                        string[] parts = measure.Replace("cl", "").Split('-');
                        if (parts.Length == 2 && double.TryParse(parts[0].Trim(), out double lower) && double.TryParse(parts[1].Trim(), out double upper))
                        {
                            double total = lower + upper; // Add lower and upper bounds together
                            double average = total / 2; // Calculate the average of the range
                            double cl = average * 10; // Convert cl to ml
                            return $"{cl} ml";
                        }
                        else
                        {
                            return "Invalid format"; // Handle invalid range format
                        }
                    }
                    else
                    {
                        double cl = double.Parse(measure.Replace(".", ",").Replace("cl", "").Trim());
                        ml = cl * 10; // 1 cl = 10 ml
                        return $"{ml} ml";
                    }

                case string _ when measure.Contains("shot"):
                    if (double.TryParse(measure.Split(' ')[0], out double numberOfShots))
                    {
                        double totalMl = Math.Round(numberOfShots * 30, 2); // Assuming 1 shot = 30 ml
                        return $"{totalMl} ml";
                    }
                    else
                    {
                        return "30 ml"; // Default to 30 ml if the number of shots cannot be determined
                    }

                case string _ when measure.Contains("parts"):
                    return measure; // Keep it as parts

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
                    return "1 dash"; // Treat "dash" as 1 if no digit is found

                default:
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