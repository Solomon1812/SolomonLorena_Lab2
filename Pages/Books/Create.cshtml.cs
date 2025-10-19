using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Solomon_Lorena_Lab2.Data;
using Solomon_Lorena_Lab2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public Book Book { get; set; } = new() { Title = string.Empty };

        public SelectList PublisherSelectList { get; set; } = default!;
        public SelectList AuthorSelectList { get; set; } = default!;

        public IActionResult OnGet()
        {
            PopulateSelectLists();
            //PublisherSelectList = new SelectList(_context.Publisher.OrderBy(p => p.PublisherName).ToList(), "ID", "PublisherName");
            //AuthorSelectList = new SelectList(_context.Author.OrderBy(p => p.FirstName).ToList(), "ID", "FirstName");
            //AuthorSelectList = new SelectList(_context.Author.OrderBy(p => p.LastName).ToList(), "ID", "LastName");

            return Page();
        }

        private void PopulateSelectLists()
        {
            var authors = _context.Set<Author>()
                .OrderBy(a => a.LastName).ThenBy(a => a.FirstName)
                .AsNoTracking()
                .ToList();

            AuthorSelectList = new SelectList(authors, "ID", "FullName");

            var publishers = _context.Set<Publisher>()
                .OrderBy(p => p.PublisherName)
                .AsNoTracking()
                .ToList();

            PublisherSelectList = new SelectList(publishers, "ID", "PublisherName");

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopulateSelectLists();
                //PublisherSelectList = new SelectList(_context.Publisher.OrderBy(p => p.PublisherName).ToList(), "ID", "PublisherName");
                //AuthorSelectList = new SelectList(_context.Author.OrderBy(p => p.FirstName).ToList(), "ID", "FirstName");
                //AuthorSelectList = new SelectList(_context.Author.OrderBy(p => p.LastName).ToList(), "ID", "LastName");

                return Page();
            }

            _context.Book.Add(Book);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
