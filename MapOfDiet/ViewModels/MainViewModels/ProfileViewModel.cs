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

namespace MapOfDiet.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        // Пока база данных не загружена интерфейс не отрисовывается чтобы не было резкого появления в ячейках значений
        [ObservableProperty] private bool isLoaded;

        // Вспомогательные переменные для хранения информации о иконке в TabControl
        public string Title { get; set; }
        public string IconPath { get; set; }

        // Имя
        [ObservableProperty] private string name;
        // Возраст
        [ObservableProperty] private int age;
        // Рост
        [ObservableProperty] private int height;
        // Текущий вес
        [ObservableProperty] private double nowWeight;
        // Желаемый вес
        [ObservableProperty] private double targetWeight;
        // Пол
        [ObservableProperty] private char gender;

        // Переменная отвечающая за поле ввода имени категории
        [ObservableProperty] private string searchCategory;

        // Список категорий найденных в результате поиска
        public ObservableCollection<Category> SearchResultsCategories { get; } = new();
        // Любимые категории
        public ObservableCollection<Category> LikeCategories { get; set; } = new();
        // Нелюбимые категории
        public ObservableCollection<Category> DislikeCategories { get; set; } = new();

        // Конструктор
        public ProfileViewModel()
        {
            IsLoaded = false;
            InitializeAsync();
        }

        // Вспомогательная функция для констуктора
        private async void InitializeAsync()
        {
            await GetProfile();
            IsLoaded = true;
        }

        // При изменении поля NowWeight вызывается и записывает сразу же новые данные
        partial void OnNowWeightChanged(double value)
        {
            if (isLoaded == true)
            {
                var record = new WeightRecord
                {
                    Date = DateOnly.FromDateTime(DateTime.Today),
                    Weight = value,
                    TargetWeight = TargetWeight
                };
                DBWork.pushNewWeightRecord(record);
            }
        }

        // При изменении поля TargetWeight вызывается и записывает сразу же новые данные
        partial void OnTargetWeightChanged(double value)
        {
            if (isLoaded == true)
            {
                var record = new WeightRecord
                {
                    Date = DateOnly.FromDateTime(DateTime.Today),
                    Weight = NowWeight,
                    TargetWeight = value
                };
                DBWork.pushNewWeightRecord(record);
            }
        }

        // Добавляет категорию
        [RelayCommand]
        private void AddCategory(Category category)
        {
            if (category == null) return;
            if (category.IsEnabled)
            {
                if (!LikeCategories.Contains(category))
                {
                    LikeCategories.Add(category);
                }
            }
            else if (!DislikeCategories.Contains(category))
            {
                DislikeCategories.Add(category);
            }
        }

        // Удаляет категорию
        [RelayCommand]
        private void RemoveCategory(Category category)
        {
            if (category == null) return;
            if (LikeCategories.Contains(category))
                LikeCategories.Remove(category);
            else if (DislikeCategories.Contains(category))
                DislikeCategories.Remove(category);
        }

        // Ищет категории
        [RelayCommand]
        private void SearchCategories()
        {
            SearchResultsCategories.Clear();
            foreach (var cat in DBWork.SearchCategoriesByName(SearchCategory))
                SearchResultsCategories.Add(cat);
        }

        // Сохраняет профиль
        [RelayCommand]
        private async Task SaveProfile()
        {
            var profile = new UserProfile
            {
                UserId = UserSession.UserId,
                Name = Name,
                Age = Age,
                Height = Height,
                Gender = Gender,
                NowWeight = NowWeight,
                TargetWeight = TargetWeight,
                LikeCategories = LikeCategories.ToList(),
                DislikeCategories = DislikeCategories.ToList()
            };
            await DBWork.PushUserProfileAsync(profile);
        }

        // Получает из бд профиль, который пользователь заполнял ранее
        [RelayCommand]
        private async Task GetProfile()
        {
            UserProfile? profile;
            profile = await DBWork.GetUserProfileAsync(UserSession.UserId);
            if (profile == null)
            {
                return;
            }
            Name = profile.Name;
            Age = profile.Age;
            Height = profile.Height;
            Gender = profile.Gender;
            NowWeight = profile.NowWeight;
            TargetWeight = profile.TargetWeight;
            foreach (var cat in profile.LikeCategories) {
                LikeCategories.Add(cat);
            }
            foreach (var cat in profile.DislikeCategories) {
                DislikeCategories.Add(cat);
            }
        }
    }
}
