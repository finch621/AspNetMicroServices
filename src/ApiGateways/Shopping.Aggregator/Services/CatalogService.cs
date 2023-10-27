using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public class CatalogService : ICatalogService
{
    private readonly HttpClient _httpClient;

    public CatalogService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<IEnumerable<CatalogModel>> GetCatalog()
    {
        var response = await _httpClient.GetAsync("/api/v1/catalog");

        return await response.ReadContentAs<List<CatalogModel>>() ?? new List<CatalogModel>();
    }

    public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category)
    {
        var response = await _httpClient.GetAsync($"/api/v1/catalog/category/{category}");

        return await response.ReadContentAs<List<CatalogModel>>() ?? new List<CatalogModel>();
    }

    public async Task<CatalogModel?> GetCatalogById(string id)
    {
        var response = await _httpClient.GetAsync($"/api/v1/catalog/{id}");

        return await response.ReadContentAs<CatalogModel>();
    }
}
