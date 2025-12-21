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
        // Название блюда/рецепта
        [ObservableProperty] private string nameFood = string.Empty;

        // Калорийность блюда
        [ObservableProperty] private int caloriesFood;

        // Количество белков
        [ObservableProperty] private double proteinsFood;

        // Количество жиров
        [ObservableProperty] private double fatsFood;

        // Количество углеводов
        [ObservableProperty] private double carbohydratesFood;

        // Описание блюда
        [ObservableProperty] private string descriptionFood;

        // Описание процесса приготовления
        [ObservableProperty] private string cookingDescriptionFood;

        // Строка поиска ингредиентов
        [ObservableProperty] private string searchFoodIngredient = string.Empty;

        // Строка поиска категорий
        [ObservableProperty] private string searchFoodCategory = string.Empty;

        // Масса выбранного ингредиента
        [ObservableProperty] private double massFoodIngredient;

        // Изображение блюда
        [ObservableProperty] private byte[] imageFood;

        // Результаты поиска категорий
        public ObservableCollection<Category> SearchResultsFoodCategories { get; } = new();

        // Результаты поиска ингредиентов
        public ObservableCollection<Ingredient> SearchResultsFoodIngredients { get; } = new();

        // Выбранные категории для блюда
        public ObservableCollection<Category> SelectedFoodCategories { get; } = new();

        // Выбранные ингредиенты для блюда
        public ObservableCollection<Ingredient> SelectedFoodIngredients { get; } = new();

        // Поиск ингредиентов по имени
        [RelayCommand]
        private void SearchFoodIngredients()
        {
            SearchResultsFoodIngredients.Clear();
            foreach (var ingr in DBWork.SearchIngredientsByName(SearchFoodIngredient))
                SearchResultsFoodIngredients.Add(ingr);
        }

        // Поиск категорий по имени
        [RelayCommand]
        private void SearchFoodCategories()
        {
            SearchResultsFoodCategories.Clear();
            foreach (var cat in DBWork.SearchCategoriesByName(SearchFoodCategory))
                SearchResultsFoodCategories.Add(cat);
        }

        // Добавление ингредиента в список выбранных
        [RelayCommand]
        private void AddFoodIngredient(Ingredient ingr)
        {
            if (MassFoodIngredient > 0 && !SelectedFoodIngredients.Any(i => i.IngrId == ingr.IngrId))
            {
                ingr.Mass = MassFoodIngredient;
                SelectedFoodIngredients.Add(ingr);
            }
        }

        // Удаление ингредиента из списка выбранных
        [RelayCommand]
        private void RemoveFoodIngredient(Ingredient ingr)
        {
            if (SelectedFoodIngredients.Contains(ingr))
                SelectedFoodIngredients.Remove(ingr);
        }

        // Добавление категории в список выбранных
        [RelayCommand]
        private void AddFoodCategory(Category cat)
        {
            if (!SelectedFoodCategories.Any(c => c.CategoryId == cat.CategoryId))
                SelectedFoodCategories.Add(cat);
        }

        // Удаление категории из списка выбранных
        [RelayCommand]
        private void RemoveFoodCategory(Category cat)
        {
            if (SelectedFoodCategories.Contains(cat))
                SelectedFoodCategories.Remove(cat);
        }

        // Загрузка изображения блюда
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

        // Сохранение блюда в базу данных
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

        // Название категории
        [ObservableProperty] string nameCategory;

        // Описание категории
        [ObservableProperty] string descriptionCategory;

        // Изображение категории
        [ObservableProperty] byte[] imageCategory;

        // Название ингредиента
        [ObservableProperty] string nameIngredient;

        // Описание ингредиента
        [ObservableProperty] string descriptionIngredient;

        // Единица измерения ингредиента
        [ObservableProperty] string measureNameIngredient;

        // Изображение ингредиента
        [ObservableProperty] byte[] imageIngredient;

        // Название физической активности
        [ObservableProperty] string nameActivity;

        // Описание физической активности
        [ObservableProperty] string descriptionActivity;

        // Единица измерения активности
        [ObservableProperty] string measureNameActivity;

        // Количество калорий на 100 действий
        [ObservableProperty] double measureToCaloriesActivity;

        // Изображение активности
        [ObservableProperty] byte[] imageActivity;

        // Сохранение категории в базу данных
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

        // Сохранение ингредиента в базу данных
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

        // Сохранение физической активности в базу данных
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

        // Загрузка изображения категории
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

        // Загрузка изображения ингредиента
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

        // Загрузка изображения активности
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
