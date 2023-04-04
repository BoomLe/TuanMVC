using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Areas.Foods.Controllers
{
    [Area("Foods")]
   
    public class FoodController : Controller
    {
        private readonly FoodService _foodService;
        private readonly ILogger<FoodController> _logger;
        public FoodController(FoodService foodService,ILogger<FoodController> logger)
        {
            _foodService = foodService;
            _logger = logger;

        }
        // [Route("/Danh-sach-top-10-mon-an/")]
        public IActionResult Index()
        {
            return View();
        }
        [BindProperty(SupportsGet = true, Name ="action")]
        public string Name {set;get;}
        public IActionResult Dosa()
        {
            var food = _foodService.Where(p => p.Name == Name ).FirstOrDefault();

            return View("Detail", food);

        }
           public IActionResult PekingDuck()
        {
            var food = _foodService.Where(p => p.Name == Name ).FirstOrDefault();

            return View("Detail", food);

        }
           public IActionResult Sushi()
        {
            var food = _foodService.Where(p => p.Name == Name ).FirstOrDefault();

            return View("Detail", food);

        }
           public IActionResult Rendang()
        {
            var food = _foodService.Where(p => p.Name == Name ).FirstOrDefault();

            return View("Detail", food);

        }
           public IActionResult Ramen()
        {
            var food = _foodService.Where(p => p.Name == Name ).FirstOrDefault();

            return View("Detail", food);

        }
           public IActionResult Pho()
        {
            var food = _foodService.Where(p => p.Name == Name ).FirstOrDefault();

            return View("Detail", food);

        }
           public IActionResult Paella()
        {
            var food = _foodService.Where(p => p.Name == Name ).FirstOrDefault();

            return View("Detail", food);

        }
           public IActionResult Lasagna()
        {
            var food = _foodService.Where(p => p.Name == Name ).FirstOrDefault();

            return View("Detail", food);

        }
           public IActionResult Kebab()
        {
            var food = _foodService.Where(p => p.Name == Name ).FirstOrDefault();

            return View("Detail", food);

        }
           public IActionResult Goulash()
        {
            var food = _foodService.Where(p => p.Name == Name ).FirstOrDefault();

            return View("Detail", food);

        }
            public IActionResult FoodInfo(int id)
        {
            var food = _foodService.Where(p => p.Id == id ).FirstOrDefault();

            return View("Detail", food);

        }
    }
}