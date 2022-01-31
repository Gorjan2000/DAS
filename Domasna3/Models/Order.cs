using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Domasna3.Models
{
    public class OrderContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<FoodModel> FoodModels { get; set; }
    }

    public enum Status
    {
        ACCEPTED,
        DELIVERED,
        CANCELED
    }
    public class Order
    {
        public int ID { get; set; }
        public string City { get; set; }
        public string Local { get; set; }
        public string UserName { get; set; }
        public List<FoodModel> FoodOrdered { get; set; }
        public Status orderStatus { get; set; }
        public Order()
        {
            FoodOrdered = new List<FoodModel>();
        }

    }

  
}