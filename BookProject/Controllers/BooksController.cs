using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookProject.Models;
using BookProject.Models.Interfaces;
using BookProject.Models.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookProject.Controllers
{
    [Route("api/books")]
    public class BooksController : Controller
    {
        private readonly IBookService bookService;

        public BooksController(AppDbContext appDbContext)
        {
            bookService = new BookService(appDbContext);
        }

        [HttpGet]
        public IEnumerable<Book> GetBooks()
        {
            return bookService.GetAllBooks();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = bookService.GetById(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<Book>> PostBook([FromBody]Book item)
        {
            await bookService.Add(item);

            return CreatedAtAction(nameof(GetBook), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, [FromBody]Book item)
        {
            await bookService.Update(id, item);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            await bookService.Remove(id);

            return NoContent();
        }
    }
}
