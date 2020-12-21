using Faker;
using FizzWare.NBuilder;
using MiniBite.Api.Inventory.Entities;
using System;
using System.Collections.Generic;

namespace MiniBite.Api.Inventory.DataAccess
{
    public class InventoryDbInitializer
    {
        public static void Seed(InventoryDbContext context)
        {
            context.Proudcts.AddRange(CreateProductsOfSize(16));
            context.SaveChanges();
        }

        static int _sequence = 1;
        static int NextSequence()
        {
            return _sequence++;
        }

        private static IList<Product> CreateProductsOfSize(int size)
        {
            var items = Builder<Product>.CreateListOfSize(size)
                .All()
                .With(x => x.Id = Guid.NewGuid())
                .Build();

            items.ForEach(i =>
            {
                var food = Pick<string>.RandomItemFrom(FoodProducts.Food);
                i.Code = $"{food}-{NextSequence()}";
                i.CategoryName = "[CategoryName is not cared by purchasing or sales]";
                i.Description = $"{Pick<string>.RandomItemFrom(FoodProducts.Sizes)} " +
                    $"{Pick<string>.RandomItemFrom(FoodProducts.Quality)} " +
                    $"{food} - {Company.BS()}";
                i.Price = Pick<decimal>.RandomItemFrom(FoodProducts.Prices);
            });

            return items;
        }
    }

    class FoodProducts
    {
        public static List<string> Colors = new List<string> { "Red", "Yellow", "Green" };

        public static List<string> Sizes = new List<string> { "Large", "Medium", "Small" };

        public static List<string> Quality = new List<string> { "Elite", "Premium", "Premier" };

        public static List<string> Food = new List<string> { "Apple", "Watermelon", "Orange", "Tomato", "Steak", "Chicken" };

        public static List<decimal> Prices = new List<decimal> { 1.99M, 2.99M, 3.99M, 4.99M, 5.99M, 6.99M, 7.99M };

        public static List<string> Units = new List<string> { "eaches", "plates", "box" };
    }
}
