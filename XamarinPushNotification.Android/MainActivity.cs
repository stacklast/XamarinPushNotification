using Android.App;
using Android.Content.PM;
using Android.Gms.Common;
using Android.OS;
using Android.Runtime;
using FFImageLoading.Forms.Platform;
using System.Net;
using Xamarin.Essentials;

namespace XamarinPushNotification.Droid
{
    [Activity(Label = "XamarinPushNotification", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            IsPlayServicesAvailable();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CachedImageRenderer.InitImageViewHandler();

#if DEBUG
            // During development, we can trust all certificates, including ASP.NET developer certificates
            // DO NOT ENABLE THIS IN RELEASE BUILDS
            // Ignorar la validación del certificado SSL (solo para desarrollo)
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
#endif

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            bool isGooglePlayService = resultCode != ConnectionResult.Success;
            Preferences.Set("isGooglePlayService", isGooglePlayService);
        }
    }
}