using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {

            if (!_memoryCache.TryGetValue<string>("time", out string cacheTime))
            {

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

                options.AbsoluteExpiration = DateTime.Now.AddSeconds(10);

                //options.SlidingExpiration = TimeSpan.FromSeconds(10);
                options.Priority = CacheItemPriority.High;

                options.RegisterPostEvictionCallback((key, value, reason, state) => {

                    //delagate
                    _memoryCache.Set("callback", $"{key} --> {value} --> Sebep : {reason}");
                
                
                });


                _memoryCache.Set<string>("time", DateTime.Now.ToString(), options);

                Product p = new Product
                {
                    Id = 1,
                    Name = "Kalem",
                    Price = 200
                };

                _memoryCache.Set<Product>("product:1", p);
            }






            return View();
        }

        public IActionResult Show()
        {
            //_memoryCache.Remove("time"); //deleted

            // bu keye ait olan değeri almaya çalışır eğer yoksa oluşturur.
            //_memoryCache.GetOrCreate<string>("time", entry =>
            //{

            //    return DateTime.Now.ToString();

            //});

            _memoryCache.TryGetValue<string>("time", out string cacheTime);
            _memoryCache.TryGetValue<string>("callback", out string cacheCallBack);

            ViewBag.Time = _memoryCache.Get<string>("time");
            ViewBag.Callback = _memoryCache.Get<string>("callback");

            ViewBag.Product = _memoryCache.Get<Product>("product:1");




            return View();
        }
    }
}