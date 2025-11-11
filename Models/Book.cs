using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.Pkcs;
using System.Security.Policy;

namespace Solomon_Lorena_Lab2.Models
{
    public class Book
    {
        public int Id { get; set; }

        //Adnotari
        [Display(Name = "Book Title")]
        [StringLength(150, MinimumLength = 3)]
        [Required()]
        public required string Title { get; set; } = string.Empty;   
        
        //public string Author { get; set; }
        public int? AuthorID { get; set; } //pk
        public Author? Author { get; set; } //navigation property


        //in dtb, entitatea se va genera ca si o coloana
        [Column(TypeName = "decimal(6,2)")]
        [Range(0.01, 500)]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        //adnotare pentru a seta tipul de data
        [DataType(DataType.Date)]
        //[Range(typeof(DateTime), "1900-01-01", "2025-12-31", ErrorMessage = "Date out of bounds")]
        public DateTime PublishingDate { get; set; }


        public int? PublisherID { get; set; } 
        //int? = poate fi si null
        //PublisherID = primary key

        public Publisher? Publisher { get; set; } 
        //navigation property

        public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();

        public ICollection<Borrowing>? Borrowings { get; set; }



    }
}
