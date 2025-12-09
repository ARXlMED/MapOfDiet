using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MapOfDiet.Models;
using MapOfDiet.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
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
        public ObservableCollection<Category> LikeCategories { get; } = new();
        public ObservableCollection<Category> DislikeCategories { get; } = new();

        partial void OnNowWeightChanged(double value)
        {
            var record = new WeightRecord
            {
                Date = DateOnly.FromDateTime(DateTime.Today),
                Weight = value,
                TargetWeight = TargetWeight
            };
            DBWork.pushNewWeightRecord(record);
        }

        partial void OnTargetWeightChanged(double value)
        {
            var record = new WeightRecord
            {
                Date = DateOnly.FromDateTime(DateTime.Today),
                Weight = NowWeight,
                TargetWeight = value
            };
            DBWork.pushNewWeightRecord(record);
        }

        [RelayCommand]
        private void AddCategory(Category category)
        {
            if (category == null) return;
            if (!LikeCategories.Contains(category) && !DislikeCategories.Contains(category))
                LikeCategories.Add(category);
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
        private void SaveProfile()
        {
            var profile = new UserProfile
            {
                Name = Name,
                Age = Age,
                Height = Height,
                Gender = Gender,
                NowWeight = NowWeight,
                TargetWeight = TargetWeight,
                LikeCategories = LikeCategories.ToList(),
                DislikeCategories = DislikeCategories.ToList()
            };
        }
    }
}
