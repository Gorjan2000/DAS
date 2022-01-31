using Domasna3.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Domasna3.Controllers
{
    public class OrderHistoryController : Controller
    {
       
        [Authorize]
        public ActionResult Index()
        {
            List<Order> orders = new List<Order>();
            List<Order> filteredOrders = new List<Order>();
            
            using (var db = new OrderContext())
            {
                 orders = db.Orders.ToList(); //сместување на нарачките во листа
            }

            foreach (var order in orders)
            {
                if (order.UserName.Equals(HttpContext.User.Identity.Name))
                {
                    FoodModel food = new FoodModel();
                    List<FoodModel> filteredFood = new List<FoodModel>();
                    string orderID = order.ID.ToString();
                    using (SqlConnection connection = new SqlConnection("Server=tcp:domasna3dbserver.database.windows.net,1433;Initial Catalog=Domasna3_db;Persist Security Info=False;User ID=dizajntim;Password=Dizajnt7.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
                    {
                        connection.Open();
                        string query = "SELECT * FROM FoodModels WHERE Order_ID in (" + orderID + ")";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    FoodModel currentFood = new FoodModel
                                    {
                                        Id = reader.GetInt32(0),
                                        Name = reader.GetString(1),
                                        Checked = reader.GetBoolean(2),
                                        UserName = reader.GetString(3)
                                    };
                                    order.FoodOrdered.Add(currentFood);
                                }
                            }
                        }
                    }
                    filteredOrders.Add(order);
                }
            }
            return View(filteredOrders);
        }


        public ActionResult ChangeStatusToDelivered(string order)
        {
            var tempID = (string)(RouteData.Values["id"]);
            int orderID = Int32.Parse(tempID);
            using (var db = new OrderContext())
            {
               var tempOrder = db.Orders.SingleOrDefault(e => e.ID == orderID);
               tempOrder.orderStatus = Status.DELIVERED;
               db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        public ActionResult ChangeStatusToCanceled(Order order)
        {
            var tempID = (string)(RouteData.Values["id"]);
            int orderID = Int32.Parse(tempID);
            using (var db = new OrderContext())
            {
                var tempOrder = db.Orders.SingleOrDefault(e => e.ID == orderID);
                tempOrder.orderStatus = Status.CANCELED;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}