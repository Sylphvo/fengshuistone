//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using Labixa.Models;
//using Outsourcing.Service;
//using Outsourcing.Data.Models;
//using PagedList;
//using Labixa.ViewModels;
//using Labixa.Helpers;
//using Outsourcing.Core.Common;
//using System.Runtime.Remoting.Messaging;

//namespace Labixa.Controllers
//{

//    public class ProductsController : Controller
//    {
//        private readonly IProductService _productService;
//        private readonly IProductCategoryService _productcategoryService;
//        private readonly IProductAttributeMappingService _productAttributeMappingService;
//        private readonly IProductRelationshipService _productRelationshipService;
//        private readonly IOrderService _OrderService;
//        private readonly IOrderItemService _OrderItemService;


//        public ProductsController(IProductService productService, 
//            IProductAttributeMappingService productAttributeMappingService,
//            IProductRelationshipService productRelationshipService, IProductCategoryService productcategoryService, IOrderService orderService, IOrderItemService orderItemService)
//        {
//            this._productService = productService;
//            this._productAttributeMappingService = productAttributeMappingService;
//            this._productRelationshipService = productRelationshipService;
//            this._productcategoryService = productcategoryService;
//            this._OrderService = orderService;
//            this._OrderItemService = orderItemService;
//        }
//        public ActionResult Products(int? page)
//        {
//            int pageNumb = (page ?? 1);
//            int pageSize = 15;
//            var list = _productService.GetAllProducts().ToPagedList(pageNumb, pageSize);
//            return View(list);
//        }
      
//        public ActionResult ProductDetail(string Slug)
//        {
//            var model = _productService.GetProductBySlug(Slug);
//            ViewBag.Title = model.Name;
//            ViewBag.Description = model.Description;
//            ViewBag.Image = "http://tueduchealthy.vn"+model.ProductPictureMappings.FirstOrDefault().Picture.Url;
//            ViewBag.Url = model.Slug;
//            return View(model);
//        }

//        public ActionResult ProductFeatured()
//        {

//            var model = _productService.GetPromotionProducts().ToList();
//            return PartialView("_productHome", model);
//        }
//        #region code add to cart by Nghia 07/23/2020
//        [HttpPost]
//        public ActionResult AddToCart(int ProductId, int Quantity)
//        {
//            if(Quantity == 0)
//            {
//                Quantity = 1;
//            }
//            /*else
//            {
//                Quantity += 1;
//            }*/
//            ListProductCartModel listCart = new ListProductCartModel();
//            if (Session["Cart"] != null)
//            {
//                listCart = (ListProductCartModel)Session["Cart"];

//                //kiểm tra product đã có trong cart chưa, nếu có thì cập nhật số lượng
//                if (listCart.listProduct.Where(p => p.ProductId == ProductId).FirstOrDefault() != null)
//                {
//                    foreach (var item in listCart.listProduct)
//                    {
//                        if (item.ProductId == ProductId)
//                        {
//                            item.Quantity += Quantity;
//                        }
//                    }
//                    var modelCart = OrderCalculator.CalculatorCart(listCart);
//                    Session["Cart"] = modelCart;
//                }
//                else
//                {
//                    //nếu không có trong cart, thì thêm product vào cart
//                    CartModel cart = new CartModel();
//                    var product = _productService.GetProductById(ProductId);
//                    cart.ProdName = product.Name;
//                    cart.ProdNameEngh = product.NameEng;
//                    cart.DescriptionEng = product.DescEng;
//                    cart.Description = product.Description;
//                    cart.slug = product.Slug;
//                    cart.ProductImage = product.ProductPictureMappings.FirstOrDefault().Picture.Url;
//                    cart.ProductId = product.Id;
//                    if (product.IsHomePage)
//                    {
//                        cart.Price = product.Price;
//                        //KhaiEdit
//                        var engprice = Math.Round(((product.Price) / 23000), 3);
//                        cart.PriceEng = engprice;
//                    }
//                    else
//                    {
//                        cart.Price = product.Price;
//                        //Khai Edit
//                        var engprice = Math.Round(((product.Price) / 23000), 3);
//                        cart.PriceEng = engprice;
//                    }
//                    cart.Quantity = Quantity;
//                    listCart.listProduct.Add(cart);
//                    var modelCart = OrderCalculator.CalculatorCart(listCart);
//                    Session["Cart"] = modelCart;
//                }
//            }
//            else
//            {
//                //tạo mới cart đưa vào session
//                CartModel cart = new CartModel();
//                var product = _productService.GetProductById(ProductId);
//                cart.ProdName = product.Name;
//                cart.ProdNameEngh = product.NameEng;
//                cart.DescriptionEng = product.DescEng;
//                cart.Description = product.Description;
//                cart.slug = product.Slug;
//                cart.ProductImage = product.ProductPictureMappings.FirstOrDefault().Picture.Url;
//                cart.ProductId = product.Id;
//                if (product.IsHomePage)
//                {
//                    cart.Price = product.Price;
//                    //Khai Edit
//                    var engprice = Math.Round(((product.Price) / 23000), 3);
//                    cart.PriceEng = engprice;
//                }
//                else
//                {
//                    cart.Price = product.Price;
//                    //Khai Edit
//                    var engprice = Math.Round(((product.Price) / 23000), 3);
//                    cart.PriceEng = engprice;
//                }
//                cart.Quantity = Quantity;
//                listCart.listProduct.Add(cart);
//                var modelCart = OrderCalculator.CalculatorCart(listCart);
//                Session["Cart"] = modelCart;
//            }

