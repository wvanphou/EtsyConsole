using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;

public class Program
{
    static string baseUrl = "https://openapi.etsy.com/";

    public static void Main()
    {
        //ShopIds for: HicklinHomestead, PrettyTape,sicksoaps, westskycreations, musictreebox, patternedpaintroller, Rustic4You, JoycesTastyCake,JustInTimeSofthats,TheMojaveMoon
        List<int> listOfShopIds = new List<int> {7759670, 5663838, 6749410, 19329201, 19120252, 7047893, 8103090, 10295953, 10682245, 12817892 };

        //Iterate through the list of ShopIds
        foreach (var id in listOfShopIds)
        {
            //Get all listings for the Shop
            var d_listings = GetListingsByShopId(id).Result;
            var listings = (dynamic)d_listings.results;
            int i = 0;
            
            //Iterate through the listings and only display the first 5
            foreach (dynamic listing in listings)
            {
                if (i == 0)
                {
                    Console.WriteLine("-----------------------------------------------Shop ID: {0} --------------------------------------------------------------------------", id);
                }
                
                if (i < 5)
                {
                    Console.WriteLine("{0}. ListingId: {1} \r\nTitle: {2} \r\nDescription: {3} ", i+1, listing.listing_id, listing.title, listing.description);
                    Console.WriteLine("\r\n\r\n");
                    i++;
                }
                else
                    break;
            }
        }
        Console.Read();
    }
   
    public static async Task<dynamic> GetListingsByShopId(int shopID)
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
            //parse the results as json
            string jsonString = await response.Content.ReadAsStringAsync();
            object responseData = JsonConvert.DeserializeObject(jsonString);
            //return dynamic object
            return (dynamic)responseData;
        }
    }
}