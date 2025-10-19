using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Solomon_Lorena_Lab2.Data;
using Solomon_Lorena_Lab2.Models;

namespace Solomon_Lorena_Lab2.Pages.Books
{
    public class CreateModel : PageModel
    {
        private readonly Solomon_Lorena_Lab2.Data.Solomon_Lorena_Lab2Context _context;

        public CreateModel(Solomon_Lorena_Lab2.Data.Solomon_Lorena_Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; } = new();

        public SelectList PublisherSelectList { get; set; } = default!;

        public IActionResult OnGet()
        {
            PublisherSelectList = new SelectList(_context.Publisher.OrderBy(p => p.PublisherName).ToList(), "ID", "PublisherName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PublisherSelectList = new SelectList(_context.Publisher.OrderBy(p => p.PublisherName).ToList(), "ID", "PublisherName");
                return Page();
            }

            _context.Book.Add(Book);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
