using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows;

namespace OpenAEC.Wpf.Extensions
{
    public class ViewModelBaseEx : ViewModelBase
    {
        //public virtual RelayCommand<Window> ApplyCommand { get; }
        //public virtual RelayCommand<Window> CloseCommand { get; }

        public ViewModelBaseEx()
        {
            //ApplyCommand = new RelayCommand<Window>(Apply);
            //CloseCommand = new RelayCommand<Window>(Close);
        }

        //public virtual void Apply(Window window)
        //{
        //    throw new NotImplementedException("Apply Command was not implemented.");
        //}

        //public virtual void Close(Window window)
        //{
        //    throw new NotImplementedException("Close Command was not implemented.");
        //}
    }
}
