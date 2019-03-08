using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;


public class Program
{
    static string baseUrl = "https://openapi.etsy.com/";
    public class results
    {
       [JsonProperty(PropertyName = "results")]
        public List<listing> shopListings { get; set; }
    }
    public class listing
    {
        public string listing_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }
    
    public static void Main()
    {
        //ShopIds for: HicklinHomestead, PrettyTape,sicksoaps, westskycreations, musictreebox, patternedpaintroller, Rustic4You, JoycesTastyCake,JustInTimeSofthats,TheMojaveMoon
        List<int> listOfShopIds = new List<int> {7759670, 5663838, 6749410, 19329201, 19120252, 7047893, 8103090, 10295953, 10682245, 12817892 };

        List<string> stringsToCheck = new List<string>() {"meditation", "popular", "happy", "serenity", "peaceful" };

        foreach (var id in listOfShopIds)
        {
            //Get all listings for the Shop
            var originalListings = GetListingsByShopId(id).Result;

            //Filter through listings by checking for specific words in the description or title
            var filteredListings = (from s in originalListings.shopListings
                                       where stringsToCheck.Any(w=> s.title.Contains(w)) || stringsToCheck.Any(w => s.description.Contains(w))

                                    select s).Take(5);

            Console.WriteLine("-----------------------------------------------Shop ID: {0} --------------------------------------------------------------------------", id);

             //Print the top 5 filtered listings
            foreach (var item in filteredListings)
            {
                Console.WriteLine("ListingId: {0} \r\nTitle: {1} \r\nDescription: {2} ", item.listing_id, item.title, item.description);
                Console.WriteLine("\r\n\r\n");
            }
        }
        Console.Read();
    }
   
    public static async Task<results> GetListingsByShopId(int shopID)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();

            //Set headers to return data in json format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //api url
            string url = "v2/shops/" +shopID+ "/listings/active?api_key=4q797kyrqkofo18s1hrzzo29";
            HttpResponseMessage response = await client.GetAsync(url);

            //parse the results and map to object
            string jsonString = await response.Content.ReadAsStringAsync();
            var jsonResults = JsonConvert.DeserializeObject<results>(jsonString);
            return (jsonResults);
        }
    }
}