using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;

namespace SVGGenerator
{
    public partial class App 
    {
        private ServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _serviceProvider = CreateServiceProvider();
            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            MainWindow.Show();
        }

        private static ServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddSingleton<ViewModels.MainViewModel>();
            services.AddSingleton<MainWindow>();
            return services.BuildServiceProvider();
        }
    }

    public partial class MainWindow : Window
    {
        private readonly ViewModels.MainViewModel _mainViewModel;

        private MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(ViewModels.MainViewModel mainViewModel)
            : this()
        {
            _mainViewModel = mainViewModel;
            DataContext = _mainViewModel;
        }

        private void RenderImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _mainViewModel.OnImageSizeChanged((int)e.NewSize.Width, (int)e.NewSize.Height);
        }
    }
}
