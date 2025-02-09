using Xamarin.Forms;

namespace XamarinPushNotification.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string ImagenUrl { get; set; }
        public ImageSource ImageSource { get; internal set; }
    }
}
