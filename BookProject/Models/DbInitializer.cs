using System.Linq;
using System.Threading.Tasks;

namespace BookProject.Models
{
    public class DbInitializer
    {
        public static async Task InitializeAsync(AppDbContext appContext)
        {
            AppDbContext appDbContext = appContext;

            if (appDbContext.Books.Count() == 0)
            {
                Book[] books = new Book[]
                {
                    new Book { Title = "Madame Bovary", Author = "Gustave Flaubert"},
                    new Book { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald"},
                    new Book { Title = "Middlemarch", Author = "George Eliot"}
                };

                foreach (var book in books)
                {
                    await appDbContext.Books.AddAsync(book);
                }

                await appDbContext.SaveChangesAsync();
            }
        }
    }
}

