using FoodManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace FoodManagement.Controllers
{
    public class Item
    {
        private FOOD_TYPE _type = new FOOD_TYPE();
        private int _quantity;

        public Item() { }

        public Item(FOOD_TYPE _type, int _quantity)
        {
            this.Type = _type;
            this.Quantity = _quantity;
        }

        public FOOD_TYPE Type { get => _type; set => _type = value; }
        public int Quantity { get => _quantity; set => _quantity = value; }
    }
}