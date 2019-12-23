using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Crudelicious.Models;
using Microsoft.EntityFrameworkCore;

namespace Crudelicious.Controllers
{
    public class HomeController : Controller
    {
        private HomeContext dbContext;

        // here we can "inject" our context service into the constructor
        public HomeController(HomeContext context)
        {
            dbContext = context;
        }
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            List<Dish> AllDishes = dbContext.Dishes.ToList();
            // ViewBag.Food = AllDishes;
            return View(AllDishes);
        }
        [HttpPost("new")]
        public IActionResult Home(Dish newfood)
        {
            if(ModelState.IsValid)
            {
                dbContext.Add(newfood); 
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else 
            {
                return View("NewDish");
            }
        }
        [HttpGet("new")]
        public IActionResult NewDish()
        {
            return View("NewDish");
        }

        [HttpGet("single/{DishId}")]
        public IActionResult Single(int DishId)
        {
            Dish OneDish = dbContext.Dishes.FirstOrDefault(food => food.DishId == DishId);
            return View("Single", OneDish);
        }

        [HttpPost("edit/{DishId}")]
        public IActionResult Edit(int DishId, Dish val)
        {
            Dish RetrievedDish = dbContext.Dishes.FirstOrDefault(food => food.DishId == DishId);
            RetrievedDish.Name = val.Name;
            RetrievedDish.Chef = val.Chef;
            RetrievedDish.Calories = val.Calories;
            RetrievedDish.Tastiness = val.Tastiness;
            RetrievedDish.Description = val.Description;
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        
           [HttpGet("RenderEdit/{DishId}")]
        public IActionResult RenderEdit(int DishId, Dish val)
        {
            Dish RetrievedDish = dbContext.Dishes.FirstOrDefault(food => food.DishId == DishId);
            return View("Edit",RetrievedDish);
        }
        [HttpGet("Delete/{DishId}")]
        public IActionResult Delete(int DishId)
        {
            Dish RetrievedDish = dbContext.Dishes.SingleOrDefault(food => food.DishId == DishId);
            dbContext.Dishes.Remove(RetrievedDish);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }



    }
}
