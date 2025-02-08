using Xamarin.Forms;

namespace XamarinPushNotification
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

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
}
