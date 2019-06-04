using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookProject.Models.Interfaces;

namespace BookProject.Models.Services
{
    public class BookService : IBookService
    {
        private readonly AppDbContext appDbContext;

        public BookService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }


        public IEnumerable<Book> GetAllBooks()
        {
            return appDbContext.Books.ToList();
        }

        public Book GetById(int id)
        {
            return appDbContext.Books.Find(id);
        }

        public async Task Add(Book item)
        {
            appDbContext.Books.Add(item);
            await appDbContext.SaveChangesAsync();
        }

        public async Task Remove(int id)
        {
            var existing = appDbContext.Books.Find(id);
            if (existing != null)
            {
                appDbContext.Books.Remove(existing);
            }

            await appDbContext.SaveChangesAsync();
        }

        public async Task Update(int id, Book item)
        {
            var book = appDbContext.Books.Find(id);

            book.Title = item.Title;
            book.Author = item.Author;

            appDbContext.Books.Update(item);
            await appDbContext.SaveChangesAsync();

        }
    }
}
