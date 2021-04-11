using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Labixa.Models;
using Outsourcing.Service;
using Outsourcing.Data.Models;
using PagedList;
using Labixa.ViewModels;
using Labixa.Helpers;
using Outsourcing.Core.Common;

namespace Labixa.Controllers
{
    public class CartController : BaseHomeController
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productcategoryService;
        private readonly IBlogService _blogService;
        private readonly IBlogCategoryService _blogCategoryService;
        private readonly IWebsiteAttributeService _websiteAttributeService;
        private readonly IStaffService _staffService;
        private readonly IProductAttributeMappingService _productAttributeMappingService;
        private readonly IProductRelationshipService _productRelationshipService;
        private readonly IOrderService _OrderService;
        private readonly IOrderItemService _OrderItemService;



        public CartController(IProductService productService, IBlogService blogService,
            IWebsiteAttributeService websiteAttributeService, IBlogCategoryService blogCategoryService,
            IStaffService staffService, IProductAttributeMappingService productAttributeMappingService,
            IProductRelationshipService productRelationshipService, IProductCategoryService productcategoryService,
            IOrderService OrderService, IOrderItemService OrderItemService)
        {
            this._productService = productService;
            this._blogService = blogService;
            this._websiteAttributeService = websiteAttributeService;
            this._blogCategoryService = blogCategoryService;
            this._staffService = staffService;
            this._productAttributeMappingService = productAttributeMappingService;
            this._productRelationshipService = productRelationshipService;
            this._productcategoryService = productcategoryService;
            this._OrderItemService = OrderItemService;
            this._OrderService = OrderService;
        }
        //
        // GET: /Cart/
        public ActionResult Index()
        {
            ListProductCartModel listCart = new ListProductCartModel();
            listCart = (ListProductCartModel)Session["Cart"];

            return View();
        }
        public ActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Checkout(string CustomerName, string CustomePhone, string CustomerAddress, string CustomerNote)
        {

            Order order = new Order();
            order.CustomerName = CustomerName;
            order.CustomerPhone = CustomePhone;
            order.CustomerAddress = CustomerAddress;
            order.Note = CustomerNote;

            ListProductCartModel listCart = new ListProductCartModel();
            listCart = (ListProductCartModel)Session["Cart"];
            #region input data vào session
            listCart.CustomePhone = CustomePhone;
            listCart.CustomerAddress = CustomerAddress;
            listCart.CustomerName = CustomerName;
            listCart.CustomerNote = CustomerNote;
            #endregion
            if (listCart != null)
            {
                order.OrderTotal = listCart.TotalPayment;
                if (listCart.GiftCode != "")
                {
                    order.TotalPayment = listCart.TotalAfterDiscount;
                }
                else
                {
                    order.TotalPayment =  listCart.TotalPayment;
                }
                order.Status = 1;
               //order.Discount = listCart.GiftCodePercent;
                order.Temp_1 = listCart.GiftCode;

                order.Deleted = false;
                order.DateCreated = DateTime.Now;

                //add Order to DB
                _OrderService.CreateOrder(order);

                #region set orderItem
                foreach (var item in listCart.listProduct)
                {
                    OrderItem Oitem = new OrderItem();
                    Oitem.Discount = 0;
                    Oitem.OrderId = order.Id;
                    Oitem.Price = item.Price;
                    Oitem.ProductId = item.ProductId;
                    Oitem.Quantity = item.Quantity;
                    
                  
                    //Oitem.Temp_1 = "";
                    //Oitem.Temp_2 = "";
                    //Oitem.Temp_3 = "";
                    //add to order Item
                    _OrderItemService.CreateOrderItem(Oitem);
                }
                #endregion
                Session["Cart"] = listCart;
                return RedirectToAction("ConfirmCheckOut", "Cart");
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ConfirmCheckOut()
        {
            ListProductCartModel listCart = new ListProductCartModel();
            listCart = (ListProductCartModel)Session["Cart"];
            Session["Cart"] = null;
            return View(listCart);
        }
    }
}