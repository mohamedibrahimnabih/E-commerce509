﻿namespace E_commerce.Models.ViewModels
{
    public class CartWithTotalPriceVM
    {
        public List<Cart> Carts { get; set; }
        public double TotalPrice { get; set; }
    }
}
