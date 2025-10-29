using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Solomon_Lorena_Lab2.Data;
using Solomon_Lorena_Lab2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Solomon_Lorena_Lab2.Pages.Books
{
    public class IndexModel : PageModel
    {
        private readonly Solomon_Lorena_Lab2.Data.Solomon_Lorena_Lab2Context _context;

        public IndexModel(Solomon_Lorena_Lab2.Data.Solomon_Lorena_Lab2Context context)
        {
            _context = context;
        }

        public IList<Book> Book { get;set; } = default!;
        public BookData BookD { get; set; }
        public int BookID { get; set; } 
        public int CategoryID { get; set; }


        //functia get pentru Index !
        public async Task OnGetAsync(int? id, int? categoryID) 
        {
            BookD = new BookData();

            Book = await _context.Book
            .Include(b => b.Publisher) // Include the related Publisher data
            .Include(b => b.Author)    // Include the related Author data
            .Include(b => b.BookCategories)
            .ThenInclude(b => b.Category)
            .AsNoTracking()
            .OrderBy(b=> b.Title)
            .ToListAsync();

            if (id != null)
            {
                BookID = id.Value;
                Book book = BookD.Books
                    .Where(i => i.Id == id.Value).Single();
                BookD.Categories = book.BookCategories.Select(s => s.Category);
            }
        }

        
    }
}
