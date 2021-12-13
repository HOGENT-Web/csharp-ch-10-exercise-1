using Client.Extensions;
using Client.Infrastructure;
using Shared.Products;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Client.Products
{
    public class ProductService : IProductService
    {
        private readonly HttpClient authenticatedClient;
        private readonly PublicClient publicClient;
        private const string endpoint = "api/product";
        public ProductService(HttpClient authenticatedClient, PublicClient publicClient)
        {
            this.authenticatedClient = authenticatedClient;
            this.publicClient = publicClient;
        }
        public async Task<ProductResponse.Create> CreateAsync(ProductRequest.Create request)
        {
            var response = await authenticatedClient.PostAsJsonAsync(endpoint, request);
            return await response.Content.ReadFromJsonAsync<ProductResponse.Create>();
        }

        public async Task DeleteAsync(ProductRequest.Delete request)
        {
            await authenticatedClient.DeleteAsync($"{endpoint}/{request.ProductId}");
        }

        public async Task<ProductResponse.GetDetail> GetDetailAsync(ProductRequest.GetDetail request)
        {
            var response = await publicClient.Client.GetFromJsonAsync<ProductResponse.GetDetail>($"{endpoint}/{request.ProductId}");
            return response;
        }

        public async Task<ProductResponse.GetIndex> GetIndexAsync(ProductRequest.GetIndex request)
        {
            var queryParameters = request.GetQueryString();
            var response = await publicClient.Client.GetFromJsonAsync<ProductResponse.GetIndex>($"{endpoint}?{queryParameters}");
            return response;
        }

        public async Task<ProductResponse.Edit> EditAsync(ProductRequest.Edit request)
        {
            var response = await authenticatedClient.PutAsJsonAsync(endpoint, request);
            return await response.Content.ReadFromJsonAsync<ProductResponse.Edit>();
        }
    }
}
