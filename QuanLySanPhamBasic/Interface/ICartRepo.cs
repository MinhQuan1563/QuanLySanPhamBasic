using QuanLySanPhamBasic.Models;

namespace QuanLySanPhamBasic.Interface
{
    public interface ICartRepo
    {
        void CreateCart(string userId);
        void DeleteCartDetail(int cartId, int bookId);
        void DeleteCart(int cartId);
        void AddBookToCart(string userId, int bookId, int quantity);
        Cart GetActiveCartForUser(string userId);
        CartDetail GetCartDetailById(int cartId, int bookId);
        Cart GetCartById(int cartId);
    }
}
