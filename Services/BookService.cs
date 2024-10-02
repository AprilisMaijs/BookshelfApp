namespace BookshelfApp.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using BookshelfApp.Models;
    using Newtonsoft.Json;
    using BookshelfApp.ExternalModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Security.Claims;

    public class BookService
    {
        private readonly HttpClient _client;

        public BookService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
        }

        public async Task<Book> GetBookByISBNAsync(string isbn)
        {
            var url = $"https://www.googleapis.com/books/v1/volumes?q=isbn:{isbn}";

            try
            {
                var response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var googleBookResult = JsonConvert.DeserializeObject<GoogleBooksApiResponse>(data);

                    if (googleBookResult != null && googleBookResult.TotalItems > 0)
                    {
                        var volumeInfo = googleBookResult.Items[0].VolumeInfo;

                        var book = new Book
                        {
                            Title = volumeInfo.Title,
                            Authors = volumeInfo.Authors,
                            ISBN = isbn,
                            CoverUrl = volumeInfo.ImageLinks?.Thumbnail,
                            Description = volumeInfo.Description,
                            PublishedDate = volumeInfo.PublishedDate,
                            // Add other fields as needed
                        };

                        return book;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., logging)
                throw new Exception("An error occurred while fetching book data.", ex);
            }

            return null;
        }

    }
}
