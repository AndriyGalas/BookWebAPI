using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookProject.Models.Interfaces
{
    public interface IBookService
    {
        IEnumerable<Book> GetAllBooks();
        Book GetById(int id);
        Task Add(Book item);
        Task Remove(int id);
        Task Update(int id, Book newBook);
    }
}
