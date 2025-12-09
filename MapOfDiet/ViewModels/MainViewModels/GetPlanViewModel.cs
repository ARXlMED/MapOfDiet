using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.ViewModels
{
    public partial class GetPlanViewModel : ObservableObject
    {
        public string Title { get; set; }
        public string IconPath { get; set; }
    }
}
