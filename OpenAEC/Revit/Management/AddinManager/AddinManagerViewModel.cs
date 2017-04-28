using System.Collections.ObjectModel;
using System.Windows;
using Autodesk.Revit.UI;
using OpenAEC.Revit.Utilities;
using GalaSoft.MvvmLight.CommandWpf;
using OpenAEC.Wpf.Extensions;
using System.Diagnostics;

namespace OpenAEC.Revit.Management.AddinManager
{
    public class AddinManagerViewModel : ViewModelBaseEx
    {
        public UIApplication UiApp { get; }
        public AddinManagerModel Model;
        public ObservableCollection<AddinWrapper> Addins { get; set; }
        public RelayCommand<Window> CloseCommand { get; }
        public RelayCommand<Window> ApplyCommand { get; }
        public RelayCommand NavigateToHelpCommand { get; set; }

        public AddinManagerViewModel(UIApplication uiApp)
        {
            UiApp = uiApp;
            Model = new AddinManagerModel(UiApp);
            Addins = Model.LoadAddins();
            CloseCommand = new RelayCommand<Window>(OnClose);
            ApplyCommand = new RelayCommand<Window>(OnApply);
            NavigateToHelpCommand = new RelayCommand(OnNavigateToHelp);
        }

        private void OnClose(Window win)
        {
            win?.Close();
        }

        private void OnApply(Window win)
        {
            Model.UpdateAddins(Addins);
            win?.Close();
        }

        private static void OnNavigateToHelp()
        {
            Process.Start("https://github.com/design-technology/OpenAEC/wiki/Addin-Manager");
        }
    }
}