using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")] // Restrict access to Admin role
    public class EditModel : BookCategoriesPageModel
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
                .Include(b => b.BookCategories).ThenInclude(b => b.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Book == null)
            {
                return NotFound();
            }

            PopulateAssignedCategoryData(_context, Book);
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


        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCategories)
        {

            if (id == null)
            {
                return NotFound();
            }

            //se va include Author conform cu sarcina de la lab 2
            var bookToUpdate = await _context.Book
            .Include(i => i.Publisher)
            .Include(i => i.BookCategories)
            .ThenInclude(i => i.Category)
            .FirstOrDefaultAsync(s => s.Id == id);

            if (bookToUpdate == null)
            {
                return NotFound();
            }

            //se va modifica AuthorID conform cu sarcina de la lab 2
            if (await TryUpdateModelAsync<Book>(
            bookToUpdate,
            "Book",
            i => i.Title, i => i.AuthorID,
            i => i.Price, i => i.PublishingDate, i => i.PublisherID))
            {
                UpdateBookCategories(_context, selectedCategories, bookToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            //Apelam UpdateBookCategories pentru a aplica informatiile din checkboxuri la entitatea Books care
            //este editata
            UpdateBookCategories(_context, selectedCategories, bookToUpdate);
            PopulateAssignedCategoryData(_context, bookToUpdate);
            PopulateSelectLists();

            return Page();
        }



        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
