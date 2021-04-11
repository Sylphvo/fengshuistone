using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Labixa.ViewModels
{
    public class CartModel
    {
        public int ProductId { get; set; }
        public string slug { get; set; }
        public string ProductImage { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string ProdName { get; set; }
        public double Price { get; set; }
    }
    public class ListProductCartModel
    {
        public ListProductCartModel()
        {
            listProduct = new List<CartModel>();
            TotalPayment = 0;
            TotalAfterDiscount = 0;
        }
        public IEnumerable<CartModel> listProduct;
        public string CustomerName { get; set; }
        public string CustomePhone { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerNote { get; set; }
        public double TotalPayment { get; set; }
        public string GiftCode { get; set; }
        public double TotalAfterDiscount { get; set; }
    }
}