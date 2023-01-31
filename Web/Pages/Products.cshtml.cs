using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using Web2.Models;

namespace Web.Pages
{
    public class ProductsModel : PageModel
    {
        public void OnGet()
        {
        }
        public async Task<Product?> GetAllProducts()
        {
			var client = new HttpClient();
			var uri = new Uri("http://localhost:5001/api/products");
			var response = await client.GetAsync(uri);

            return JsonConvert.DeserializeObject<Product>(response.Content.ReadAsStringAsync().Result);
		}
    }
}
