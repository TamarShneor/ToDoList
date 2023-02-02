namespace User.Models
{
    public class user 
    {
        internal int id;

        public int Id { get; set; }
        public string ?Name { get; set; }
        public string ?Password  { get; set; }
        public bool	IsAdmin  { get; set; }

    }

}