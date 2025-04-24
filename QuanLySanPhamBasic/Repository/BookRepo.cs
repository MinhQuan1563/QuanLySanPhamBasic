using Microsoft.EntityFrameworkCore;
using QuanLySanPhamBasic.Interface;
using QuanLySanPhamBasic.Models;

namespace QuanLySanPhamBasic.Repository
{
    public class BookRepo : IBookRepo
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BookRepo> _logger;

        public BookRepo(AppDbContext context, ILogger<BookRepo> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> CreateAsync(Book book)
        {
            try
            {
                await _context.Books.AddAsync(book);
                return await SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating item.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                return await SaveAsync();
            }
            else
            {
                _logger.LogWarning("Attempted to delete a non-existing Book with ID {Id}.", id);
                return false;
            }
        }

        public async Task<IEnumerable<Book>> GetAllAsync(string? search)
        {
            IQueryable<Book> query = _context.Books.Include(b => b.Genre).AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Title.Contains(search));
            }

            return await query.ToListAsync();
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            return await _context.Books.AsNoTrackingWithIdentityResolution()
                .FirstOrDefaultAsync(e => e.Id == id) ?? new Book();
        }

        public async Task<bool> UpdateAsync(int id, Book book)
        {
            var affectedRows = await _context.Books
                .Where(e => e.Id == id)
                .ExecuteUpdateAsync(e => e
                    .SetProperty(p => p.Title, book.Title));

            if (affectedRows == 0)
            {
                _logger.LogWarning("Attempted to update a non-existing Book with ID {Id}.", id);
                return false;
            }

            return affectedRows > 0;
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
