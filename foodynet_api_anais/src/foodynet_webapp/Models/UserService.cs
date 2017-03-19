//using foodynet_api_anais.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace foodynet_webapp.Models
{
    public class UserService
    {
        HttpClient _client = new HttpClient();
        AuthTokenService _token;

        public UserService(AuthTokenService token)
        {
            _token = token;
            _client.BaseAddress = new Uri("http://localhost:5001");
            MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            
            if (_token.GetToken() != null)
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.GetToken());
        }

        public Boolean Signup(User user)
        {
            HttpResponseMessage response = _client.PostAsJsonAsync("/api/users/signup", user).Result;
            
            if (response.IsSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();
                return true;
            }
            else
            {
                response.Dispose();
                return false;
            }
            
        }

        public Boolean Signin(User user)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair
                <string, string>("email", user.Email),
                new KeyValuePair<string, string>("password", user.Password)
            });
            HttpResponseMessage response = _client.PostAsync("/user/login", formContent).Result;
            
            if (response.IsSuccessStatusCode)
            {
                AuthToken model = null;
                var content = response.Content.ReadAsStringAsync();
                model = JsonConvert.DeserializeObject<AuthToken>(content.Result);
                _token.SetToken(model.access_token);

                if (model.user.Admin == 1)
                    _token.SetAdmin(model.user.Admin);

                return true;
            }
            else
                return false;
        }

        public IEnumerable<Favorite> GetFavorites()
        {
            if (_token.GetToken() == null) 
                return null;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.GetToken());
            HttpResponseMessage response = _client.GetAsync("/api/users/favorites").Result;
            string stringData = response.Content.ReadAsStringAsync().Result;

            IEnumerable<Favorite> data = JsonConvert.DeserializeObject<IEnumerable<Favorite>>(stringData);

            return data;
        }

        public Boolean AddFavorite(Favorite favorite)
        {
            if (_token.GetToken() != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.GetToken());
                HttpResponseMessage response = _client.PostAsJsonAsync("/api/users/favorite", favorite).Result;
                if (response.IsSuccessStatusCode)
                    return true;
            }
            
            return false;
        }

        public Boolean RemoveFavorite(int id)
        {
            if (_token.GetToken() != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.GetToken());
                HttpResponseMessage response = _client.DeleteAsync("/api/users/favorite/" + id).Result;
                if (response.IsSuccessStatusCode)
                    return true;
            }

            return false;
        }

        public IEnumerable<User> GetAll()
        {
            if (_token.GetToken() == null)
                return null;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.GetToken());
            HttpResponseMessage response = _client.GetAsync("/api/users/getall").Result;
            string stringData = response.Content.ReadAsStringAsync().Result;

            IEnumerable<User> data = JsonConvert.DeserializeObject<IEnumerable<User>>(stringData);

            return data;
        }

        public Boolean Update(User user)
        {
            if (_token.GetToken() != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.GetToken());
                HttpResponseMessage response = _client.PutAsJsonAsync("/api/users/", user).Result;
                if (response.IsSuccessStatusCode)
                    return true;
            }

            return false;
        }

        public Boolean Remove(int id)
        {
            if (_token.GetToken() != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.GetToken());
                HttpResponseMessage response = _client.DeleteAsync("/api/users/" + id).Result;
                if (response.IsSuccessStatusCode)
                    return true;
            }

            return false;
        }

        public Boolean IsAuth()
        {
            if (_token.GetToken() == null)
                return false;
            return true;
        }

        public Boolean IsAdmin()
        {
            return _token.IsAdmin();
        }

        public void Logout()
        {
            _token.Remove();
        }
    }
}
