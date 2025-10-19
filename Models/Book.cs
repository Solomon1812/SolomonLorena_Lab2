using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace Solomon_Lorena_Lab2.Models
{
    public class Book
    {
        public int Id { get; set; }

        //Adnotari
        [Display(Name = "Book Title")]
        public required string Title { get; set; } = string.Empty;   
        
        //public string Author { get; set; }
        public int? AuthorID { get; set; } //pk
        public Author? Author { get; set; } //navigation property


        //in dtb, entitatea se va genera ca si o coloana
        [Column(TypeName = "decimal(6, 2)")]
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
    }
}
