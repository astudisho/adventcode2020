using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventCode.Exercises
{
    public static class Day21
    {
        public static string Data { get; set; }

        public static IList<Food> ParseFoods()
        {
            if (Data is null) Data = InputFetcher.FetchInput("Day21.txt");
            var foodString = Data.Split("\r\n");

            var foods = foodString.Select(x =>
            {
                return new Food
                {
                    Ingredients = GetIngredients(x),
                    Allergenes = GetAllergen(x)
                };
            }).ToList();

            return foods;
        }

        public static string[] GetAllergen(string food)
        {
            var regex = new Regex(@"\([a-z, ]+\)");
            var allergenMatch = regex.Match(food);
            var allergenes = allergenMatch.Value.Trim('(', ')')
                                        .Replace("contains ", "")
                                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                        .Select(x => x.Trim())
                                        .ToArray();

            return allergenes;
        }

        public static string[] GetIngredients(string food)
        {
            var regex = new Regex(@"[a-z ]+");
            var ingredientMatch = regex.Match(food);
            var ingredients = ingredientMatch.Value.Trim()
                                        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                        .Select(x => x.Trim())
                                        .ToArray();

            return ingredients;
        }

        public static HashSet<string> GetNonAllergenIngredients(IList<Food> foods)
        {

            var allergenes = foods.SelectMany(x => x.Allergenes,
                                              (food, allergen) =>
                                              {
                                                  return allergen;
                                              }).ToHashSet();

            var ingredients = foods.SelectMany(x => x.Ingredients).ToHashSet();
            var nonAllergenIngredients = new HashSet<string>();

            foreach (var ingredient in ingredients)
            {
                var possibleAllergens = foods.Where(f => f.Ingredients.Contains(ingredient)).SelectMany(x => x.Allergenes);
                var isAllergenFree = possibleAllergens.All(x => foods.Any(y => !y.Ingredients.Contains(ingredient) && y.Allergenes.Contains(x)));
                if (isAllergenFree)
                {
                    nonAllergenIngredients.Add(ingredient);
                }
            }

            return nonAllergenIngredients;
        }

        public static int Exercise1()
        {
            var foods = ParseFoods();

            var nonAllergenIngredients = GetNonAllergenIngredients(foods);

            var aux = nonAllergenIngredients.Select(ingredient => foods.SelectMany(f => f.Ingredients).Count(i => ingredient == i)).Sum();

            return aux;
        }

        public static string Exercise2()
        {
            var foods = ParseFoods();

            var ingredients = foods.SelectMany(x => x.Ingredients).ToHashSet();
            var nonAllergenIngredients = GetNonAllergenIngredients(foods);
            var allAllergenes = foods.SelectMany(x => x.Allergenes).ToHashSet().ToList();
            allAllergenes.Sort();
            ingredients.ExceptWith(nonAllergenIngredients);

            List<(string, string)> result = new List<(string, string)>();
            var pendingAllergenes = allAllergenes;

            while (pendingAllergenes.Any())
            {
                
                foreach (var allergen in pendingAllergenes)
                {
                    var allergenFoods = foods.Where( f => f.Allergenes.Contains(allergen.ToString()));
                    var allergenIngredients = ingredients.Where(i => allergenFoods.All(af => af.Ingredients.Contains(i)));

                    if(allergenIngredients.Count() == 1)
                    {
                        var ingredient = allergenIngredients.First();
                        result.Add((ingredient, allergen));
                        pendingAllergenes.Remove(allergen);
                        ingredients.Remove(ingredient);
                        break;
                    }
                }
            }
            var aux = result.OrderBy(x => x.Item2).Select(x => x.Item1).Aggregate((prev, next) => $"{prev},{next}");
            

            return aux;
        }

        public class Food
        {
            public string[] Ingredients { get; set; }
            public string[] Allergenes { get; set; }
        }
    }
}
