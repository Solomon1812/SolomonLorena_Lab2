using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Solomon_Lorena_Lab2.Data;
using Solomon_Lorena_Lab2.Models;

namespace Solomon_Lorena_Lab2.Pages.Books
{
    public class EditModel : PageModel
    {
        private readonly Solomon_Lorena_Lab2.Data.Solomon_Lorena_Lab2Context _context;

        public EditModel(Solomon_Lorena_Lab2.Data.Solomon_Lorena_Lab2Context context)
        {
            _context = context;
        }

        // single property used by the Razor page and model binding
        [BindProperty]
        public Book Book { get; set; } = default!;

        public SelectList AuthorSelectList { get; set; } = default!;
        public SelectList PublisherSelectList { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Book = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Book == null)
            {
                return NotFound();
            }

            PopulateSelectLists();
            return Page();
        }


        private void PopulateSelectLists()
        {
            var authors = _context.Set<Author>()
                .OrderBy(a => a.LastName).ThenBy(a => a.FirstName)
                .AsNoTracking().ToList();

            // use FullName for display, pass current Book.AuthorID as selected value
            AuthorSelectList = new SelectList(authors, "ID", "FullName", Book?.AuthorID);

            var publishers = _context.Set<Publisher>()
                .OrderBy(p => p.PublisherName)
                .AsNoTracking().ToList();

            PublisherSelectList = new SelectList(publishers, "ID", "PublisherName", Book?.PublisherID);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopulateSelectLists();
                return Page();
            }

            _context.Attach(Book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(Book.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
