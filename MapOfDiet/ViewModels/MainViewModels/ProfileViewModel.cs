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
        [ObservableProperty] private bool isLoaded;

        public string Title { get; set; }
        public string IconPath { get; set; }

        [ObservableProperty] private string name;
        [ObservableProperty] private int age;
        [ObservableProperty] private int height;
        [ObservableProperty] private double nowWeight;
        [ObservableProperty] private double targetWeight;
        [ObservableProperty] private char gender;

        [ObservableProperty] private string searchCategory;
        public ObservableCollection<Category> SearchResultsCategories { get; } = new();
        public ObservableCollection<Category> LikeCategories { get; set; } = new();
        public ObservableCollection<Category> DislikeCategories { get; set; } = new();

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

        [RelayCommand]
        private void RemoveCategory(Category category)
        {
            if (category == null) return;
            if (LikeCategories.Contains(category))
                LikeCategories.Remove(category);
            else if (DislikeCategories.Contains(category))
                DislikeCategories.Remove(category);
        }

        [RelayCommand]
        private void SearchCategories()
        {
            SearchResultsCategories.Clear();
            foreach (var cat in DBWork.SearchCategoriesByName(SearchCategory))
                SearchResultsCategories.Add(cat);
        }

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

        [RelayCommand]
        private async Task GetProfile()
        {
            UserProfile? profile;
            Debug.WriteLine($"DEBUG: UserIdInUserSession = {UserSession.UserId}");
            profile = await DBWork.GetUserProfileAsync(UserSession.UserId);
            if (profile == null)
            {
                Debug.WriteLine($"DEBUG: UserProfile = null");
                return;
            }
            Debug.WriteLine($"DEBUG: UserIdInProfile = {profile.UserId}");
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

        public ProfileViewModel()
        {
            IsLoaded = false;
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await GetProfile();
            IsLoaded = true;
        }

    }
}
