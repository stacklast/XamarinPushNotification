using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xamarin.Forms;
using XamarinPushNotification.Queries;

namespace XamarinPushNotification
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {

            InitializeComponent();

            ConfigureServices();

            // Obtener el Mediator desde el contenedor de servicios
            var mediator = ServiceProvider.GetService<IMediator>();

            MainPage = new MainPage(mediator);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private void ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            // Register HttpClient
            serviceCollection.AddHttpClient();

            // Register MediatR and Handlers
            serviceCollection.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(GetProductsHandler).Assembly);
            });

            // Register Services

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
