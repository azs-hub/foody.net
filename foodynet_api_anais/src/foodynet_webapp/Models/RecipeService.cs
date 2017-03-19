using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace foodynet_webapp.Models
{
    public class RecipeService
    {
        HttpClient _client = new HttpClient();
        AuthTokenService _token;

        public RecipeService(AuthTokenService token)
        {
            _token = token;
            _client.BaseAddress = new Uri("http://localhost:5001");
            MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
        }
        

        public PaginatedList GetAll(string page)
        {
            HttpResponseMessage response = _client.GetAsync("/api/recipes?p=" + page).Result;
            string stringData = response.Content.ReadAsStringAsync().Result;
            PaginatedList data = JsonConvert.DeserializeObject<PaginatedList>(stringData);

            return data;
        }

        public Recipe GetRecipeById(int id)
        {
            HttpResponseMessage response = _client.GetAsync("/api/recipes/" + id).Result;
            string stringData = response.Content.ReadAsStringAsync().Result;
            Recipe data = JsonConvert.DeserializeObject<Recipe>(stringData);

            return data;
        }

        public Recipe CreateRecipe(Recipe recipe)
        {
            if (_token.GetToken() == null)
                return null;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.GetToken());

            HttpResponseMessage response = _client.PostAsJsonAsync("/api/recipes", recipe).Result;
            string stringData = response.Content.ReadAsStringAsync().Result;
            Recipe data = JsonConvert.DeserializeObject<Recipe>(stringData);
            if (response.IsSuccessStatusCode)
                return data;
            return null;
        }

        public Recipe UpdateRecipe(Recipe recipe)
        {
            if (_token.GetToken() == null)
                return null;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.GetToken());

            HttpResponseMessage response = _client.PutAsJsonAsync("/api/recipes", recipe).Result;
            string stringData = response.Content.ReadAsStringAsync().Result;
            Recipe data = JsonConvert.DeserializeObject<Recipe>(stringData);
            if (response.IsSuccessStatusCode)
                return data;
            return null;
        }

        public Boolean RemoveRecipe(Recipe recipe)
        {
            if (_token.GetToken() == null)
                return false;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.GetToken());

            HttpResponseMessage response = _client.DeleteAsync("/api/recipes/" + recipe.Id).Result;
            string stringData = response.Content.ReadAsStringAsync().Result;
            Recipe data = JsonConvert.DeserializeObject<Recipe>(stringData);
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }
    }
}
