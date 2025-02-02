using Serilog;
using System.Configuration;
using System.Data;
using System.Windows;

namespace DC.Translator.Tool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public App()
        {

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //Database.Initialize();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Serilog.Log.CloseAndFlush();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            Logging.Init();
            containerRegistry.Register<IMyDialogService, MyDialogService>();
            containerRegistry.RegisterSingleton<ILogger>(() => Serilog.Log.Logger);
            containerRegistry.Register<BaiduTranslationClient>();
            containerRegistry.RegisterDialog<CreateEditStaticItemDialog, CreateEditStaticItemVM>();
            containerRegistry.RegisterDialog<CreateEditDynamicItemDialog, CreateEditDynamicItemVM>();
        }

        protected override Window CreateShell()
        {
            return new MainWindow();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
            ViewModelLocationProvider.Register<MainWindow, MainViewModel>();
            ViewModelLocationProvider.Register<CreateEditStaticItemDialog, CreateEditStaticItemVM>();
            ViewModelLocationProvider.Register<CreateEditDynamicItemDialog, CreateEditDynamicItemVM>();
        }

        protected override IContainerExtension CreateContainerExtension()
        {
            return base.CreateContainerExtension();
        }
    }

}
