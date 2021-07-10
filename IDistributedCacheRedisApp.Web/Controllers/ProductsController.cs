using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distirubuteCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distirubuteCache = distributedCache;
        }
        public async Task<IActionResult> Index()
        {
            var cacheEnryOptions = new DistributedCacheEntryOptions();
            cacheEnryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(30); // 30 dakika sonra silincek

            var product = new Product
            {
                Id = 1,
                Name = "Kalem",
                Price = 100
            };

            string jsonProduct = JsonConvert.SerializeObject(product);

            await _distirubuteCache.SetStringAsync("product:1", jsonProduct, cacheEnryOptions);

            product.Id = 2;
            product.Name = "Silgi";
            jsonProduct = JsonConvert.SerializeObject(product);

            await _distirubuteCache.SetStringAsync("product:2", jsonProduct, cacheEnryOptions);




            //_distirubuteCache.SetString("name", "anil", cacheEnryOptions);
            //await _distirubuteCache.SetStringAsync("surname", "cırık", cacheEnryOptions);

            return View();
        }

        public IActionResult Show()
        {
            string jsonProduct = _distirubuteCache.GetString("product:1");

            var product = JsonConvert.DeserializeObject<Product>(jsonProduct);

            ViewBag.product = product;

            return View();
        }

        public IActionResult Remove()
        {
            _distirubuteCache.Remove("name");

            return View();
        }

        public IActionResult ImageCache()
        {

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/instaprogfilimg.JPG");

            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            _distirubuteCache.Set("anlimg", imageByte);


            return View();
        }

        public IActionResult ImageUrl()
        {
            byte[] imgByte = _distirubuteCache.Get("anlimg");

            return File(imgByte, "image/jpg");
        }
    }
}