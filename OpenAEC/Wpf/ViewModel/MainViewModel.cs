using System.Diagnostics;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using OpenAEC.Wpf.Extensions;

namespace OpenAEC.Wpf.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public string ToolName { get; set; }
        public string ToolDescription { get; set; }
        public string HelpPageLink { get; set; }
        public string WindowTitle { get; set; }
        public RelayCommand<Window> CloseWindowCommand { get; }
        public RelayCommand NavigateToHelpCommand { get; }

        public MainViewModel()
        {
            CloseWindowCommand = new RelayCommand<Window>(CloseWindow);
            NavigateToHelpCommand = new RelayCommand(NavigateToHelp);
        }

        private static void CloseWindow(Window window)
        {
            window?.Close();
        }

        private void NavigateToHelp()
        {
            Process.Start(HelpPageLink);
        }

        private ViewModelBaseEx _currentPageViewModel;
        public ViewModelBaseEx CurrentPageViewModel
        {
            get => _currentPageViewModel;

            set
            {
                if (_currentPageViewModel == value)
                {
                    return;
                }

                _currentPageViewModel = value;
                RaisePropertyChanged(() => CurrentPageViewModel);
            }
        }
    }
}
