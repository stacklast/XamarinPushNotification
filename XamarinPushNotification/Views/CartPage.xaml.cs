using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinPushNotification.Models;

namespace XamarinPushNotification.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartPage : ContentPage
    {
        public ObservableCollection<Producto> CartItems { get; }

        public CartPage(ObservableCollection<Producto> cartItems)
        {
            InitializeComponent();
            CartItems = cartItems;
            BindingContext = this;
        }
    }
}