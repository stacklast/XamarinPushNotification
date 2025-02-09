using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using XamarinPushNotification.Configuration;
using XamarinPushNotification.Models;

namespace XamarinPushNotification.Queries
{
    public class GetProductsQuery : IRequest<IEnumerable<Producto>> { }

    public class GetProductsHandler : IRequestHandler<GetProductsQuery, IEnumerable<Producto>>
    {
        private readonly HttpClient _httpClient;

        public GetProductsHandler(HttpClient httpClient)
        {
            var clientHandler = new HttpClientHandler
            {
                //bypass SSL
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            _httpClient = new HttpClient(clientHandler);
        }

        public async Task<IEnumerable<Producto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {

                // Realizar la solicitud HTTP con cancelación
                var response = await _httpClient.GetAsync(ApiConfig.ProductosEndpoint, cancellationToken);
                response.EnsureSuccessStatusCode();

                var responseData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Producto>>(responseData) ?? new List<Producto>();
            }
            catch (HttpRequestException ex)
            {
                // Manejo de errores de conexión
                Console.WriteLine($"Error en la solicitud HTTP: {ex.Message}");
                return new List<Producto>();
            }
            catch (TaskCanceledException ex)
            {
                // Manejo si la solicitud se cancela (timeout o manual)
                Console.WriteLine($"Solicitud cancelada: {ex.Message}");
                return new List<Producto>();
            }
            catch (Exception ex)
            {
                // Manejo de errores inesperados
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return new List<Producto>();
            }
        }
    }
}
