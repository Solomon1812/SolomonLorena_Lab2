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
    public class CreateModel : BookCategoriesPageModel
    {
        private readonly Solomon_Lorena_Lab2.Data.Solomon_Lorena_Lab2Context _context;

        public CreateModel(Solomon_Lorena_Lab2.Data.Solomon_Lorena_Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; }

        public SelectList PublisherSelectList { get; set; } = default!;
        public SelectList AuthorSelectList { get; set; } = default!;

        public IActionResult OnGet()
        {
            PopulateSelectLists();
            //PublisherSelectList = new SelectList(_context.Publisher.OrderBy(p => p.PublisherName).ToList(), "ID", "PublisherName");
            //AuthorSelectList = new SelectList(_context.Author.OrderBy(p => p.FirstName).ToList(), "ID", "FirstName");
            //AuthorSelectList = new SelectList(_context.Author.OrderBy(p => p.LastName).ToList(), "ID", "LastName");

            var book = new Book { Title = string.Empty }; // Set required property
            book.BookCategories = new List<BookCategory>();
            PopulateAssignedCategoryData(_context, book);
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

        public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
        {
            if (!ModelState.IsValid)
            {
                PopulateSelectLists();
                //PublisherSelectList = new SelectList(_context.Publisher.OrderBy(p => p.PublisherName).ToList(), "ID", "PublisherName");
                //AuthorSelectList = new SelectList(_context.Author.OrderBy(p => p.FirstName).ToList(), "ID", "FirstName");
                //AuthorSelectList = new SelectList(_context.Author.OrderBy(p => p.LastName).ToList(), "ID", "LastName");
                PopulateAssignedCategoryData(_context, Book);
                return Page();
            }

            var newBook = new Book { Title = Book.Title };

            // Add selected categories (if any)
            if (selectedCategories != null)
            {
                newBook.BookCategories = new List<BookCategory>();

                foreach (var cat in selectedCategories)
                {
                    var catToAdd = new BookCategory
                    {
                        CategoryID = int.Parse(cat)
                    };
                    newBook.BookCategories.Add(catToAdd);
                }
            }

            // Transfer data from the bound Book to newBook
            newBook.Title = Book.Title;
            newBook.AuthorID = Book.AuthorID;
            newBook.Price = Book.Price;
            newBook.PublishingDate = Book.PublishingDate;
            newBook.PublisherID = Book.PublisherID;

            // ADD the composed newBook (which includes BookCategories), not the bound Book
            _context.Book.Add(newBook);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
