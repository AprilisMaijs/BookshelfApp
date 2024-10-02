using Microsoft.AspNetCore.Identity;

namespace BookshelfApp.Models
{
    public class Book
    {
        public int Id { get; set; }

        // Existing properties
        public string Title { get; set; }
        public List<String> Authors { get; set; } // Consider changing to a List<string> if you want to handle multiple authors
        public string ISBN { get; set; }
        public string CoverUrl { get; set; }
        public string Description { get; set; }

        // New properties
        public string PublishedDate { get; set; }
        // Add other properties as needed

        // Relationships
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }

    public class ApplicationUser : IdentityUser
    {
        public ICollection<Book> Books { get; set; }
    }
}
