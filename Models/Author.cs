namespace Solomon_Lorena_Lab2.Models
{
    public class Author
    {
        public int ID { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;

        // display: "LastName, FirstName"
        public string FullName => $"{LastName}, {FirstName}";

    }
}
