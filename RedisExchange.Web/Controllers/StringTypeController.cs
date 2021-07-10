using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchange.Web.Services;
using StackExchange.Redis;

namespace RedisExchange.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _db;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _db = _redisService.GetDb(0);
        }

        public IActionResult Index()
        {
            

            _db.StringSet("name", "Anıl Cırık");
            _db.StringSet("ziyaretçi", 100);

            return View();
        }

        public IActionResult Show()
        {
            var value = _db.StringGet("name");

            _db.StringIncrement("ziyaretçi", 1);

            if(value.HasValue)
            {
                ViewBag.name = value;
            }

            return View();
        }
    }
}