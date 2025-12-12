using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MapOfDiet.Models;
using MapOfDiet.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MapOfDiet.ViewModels
{
    public partial class AdminViewModel : ObservableObject
    {
        // Categories
        [ObservableProperty] string nameCategory;
        [ObservableProperty] string descriptionCategory;

        // Ingredients
        [ObservableProperty] string nameIngredient;
        [ObservableProperty] string descriptionIngredient;
        [ObservableProperty] string measureNameIngredient;

        // Phys Activity
        [ObservableProperty] string nameActivity;
        [ObservableProperty] string descriptionActivity;
        [ObservableProperty] string measureNameActivity;
        [ObservableProperty] double measureToCaloriesActivity; // 100 действий сколько калорий

        [RelayCommand]
        private void SaveCategory()
        {
            var category = new Category
            {
                Name = NameCategory,
                Description = DescriptionCategory
            };
            DBWork.pushNewCategory(category);
        }

        [RelayCommand]
        private void SaveIngredient()
        {
            var ingredient = new Ingredient
            {
                Name = NameIngredient,
                Description = DescriptionIngredient,
                MeasureName = MeasureNameIngredient
            };
            DBWork.pushNewIngredient(ingredient);
        }

        [RelayCommand]
        private void SaveActivity()
        {
            var activity = new MyActivity
            {
                Name = NameActivity,
                Description = DescriptionActivity,
                MeasureName = MeasureNameActivity,
                MeasureToCalories = MeasureToCaloriesActivity
            };
            DBWork.pushNewActivity(activity);
        }


        // ADD RECIPE
        [ObservableProperty] private string name = string.Empty;
        [ObservableProperty] private int calories;
        [ObservableProperty] private double proteins;
        [ObservableProperty] private double fats;
        [ObservableProperty] private double carbohydrates;
        [ObservableProperty] private string description;
        [ObservableProperty] private string cookingDescription;

        [ObservableProperty] private string searchIngredient = string.Empty;
        [ObservableProperty] private string searchCategory = string.Empty;

        [ObservableProperty] private double massIngredient;
        public ObservableCollection<Category> SearchResultsCategories { get; } = new();
        public ObservableCollection<Ingredient> SearchResultsIngredients { get; } = new();

        public ObservableCollection<Category> SelectedCategories { get; } = new();
        public ObservableCollection<Ingredient> SelectedIngredients { get; } = new();

        [RelayCommand]
        private void SearchIngredients()
        {
            SearchResultsIngredients.Clear();
            foreach (var ingr in DBWork.SearchIngredientsByName(SearchIngredient))
                SearchResultsIngredients.Add(ingr);
        }

        [RelayCommand]
        private void SearchCategories()
        {
            SearchResultsCategories.Clear();
            foreach (var cat in DBWork.SearchCategoriesByName(SearchCategory))
                SearchResultsCategories.Add(cat);
        }

        [RelayCommand]
        private void AddIngredient(Ingredient ingr)
        {
            if (MassIngredient > 0 && !SelectedIngredients.Any(i => i.IngrId == ingr.IngrId))
            {
                ingr.Mass = MassIngredient;
                SelectedIngredients.Add(ingr);
            }
        }

        [RelayCommand]
        private void RemoveIngredient(Ingredient ingr)
        {
            if (SelectedIngredients.Contains(ingr))
                SelectedIngredients.Remove(ingr);
        }

        [RelayCommand]
        private void AddCategory(Category cat)
        {
            if (!SelectedCategories.Any(c => c.CategoryId == cat.CategoryId))
                SelectedCategories.Add(cat);
        }

        [RelayCommand]
        private void RemoveCategory(Category cat)
        {
            if (SelectedCategories.Contains(cat))
                SelectedCategories.Remove(cat);
        }

        [RelayCommand]
        private void SaveRecipe()
        {
            var recipe = new Food
            {
                Name = Name,
                Calories = Calories,
                Proteins = Proteins,
                Fats = Fats,
                Carbohydrates = Carbohydrates,
                Categories = SelectedCategories.ToList(),
                Ingredients = SelectedIngredients.ToList(),
                Description = Description,
                CookingDescription = CookingDescription
            };

            DBWork.AddFood(recipe);
        }
    }
}
