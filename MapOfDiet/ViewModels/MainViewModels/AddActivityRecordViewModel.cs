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
        public string Title { get; set; }
        public string IconPath { get; set; }

        [ObservableProperty] private string searchNameActivity;

        public ObservableCollection<MyActivity> SearchResultsActivity { get; } = new();

        [ObservableProperty] private DateTime dateActivity = DateTime.Now.Date;
        [ObservableProperty] private DateTime timeActivity = DateTime.Now;

        [RelayCommand]
        private void SearchActivity()
        {
            SearchResultsActivity.Clear();
            foreach (var activity in DBWork.SearchActivitiesByName(SearchNameActivity))
                SearchResultsActivity.Add(activity);
        }

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
