using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MapOfDiet.Models;
using MapOfDiet.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace MapOfDiet.ViewModels
{
    public partial class AdminViewModel : ObservableObject
    {
        // ADD RECIPE
        [ObservableProperty] private string nameFood = string.Empty;
        [ObservableProperty] private int caloriesFood;
        [ObservableProperty] private double proteinsFood;
        [ObservableProperty] private double fatsFood;
        [ObservableProperty] private double carbohydratesFood;
        [ObservableProperty] private string descriptionFood;
        [ObservableProperty] private string cookingDescriptionFood;

        [ObservableProperty] private string searchFoodIngredient = string.Empty;
        [ObservableProperty] private string searchFoodCategory = string.Empty;

        [ObservableProperty] private double massFoodIngredient;

        [ObservableProperty] private byte[] imageFood;
        public ObservableCollection<Category> SearchResultsFoodCategories { get; } = new();
        public ObservableCollection<Ingredient> SearchResultsFoodIngredients { get; } = new();

        public ObservableCollection<Category> SelectedFoodCategories { get; } = new();
        public ObservableCollection<Ingredient> SelectedFoodIngredients { get; } = new();

        [RelayCommand]
        private void SearchFoodIngredients()
        {
            SearchResultsFoodIngredients.Clear();
            foreach (var ingr in DBWork.SearchIngredientsByName(SearchFoodIngredient))
                SearchResultsFoodIngredients.Add(ingr);
        }

        [RelayCommand]
        private void SearchFoodCategories()
        {
            SearchResultsFoodCategories.Clear();
            foreach (var cat in DBWork.SearchCategoriesByName(SearchFoodCategory))
                SearchResultsFoodCategories.Add(cat);
        }

        [RelayCommand]
        private void AddFoodIngredient(Ingredient ingr)
        {
            if (MassFoodIngredient > 0 && !SelectedFoodIngredients.Any(i => i.IngrId == ingr.IngrId))
            {
                ingr.Mass = MassFoodIngredient;
                SelectedFoodIngredients.Add(ingr);
            }
        }

        [RelayCommand]
        private void RemoveFoodIngredient(Ingredient ingr)
        {
            if (SelectedFoodIngredients.Contains(ingr))
                SelectedFoodIngredients.Remove(ingr);
        }

        [RelayCommand]
        private void AddFoodCategory(Category cat)
        {
            if (!SelectedFoodCategories.Any(c => c.CategoryId == cat.CategoryId))
                SelectedFoodCategories.Add(cat);
        }

        [RelayCommand]
        private void RemoveFoodCategory(Category cat)
        {
            if (SelectedFoodCategories.Contains(cat))
                SelectedFoodCategories.Remove(cat);
        }

        [RelayCommand]
        private void AddImageFood()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
            };

            if (dlg.ShowDialog() == true)
            {
                ImageFood = File.ReadAllBytes(dlg.FileName);
            }
        }

        [RelayCommand]
        private void SaveFood()
        {
            var recipe = new Food
            {
                Name = NameFood,
                Calories = CaloriesFood,
                Proteins = ProteinsFood,
                Fats = FatsFood,
                Carbohydrates = CarbohydratesFood,
                Categories = SelectedFoodCategories.ToList(),
                Ingredients = SelectedFoodIngredients.ToList(),
                Description = DescriptionFood,
                CookingDescription = CookingDescriptionFood,
                Image = ImageFood
            };

            DBWork.AddFood(recipe);
        }

        // Categories
        [ObservableProperty] string nameCategory;
        [ObservableProperty] string descriptionCategory;
        [ObservableProperty] byte[] imageCategory;

        // Ingredients
        [ObservableProperty] string nameIngredient;
        [ObservableProperty] string descriptionIngredient;
        [ObservableProperty] string measureNameIngredient;
        [ObservableProperty] byte[] imageIngredient;

        // Phys Activity
        [ObservableProperty] string nameActivity;
        [ObservableProperty] string descriptionActivity;
        [ObservableProperty] string measureNameActivity;
        [ObservableProperty] double measureToCaloriesActivity; // 100 действий сколько калорий
        [ObservableProperty] byte[] imageActivity;


        [RelayCommand]
        private void SaveCategory()
        {
            var category = new Category
            {
                Name = NameCategory,
                Description = DescriptionCategory,
                Image = ImageCategory
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
                MeasureName = MeasureNameIngredient,
                Image = ImageIngredient
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
                MeasureToCalories = MeasureToCaloriesActivity,
                Image = ImageActivity
            };
            DBWork.pushNewActivity(activity);
        }

        [RelayCommand]
        private void AddImageCategory()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
            };

            if (dlg.ShowDialog() == true)
            {
                ImageCategory = File.ReadAllBytes(dlg.FileName);
            }
        }

        [RelayCommand]
        private void AddImageIngredient()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
            };

            if (dlg.ShowDialog() == true)
            {
                ImageIngredient = File.ReadAllBytes(dlg.FileName);
            }
        }

        [RelayCommand]
        private void AddImageActivity()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
            };

            if (dlg.ShowDialog() == true)
            {
                ImageActivity = File.ReadAllBytes(dlg.FileName);
            }
        }
    }
}
