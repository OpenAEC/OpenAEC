using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using OpenAEC.Wpf.Extensions;

namespace OpenAEC.Wpf.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public string WindowTitle { get; set; }
        public RelayCommand NavigateToHelpCommand { get; }
        public RelayCommand NavigateToCreditsCommand { get; }

        public MainViewModel()
        {
            NavigateToHelpCommand = new RelayCommand(OnNavigateToHelp);
            NavigateToCreditsCommand = new RelayCommand(OnNavigateToCredits);
        }

        private static void OnNavigateToHelp()
        {
            Process.Start("https://github.com/design-technology/OpenAEC");
        }

        private void OnNavigateToCredits()
        {
            Process.Start("https://github.com/design-technology/OpenAEC/wiki/Credits");
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
