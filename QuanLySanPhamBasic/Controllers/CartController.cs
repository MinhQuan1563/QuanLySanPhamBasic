using Microsoft.AspNetCore.Mvc;
using QuanLySanPhamBasic.Interface;
using System.Security.Claims;

namespace QuanLySanPhamBasic.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepo _cartRepo;

        public CartController(ICartRepo cartRepo)
        {
            _cartRepo = cartRepo;
        }
        public IActionResult Index()
        {
            string userId = GetLoggedInUserId();
            var cart = _cartRepo.GetActiveCartForUser(userId);

            if (cart == null)
            {
                TempData["Message"] = "Giỏ hàng của bạn hiện tại đang TRỐNG";
            }

            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int bookId, int quantity)
        {
            string userId = GetLoggedInUserId();
            _cartRepo.AddBookToCart(userId, bookId, quantity);

            return RedirectToAction("Index", "Cart");
        }

        private string GetLoggedInUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpGet]
        public IActionResult Delete(int cartId, int bookId)
        {
            var cartDetail = _cartRepo.GetCartDetailById(cartId, bookId);
            TempData["Message"] = "Bạn có chắc muốn xóa sản phẩm này không?";
            return View(cartDetail);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int cartId, int bookId)
        {
            _cartRepo.DeleteCartDetail(cartId, bookId);
            return RedirectToAction("Index", "Cart");
        }

        [HttpGet]
        public IActionResult DeleteAllCart(int cartId)
        {
            var cart = _cartRepo.GetCartById(cartId);
            TempData["Message"] = "Bạn có chắc muốn xóa TẤT CẢ sản phẩm này không?";
            return View(cart);
        }

        [HttpPost]
        public IActionResult DeleteAllCartConfirmed(int cartId)
        {
            _cartRepo.DeleteCart(cartId);
            return RedirectToAction("Index", "Cart");
        }
    }
}
