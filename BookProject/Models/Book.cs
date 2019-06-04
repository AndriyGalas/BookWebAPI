namespace BookProject.Models
{
    public class Book
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Author { get; set; }

        public Book()
        {
            Title = string.Empty;
            Author = string.Empty;
        }

        public Book(string title, string author)
        {
            Title = title;
            Author = author;
        }
    }
}
