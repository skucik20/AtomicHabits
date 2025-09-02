﻿using System.Configuration;
using System.Data;
using System.Windows;

namespace WpfApp.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // run Bootstraper
            var bootstrapper = new Bootstrapper.Bootstrapper();
            bootstrapper.Run();
        }
    }

}
