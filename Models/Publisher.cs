namespace Solomon_Lorena_Lab2.Models
{
    public class Publisher
    {

        public int ID { get; set; }
        //FK pentru entitatea Book
        public string PublisherName { get; set; }
        public ICollection<Book> Books { get; set; }
        //navigation property
    }
}
