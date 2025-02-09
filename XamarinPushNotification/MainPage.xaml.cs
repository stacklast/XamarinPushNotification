using MediatR;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Net;
using Xamarin.Forms;
using XamarinPushNotification.Commands;
using XamarinPushNotification.Models;
using XamarinPushNotification.Queries;
using XamarinPushNotification.Views;

namespace XamarinPushNotification
{
    public partial class MainPage : ContentPage
    {
        private readonly IMediator _mediator;

        public ObservableCollection<Producto> Products { get; set; }
        public ObservableCollection<Producto> CartItems { get; set; }
        private int _cartItemCount;
        public int CartItemCount
        {
            get => _cartItemCount;
            set
            {
                _cartItemCount = value;
                OnPropertyChanged(nameof(CartItemCount));
            }
        }

        public MainPage(IMediator mediator)
        {
            InitializeComponent();

            _mediator = mediator;


            Products = new ObservableCollection<Producto>();
            CartItems = new ObservableCollection<Producto>();

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

        private async void OnAddToCartClickedAsync(object sender, EventArgs e)
        {
            var button = sender as Button;
            var product = button?.BindingContext as Producto;

            if (product != null)
            {
                CartItems.Add(product);
                CartItemCount = CartItems.Count;
                btnCarrito.Text = $"Carrito ({CartItems.Count})";

                var message = new Notification
                {
                    Title = "Producto Agregado " + product.Titulo.ToString(),
                    Body = "Precio:" + product.Precio.ToString(),
                    Tokens = new List<string>
                    {
                        "e6x08-t2SDStMbky55rudT:APA91bE8l3X-0a8StX-xZyUFdhKUkomT1gOqkK3QZNIjctlPW8xQavHKATCNTn_tUZxDinvs6vUYDuX84oKvfnT2R7R7GKuT_BJecaJQeAgrjb7Am0FykkI"
                    },
                    CustomData = null
                };

                var notification = await _mediator.Send(new PushNotificationCommand(message));

                Console.WriteLine($"Producto agregado al carrito: {product.Titulo}");
            }
        }

        private async void OnCartClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CartPage(CartItems));
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
