using MediatR;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Net;
using Xamarin.Forms;
using XamarinPushNotification.Models;
using XamarinPushNotification.Queries;

namespace XamarinPushNotification
{
    public partial class MainPage : ContentPage
    {
        private readonly IMediator _mediator;

        public ObservableCollection<Producto> Products { get; set; }


        public MainPage(IMediator mediator)
        {
            InitializeComponent();

            _mediator = mediator;


            Products = new ObservableCollection<Producto>();

            LoadProducts();

            BindingContext = this;

            BroadcastMessages();
        }

        public MainPage()
        {

        }

        private async void LoadProducts()
        {
            try
            {
                var products = await _mediator.Send(new GetProductsQuery());
                foreach (var product in products)
                {
                    Products.Add(product);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void BroadcastMessages()
        {
            //Recibir mensajes en segundo plano y ejecutar acciones
            MessagingCenter.Subscribe<string>(this, "MensajeFireBase", (value) =>
            {
                //ejecutar desde el hilo principal
                Device.BeginInvokeOnMainThread(() =>
                {
                    txtMensaje.Text = "Mensaje" + value;

                    //si se quiere dejar de recibir eventos de la suscripcion
                    //MessagingCenter.Unsubscribe<string>(this, "MensajeFireBase");
                });
            });
        }

    }

    public class ImageSourceConverter : IValueConverter
    {
        static WebClient Client = new WebClient();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var byteArray = Client.DownloadData(value.ToString());
            return ImageSource.FromStream(() => new MemoryStream(byteArray));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
