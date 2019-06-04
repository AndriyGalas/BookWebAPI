using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookProject.Controllers;
using BookProject.Models;
using BookProject.Models.Interfaces;
using BookProject.Models.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace NUnitTestBookProject.Controllers
{
    class BookServiceTest
    {
        private DbContextOptions<AppDbContext> options;
        private AppDbContext context;
        private IBookService bookService;

        [SetUp]
        public void SetUp()
        {
            options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "DbForTesting").Options;
            context = new AppDbContext(options);
            bookService = new BookService(context);
        }

        [TearDown]
        public void TearDown()
        {
            context.Books.RemoveRange(context.Books);
            context.SaveChangesAsync();
        }

        [Test]
        public void GetAllBooks_ReturnsValidTypeTest()
        {
            //Act
            var actualResult = bookService.GetAllBooks();

            //Assert
            Assert.IsAssignableFrom<List<Book>>(actualResult);
        }

        [Test]
        public async Task GetAllBooks_ReturnsExpectedDataTest()
        {
            //Arrange
            var books = new List<Book>
            {
                new Book {Title = "TestTitle1", Author = "TestAuthor1"},
                new Book {Title = "TestTitle2", Author = "TestAuthor2"},
                new Book {Title = "TestTitle3", Author = "TestAuthor3"}
            };

            foreach (var book in books)
            {
                context.Books.Add(book);
            }
            await context.SaveChangesAsync();

            //Act
            var expectedResult = books.Count;
            var actualResult = bookService.GetAllBooks().Count();

            //Assert
            Assert.AreEqual(expectedResult, actualResult);

        }

        [Test]
        [TestCase(2)]
        public void GetById_UnknownId_ReturnsNull(int id)
        {
            //Act
            var actualResult = bookService.GetById(id);

            //Assert
            Assert.AreEqual(null, actualResult);

        }

        [Test]
        [TestCase(55)]
        public async Task GetById_ExistingId_ReturnsRightTypeItem(int id)
        {
            //Arrange
            var book = new Book
            {
                Id = id,
                Title = "TestTitle1",
                Author = "TestAuthor1"
            };

            context.Books.Add(book);
            await context.SaveChangesAsync();

            //Act
            var actualResult = bookService.GetById(id);

            //Assert
            Assert.IsAssignableFrom<Book>(actualResult);

        }

        [Test]
        [TestCase(40)]
        public async Task GetById_ExistingId_ReturnsRightItem(int id)
        {
            //Arrange
            var books = new List<Book>
            {
                new Book {Id = id-1, Title = "TestTitle1", Author = "TestAuthor1"},
                new Book {Id = id, Title = "TestTitle2", Author = "TestAuthor2"},
                new Book {Id = id+1, Title = "TestTitle3", Author = "TestAuthor3"}
            };

            context.Books.AddRange(books);
            await context.SaveChangesAsync();

            //Act
            var expectedResult = books[1];
            var actualResult = bookService.GetById(id);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);

        }

        [Test]
        public async Task Add_IncreasesNumberOfBooks()
        {
            //Arrange
            var book = new Book
            {
                Title = "TestTitle1",
                Author = "TestAuthor1"
            };

            context.Books.Add(book);
            await context.SaveChangesAsync();

            //Act
            var expectedResult = 1;
            var actualResult = bookService.GetAllBooks().Count();

            //Assert
            Assert.AreEqual(expectedResult, actualResult);

        }

        [Test]
        [TestCase(66)]
        public async Task Add_AddsRightItem(int id)
        {
            //Arrange
            var book = new Book
            {
                Id = id,
                Title = "TestTitle1",
                Author = "TestAuthor1"
            };

            context.Books.Add(book);
            await context.SaveChangesAsync();

            //Act
            var expectedResult = book;
            var actualResult = bookService.GetById(id);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);

        }

        [Test]
        [TestCase(66)]
        public async Task Remove_DeletesRightItem(int id)
        {
            //Arrange
            var book = new Book
            {
                Id = id,
                Title = "TestTitle1",
                Author = "TestAuthor1"
            };

            context.Books.Add(book);
            await context.SaveChangesAsync();

            //Act
            await bookService.Remove(id);
            var actualResult = bookService.GetById(id);

            //Assert
            Assert.IsNull(actualResult);

        }

        [Test]
        public void Update_ChangesRightItem()
        {
            //Arrange
            var book = new Book
            {
                Title = "TestTitle1",
                Author = "TestAuthor1"
            };

            var expectedBook = new Book
            {
                Title = "Updated1",
                Author = "Updated1"
            };

            context.Books.Add(book);
            context.SaveChangesAsync();

            //Act
            var id = context.Books.First().Id;
            bookService.Update(id, expectedBook);
            var actualResult = bookService.GetById(id);

            //Assert
            Assert.AreEqual(expectedBook.Title, actualResult.Title);
            Assert.AreEqual(expectedBook.Author, actualResult.Author);
        }
    }
}