//            return null;
//        }
//        public ActionResult DetailCart()
//        {
//            return View();
//        }
//        public ActionResult DeleteCartProduct(int prodId)
//        {
//            ListProductCartModel listCart = new ListProductCartModel();
//            listCart = (ListProductCartModel)Session["Cart"];
//            var st = listCart.listProduct.Find(c => c.ProductId == prodId);
//            listCart.listProduct.Remove(st);
//            var modelCart = OrderCalculator.CalculatorCart(listCart);
//            Session["Cart"] = modelCart;
//            return RedirectToAction("DetailCart", "Products");
//        }
//        public ActionResult CartPays()
//        {
//            return View();
//        }
//        [HttpPost]
//        public ActionResult CartPays(string nameCus, string phoneCus, string addressCus, string noteCus)
//        {
//            Order order = new Order();
//            order.CustomerName = nameCus;
//            order.CustomerPhone = phoneCus;
//            order.CustomerAddress = addressCus;
//            //order.CustomerEmail = CustomerEmail;
//            order.Note = noteCus;

//            ListProductCartModel listCart = new ListProductCartModel();
//            listCart = (ListProductCartModel)Session["Cart"];
//            #region input data vào session
//            listCart.CustomePhone = phoneCus;
//            listCart.CustomerAddress = addressCus;
//            listCart.CustomerName = nameCus;
//            listCart.CustomerNote = noteCus;
//            //listCart.CustomerEmail = CustomerEmail;
//            #endregion
//            if (listCart != null)
//            {
//                order.OrderTotal = listCart.TotalPayment;
//                if (listCart.GiftCode != "")
//                {
//                    order.TotalPayment = listCart.TotalPayment;
//                }
//                else
//                {
//                    order.TotalPayment = listCart.TotalPayment ;
//                }
//                order.Status = 1;
//                order.Discount = listCart.GiftCodePercent;
//                order.Temp_1 = listCart.GiftCode;

//                order.Deleted = false;
//                order.DateCreated = DateTime.Now;
//                listCart.TotalPayment = order.TotalPayment;
//                order.Note = noteCus;
//                //add Order to DB
//                _OrderService.CreateOrder(order);

//                #region set orderItem
//                foreach (var item in listCart.listProduct)
//                {
//                    OrderItem Oitem = new OrderItem();
//                    Oitem.Discount = 0;
//                    Oitem.OrderId = order.Id;
//                    Oitem.Price = item.Price;
//                    Oitem.ProductId = item.ProductId;
//                    Oitem.Quantity = item.Quantity;
                    

//                    //Oitem.Temp_1 = "";
//                    //Oitem.Temp_2 = "";
//                    //Oitem.Temp_3 = "";
//                    //add to order Item
//                    _OrderItemService.CreateOrderItem(Oitem);
//                }
//                #endregion
//                Session["Cart"] = listCart;
//                return RedirectToAction("ConfirmCartPays", "Products");
//            }
//            return RedirectToAction("Index", "Home");
//        }
//        public ActionResult ConfirmCartPays()
//        {
//            ListProductCartModel listCart = new ListProductCartModel();
//            listCart = (ListProductCartModel)Session["Cart"];
//            Session["Cart"] = null;
//            HttpCookie caibanh = Request.Cookies["_culture"];
//            if (caibanh.Value == "vi")
//            {
//                ViewBag.SuccessMsg = "Xin cám ơn, mọi thông tin đã được lưu trữ";
//            }
//            else {
//                ViewBag.SuccessMsg = "Thank you, all information has been stored";
//            }
//            ViewBag.Keyword = listCart.listProduct.FirstOrDefault().ProdName;
//            return View(listCart);
//        }
//        public ActionResult DeleteCart()
//        {
//            Session["Cart"] = null;
//            return RedirectToAction("Index", "Home");
//        }
//        #endregion
//    }
//}

