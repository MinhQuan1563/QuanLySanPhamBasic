using QuanLySanPhamBasic.Models;
using QuanLySanPhamBasic.Interface;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace QuanLySanPhamBasic.Repositories
{
	public class CartRepo : ICartRepo
	{
		private readonly AppDbContext _context;

		public CartRepo(AppDbContext context)
        {
			_context = context;
		}
		public Cart GetActiveCartForUser(string userId)
		{
			var res = _context.Carts
                    .Include(c => c.CartDetails)
						.ThenInclude(cd => cd.Book)
					.FirstOrDefault(c => c.UserId == userId && c.Status == "Pending");

            return res;
		}

		public void CreateCart(string userId)
		{
			var newCart = new Cart
			{
				UserId = userId,
				Status = "Pending",
				TotalPrice = 0,
				CreatedDate = DateTime.Now,
			};

			_context.Carts.Add(newCart);
			_context.SaveChanges();
		}

		public void AddBookToCart(string userId, int bookId, int quantity)
		{
			var cart = GetActiveCartForUser(userId);
			if (cart == null)
			{
				CreateCart(userId);
				cart = GetActiveCartForUser(userId);
			}

			var book = _context.Books.Find(bookId);
			var cartDetail = cart.CartDetails!.FirstOrDefault(cd => cd.BookId == bookId);

			if (cartDetail == null)
			{
				cartDetail = new CartDetail
				{
					CartId = cart.Id,
					BookId = bookId,
					Price = book!.Price,
					Quantity = quantity
				};
				_context.CartDetails.Add(cartDetail);
			}
			else
			{
				cartDetail.Quantity += quantity;
			}

			cart.TotalPrice += book!.Price * quantity;
			_context.SaveChanges();
		}

		public void DeleteCartDetail(int cartId, int bookId)
		{
			var cartDetail = _context.CartDetails.FirstOrDefault(x => x.CartId == cartId && x.BookId == bookId);

            if (cartDetail != null)
            {
                _context.CartDetails.Remove(cartDetail);

                _context.SaveChanges();
            }
        }

        public void DeleteCart(int cartId)
        {
			var cart = _context.Carts.FirstOrDefault(x => x.Id == cartId);
            if (cart != null)
            {
                _context.Carts.Remove(cart);

                _context.SaveChanges();
            }
        }

        public CartDetail GetCartDetailById(int cartId, int bookId)
        {
            return _context.CartDetails.FirstOrDefault(x => x.CartId == cartId && x.BookId == bookId) ?? new CartDetail();
        }

        public Cart GetCartById(int cartId)
        {
            return _context.Carts.FirstOrDefault(x => x.Id == cartId) ?? new Cart();
        }
    }
}
