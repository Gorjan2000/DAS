using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using Domasna3.Models;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Domasna3.Controllers
{
    public class LokaciiController : Controller
    {
        private lokaciiEntities2 db = new lokaciiEntities2();
        static Order order = new Order();

        [Authorize]
        public ActionResult Index() //Choose city
        {
            List<string> cities = new List<String>();
            var podatociApiMapa = db.mapas.ToList(); //парсирање на податоците од апи од мапата, во листа
            foreach (var row in podatociApiMapa)
            {
                cities.Add(row.city);
            }
            List<string> uniqueListCities = cities.Distinct().ToList(); //додавање на уникатни градови (без повторување) во листа

            return View(uniqueListCities);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChooseLocal(string form)
        {
            var cityName = Request.Form["cityName"]; //земање на селектираниот град (од dropdown од претходната страна)
            order.City = cityName;
            List<string> locals = new List<string>(); //листа на локали
            var podatociApiMapa = db.mapas.ToList();
            foreach (var item in podatociApiMapa)
            {
                string city = item.city;
                if (city == null)
                {
                    city = "test";
                }
                if (city.Equals(cityName))
                {
                    locals.Add(item.name); //додавање на локали во листа, за соодветно избраниот град
                }
            }
            List<string> uniqueListLocals = locals.Distinct().ToList(); //додавање на уникатни локали (без повторување) во листа
            return View(uniqueListLocals);
        }


        [Authorize]
        public ActionResult ChooseFood()
        {
            var local = Request.Form["localName"]; //земање на селектираниот локал (од dropdown од претходната страна)
            order.Local = local;
            var foodList = new List<FoodModel> //мени на локалот
            {
                 new FoodModel{Id = 1,Name = "Хамбургер", Checked = false},
                 new FoodModel{Id = 2, Name = "Пица", Checked = false},
                 new FoodModel{Id = 3, Name = "Салата", Checked = false},
                 new FoodModel{Id = 4, Name = "Сок", Checked = false},
                 new FoodModel{Id = 5, Name = "Колач", Checked = false},

            };
            return View(foodList);
        }


        [HttpPost]
        [Authorize]
        public ActionResult Order(List<FoodModel> checkBoxList) //листа на селектирана храна од менито (chechboxes)
        {
            List<FoodModel> orderedFood = new List<FoodModel>(); //листа на порачана храна
            foreach(var item in checkBoxList)
            {
                if (item.Checked)
                {
                    if(item.Id == 1)
                    {
                        item.Name = "Хамбургер";
                    }else if(item.Id == 2)
                    {
                        item.Name = "Пица";
                    }else if(item.Id == 3)
                    {
                        item.Name = "Салата";
                    }else if(item.Id == 4)
                    {
                        item.Name = "Сок";
                    }
                    else
                    {
                        item.Name = "Колач";
                    }
                    item.UserName = HttpContext.User.Identity.Name; //username на најавениот корисник
                    orderedFood.Add(item);
                }
            }
            if(orderedFood.Count == 0)
            {
                // nema selektirano nisto
            }
            order.FoodOrdered = orderedFood; //додавање на нарачаната храна во база
            order.orderStatus = Status.ACCEPTED; //поставување на статус
            var username = HttpContext.User.Identity.Name;
            order.UserName = username; //username на најавениот корисник
            using (var db = new OrderContext())
            {
                db.Orders.Add(order); //додавање на нарачки во база
                db.SaveChanges(); 
            }
            return View();
        }
    }
}