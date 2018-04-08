using MasterYodaRecipes.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace MasterYodaRecipes
{
    public class SearchRecipesManager
    {
        //------------------------------------------------
        //Go to https://developer.edamam.com/edamam-recipe-api to request free API_ID and APP_KEY and set values below
        private string appID = "[API_ID]";
        private string appKey = "[APP_KEY]";
        //------------------------------------------------

        private const string URL = "https://api.edamam.com/search?";
        private string urlParameters = "?q={0}&app_id={1}&&app_key={2}";

        /// <summary>
        /// Searches edamam api for recipes with the given parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public async Task<IList<Recipe>> GetRecipesAsync(List<string> searchParameters)
        {
            List<Recipe> recipes = new List<Recipe>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string searchQuery = string.Join(",", searchParameters);

                HttpResponseMessage response = await client.GetAsync(string.Format(urlParameters, searchQuery, appID, appKey));
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();
                    var result = JsonConvert.DeserializeObject<Rootobject>(jsonString.Result);
                    foreach (var item in result.hits)
                    {
                        recipes.Add(item.recipe);
                    }
                }
                else if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                }
            }
            return recipes;
        }
    }
}