using System.Collections.ObjectModel;
using System.Windows;
using Autodesk.Revit.UI;
using OpenAEC.Revit.Utilities;
using GalaSoft.MvvmLight.CommandWpf;
using OpenAEC.Wpf.Extensions;

namespace OpenAEC.Revit.Management.AddinManager
{
    public class AddinManagerViewModel : ViewModelBaseEx
    {
        public UIApplication UiApp { get; }
        public AddinManagerModel Model;
        public ObservableCollection<AddinWrapper> Addins { get; set; }
        public RelayCommand<Window> CloseCommand { get; }
        public RelayCommand<Window> ApplyCommand { get; }

        public AddinManagerViewModel(UIApplication uiApp)
        {
            UiApp = uiApp;
            Model = new AddinManagerModel(UiApp);
            Addins = Model.LoadAddins();
            CloseCommand = new RelayCommand<Window>(OnClose);
            ApplyCommand = new RelayCommand<Window>(OnApply);
        }

        public void OnClose(Window win)
        {
            win?.Close();
        }

        public void OnApply(Window win)
        {
            Model.UpdateAddins(Addins);
            win?.Close();
        }
    }
}