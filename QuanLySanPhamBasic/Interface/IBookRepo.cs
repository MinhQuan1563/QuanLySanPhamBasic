using QuanLySanPhamBasic.Models;

namespace QuanLySanPhamBasic.Interface
{
    public interface IBookRepo
    {
        Task<IEnumerable<Book>> GetAllAsync(string? search);
        Task<Book> GetByIdAsync(int id);
        Task<bool> CreateAsync(Book book);
        Task<bool> UpdateAsync(int id, Book book);
        Task<bool> DeleteAsync(int id);
        Task<bool> SaveAsync();
    }
}
