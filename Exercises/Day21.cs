using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventCode.Exercises
{
    public static class Day21
    {
        public static string Data => InputFetcher.FetchInput("Day21.txt");

        public static IList<Food> ParseFoods()
        {
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
            var allergenes = allergenMatch.Value.Trim('(',')')
                                        .Replace("contains ", "")
                                        .Split(",")
                                        .Select(x => x.Trim())
                                        .ToArray();

            return allergenes;
        }

        public static string[] GetIngredients(string food)
        {
            var regex = new Regex(@"[a-z ]+");
            var ingredientMatch = regex.Match(food);
            var ingredients = ingredientMatch.Value.Split(" ")
                                        .Select(x => x.Trim())
                                        .ToArray();

            return ingredients;
        }

        public static int Exercise1()
        {
            var result = 0;
            var foods = ParseFoods();

            var allergenes = foods.SelectMany(x => x.Allergenes,
                                              (food, allergen) => 
                                              {
                                                  return allergen;
                                              }).ToHashSet();

            // Ingredients.
            var nonSuspiciousIngredients = new List<string>();
            var suspiciousIngredients = new List<string>();
            foreach (var allergen in allergenes)
            {
                var allergenFoods = foods.Where(x => x.Allergenes.Contains(allergen)).ToList();
                var allergenIngredients = allergenFoods.SelectMany(x => x.Ingredients, (food, ingredient) => ingredient ).ToList();

                var suspiciousIngredients2 = allergenIngredients.Where(x => allergenFoods.All(y => y.Ingredients.Contains(x))).ToList();

                suspiciousIngredients.AddRange(allergenIngredients);

                nonSuspiciousIngredients.AddRange(allergenIngredients);
            }
            
            return result;
        }

        public class Food
        {
            public string[] Ingredients { get; set; }
            public string[] Allergenes { get; set; }
        }
    }
}
