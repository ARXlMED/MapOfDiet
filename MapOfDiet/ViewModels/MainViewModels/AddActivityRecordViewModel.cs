using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MapOfDiet.Models;
using MapOfDiet.Services;
using System;
using System.Collections.ObjectModel;

namespace MapOfDiet.ViewModels
{
    public partial class AddActivityRecordViewModel : ObservableObject
    {
        // Вспомогательные переменные для хранения информации о иконке в TabControl
        public string Title { get; set; }
        public string IconPath { get; set; }
        
        // Переменная отвечающая за поле ввода имени активности
        [ObservableProperty] private string searchNameActivity;

        // Список активностей найденный в результате поиска
        public ObservableCollection<MyActivity> SearchResultsActivity { get; } = new();

        // Дата активности
        [ObservableProperty] private DateTime dateActivity = DateTime.Now.Date;
        // Время активности
        [ObservableProperty] private DateTime timeActivity = DateTime.Now;

        // Ищет активности по заданному имени и добавляет их в список активностей
        [RelayCommand]
        private void SearchActivity()
        {
            SearchResultsActivity.Clear();
            foreach (var activity in DBWork.SearchActivitiesByName(SearchNameActivity))
                SearchResultsActivity.Add(activity);
        }

        // Добавляет запись о активности данного пользователя 
        [RelayCommand]
        private void AddActivity(MyActivity activity)
        {
            if (activity == null) return;

            DateTime activityDateTime = new DateTime(DateActivity.Year, DateActivity.Month, DateActivity.Day, TimeActivity.Hour, TimeActivity.Minute, TimeActivity.Second);

            var activityRecord = new MyActivityRecord
            {
                Activity = activity,
                DateTime = activityDateTime,
                Amount = activity.EnteredAmount,
                Description = activity.Name
            };

            DBWork.PushActivityRecord(activityRecord);
            activity.EnteredAmount = 0;
        }
    }
}
