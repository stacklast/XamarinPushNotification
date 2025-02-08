using Android.App;
using Android.Content;
using Android.Util;
using Firebase.Messaging;
using Xamarin.Essentials;

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
            Log.Debug(TAG, "From: " + message.From);
            Log.Debug(TAG, "Notification Message Body: " + message.GetNotification().Body);

            androidNotification.CrearNotificacionLocal(message.GetNotification().Title, message.GetNotification().Body);
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
