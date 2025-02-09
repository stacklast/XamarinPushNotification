using MediatR;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XamarinPushNotification.Configuration;
using XamarinPushNotification.Models;

namespace XamarinPushNotification.Commands
{

    public class PushNotificationCommand : IRequest<bool>
    {
        public Notification Notification { get; set; }
        public PushNotificationCommand(Notification notification) => Notification = notification;
    }

    public class PushNotificationCommandHandler : IRequestHandler<PushNotificationCommand, bool>
    {
        private readonly HttpClient _httpClient;

        public PushNotificationCommandHandler()
        {
            var clientHandler = new HttpClientHandler
            {
                //bypass SSL
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            _httpClient = new HttpClient(clientHandler);
        }
        public async Task<bool> Handle(PushNotificationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var json = JsonConvert.SerializeObject(request.Notification);
                var response = await _httpClient.PostAsync(ApiConfig.NotificationPushEndpoint, new StringContent(json, Encoding.UTF8, "application/json"));
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error de red: {ex.Message}");
                throw new Exception("No se pudo enviar la notificacion.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("Ocurrió un error al enviar la notificacion.");
            }
        }
    }
}
