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

        [BindProperty]
        public Book book { get; set; } = default!;

        // Add this property to your EditModel class
        [BindProperty]
        public Book Book { get; set; }

        public SelectList AuthorSelectList { get; set; } = default!;
        public SelectList PublisherSelectList { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            book = await _context.Book.Include(b => b.Author).Include(b => b.Publisher).FirstOrDefaultAsync(m => m.Id == id);

            //var book =  await _context.Book.FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            book = book;

            PopulateSelectLists();
            //ViewData["PublisherID"] = new SelectList(_context.Set<Publisher>(), "ID", "PublisherName");
            //ViewData["AuthorID"] = new SelectList(_context.Set<Author>(), "ID", "FirstName");
            //ViewData["AuthorID"] = new SelectList(_context.Set<Author>(), "ID", "LastName");

            return Page();
        }


        private void PopulateSelectLists()
        {
            var authors = _context.Set<Author>()
                .OrderBy(a => a.LastName).ThenBy(a => a.FirstName)
                .AsNoTracking().ToList();

                AuthorSelectList = new SelectList(authors, "ID", "FullName"); //, book?.AuthorID
            

            var publishers = _context.Set<Publisher>()
                .OrderBy(p => p.PublisherName)
                .AsNoTracking().ToList();

            PublisherSelectList = new SelectList(publishers, "ID", "PublisherName"); //, book?.PublisherID
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.Id))
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
