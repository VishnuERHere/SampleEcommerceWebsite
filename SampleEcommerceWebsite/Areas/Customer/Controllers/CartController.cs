using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleEcommerceWebsite.DataAccess.Repository.IRepository;
using SampleEcommerceWebsite.Models;
using SampleEcommerceWebsite.Models.ViewModels;
using SampleEcommerceWebsite.Utility;
using System.Security.Claims;

namespace SampleEcommerceWebsite.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartViewModel ShoppingCartViewModel { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartViewModel = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
                OrderHeader= new()

            };

            //Calculating total cart value
            foreach(var cart in  ShoppingCartViewModel.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartViewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartViewModel);
        }


        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartViewModel = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
                OrderHeader = new()
            };

            ShoppingCartViewModel.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            ShoppingCartViewModel.OrderHeader.Name = ShoppingCartViewModel.OrderHeader.ApplicationUser.Name;
            ShoppingCartViewModel.OrderHeader.PhoneNumber = ShoppingCartViewModel.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartViewModel.OrderHeader.StreetAddress = ShoppingCartViewModel.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartViewModel.OrderHeader.City = ShoppingCartViewModel.OrderHeader.ApplicationUser.City;
            ShoppingCartViewModel.OrderHeader.State = ShoppingCartViewModel.OrderHeader.ApplicationUser.State;
            ShoppingCartViewModel.OrderHeader.PostalCode = ShoppingCartViewModel.OrderHeader.ApplicationUser.PostalCode;

            //Calculating total cart value
            foreach (var cart in ShoppingCartViewModel.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartViewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartViewModel);
        }
        [HttpPost]
        [ActionName("Summary")]
		public IActionResult SummaryPOST()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartViewModel.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product");

            ShoppingCartViewModel.OrderHeader.OrderDate = System.DateTime.Now;
			ShoppingCartViewModel.OrderHeader.ApplicationUserId = userId;

			ApplicationUser applicationUser= _unitOfWork.ApplicationUser.Get(u => u.Id == userId);


			//Calculating total cart value
			foreach (var cart in ShoppingCartViewModel.ShoppingCartList)
			{
				cart.Price = GetPriceBasedOnQuantity(cart);
				ShoppingCartViewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}

            if(applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                //regular customer - payment needs to be done immediately
                ShoppingCartViewModel.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusPending;
				ShoppingCartViewModel.OrderHeader.OrderStatus = StaticDetails.StatusPending;
			}
            else
            {
				//Company customer - payment can be done in next 30 days
				ShoppingCartViewModel.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusDelayedPayment;
				ShoppingCartViewModel.OrderHeader.OrderStatus = StaticDetails.StatusApproved;

			}

            _unitOfWork.OrderHeader.Add(ShoppingCartViewModel.OrderHeader);
			_unitOfWork.Save();

            foreach(var cart in ShoppingCartViewModel.ShoppingCartList)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = ShoppingCartViewModel.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
  
            }

			if (applicationUser.CompanyId.GetValueOrDefault() == 0)
			{
				//regular customer - payment logic goes here

			}

			return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartViewModel.OrderHeader.Id });
		}


        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }

		public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            cartFromDb.Count += 1; 
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {

            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (cartFromDb.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Remove(int cartId)
        {

            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

            _unitOfWork.ShoppingCart.Remove(cartFromDb);
        
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }


        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
			if (shoppingCart.Count <= 50)
			{
				return shoppingCart.Product.Price;
			}
			else
			{
				if (shoppingCart.Count <= 100)
				{
					return shoppingCart.Product.Price50;
				}
				else
				{
					return shoppingCart.Product.Price100;
				}
			}
            
        }
    }
}
