using Android.App;
using Android.Content;
using Android.Util;
using Firebase.Messaging;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XamarinPushNotification.Droid
{
    [Service(Exported = true)] // 👈 Agrega Exported = false
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";
        AndroidNotificationManager androidNotification = new AndroidNotificationManager();

        public override void OnMessageReceived(RemoteMessage message)
        {
            IDictionary<string, string> MensajeData = message.Data;

            //string Titulo = MensajeData["title"];
            //string SubTitulo = MensajeData["body"];

            //androidNotification.CrearNotificacionLocal(Titulo, SubTitulo);

            Log.Debug(TAG, "From: " + message.From);
            Log.Debug(TAG, "Notification Message Body: " + message.GetNotification().Body);

            androidNotification.CrearNotificacionLocal(message.GetNotification().Title, message.GetNotification().Body);

            //viene de Xamarin.Forms
            //se recibe en MainPage.xaml.cs
            MessagingCenter.Send<string>(message.GetNotification().Title, "MensajeFireBase");
        }

        public override void OnNewToken(string token)
        {
            base.OnNewToken(token);

            Preferences.Set("TokenFirebase", token);
            sedRegisterToken(token);
        }

        public void sedRegisterToken(string token)
        {
            //Tu código para registrar el token a tu servidor y base de datos
        }
    }
}
