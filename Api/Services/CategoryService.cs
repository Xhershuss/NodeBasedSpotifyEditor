using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyEditor.Api.Auth;
using SpotifyEditor.Api.Models;
using SpotifyEditor.Api.Models.Responses;
using Newtonsoft.Json;

namespace SpotifyEditor.Api.Services
{
    public class CategoryService : BaseService
    {
        public CategoryService(HttpClient spotifyApiClient) : base(spotifyApiClient) { }


        public async Task<List<Category>> GetCategories()
        {
            int limit = 50;
            int offset = 0;

            var categoriesList = new List<Category>();

            int i = 0;

            while (true)
            {
                Console.WriteLine(i++);
                await EnsureAuthTokens();
                var url = $"browse/categories?limit={limit}&offset={offset}";
                var response = await _spotifyApiClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var categoriesResponse = JsonConvert.DeserializeObject<CategoryList>(json);
                
                categoriesList.AddRange(categoriesResponse.Categories.Items);
                if (categoriesResponse.Categories.Items.Count < limit)
                {
                    break;
                }
                offset += limit;
            }

            return categoriesList;
            
        }
        public async Task<Category> GetCategory(string categoryId)
        {
            await EnsureAuthTokens();
            var response = await _spotifyApiClient.GetAsync($"browse/categories/{categoryId}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var category = Category.FromJson(json);
            return category;
        }

    }
}
