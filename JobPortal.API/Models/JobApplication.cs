namespace JobPortal.API.Models
{
    public class JobApplication
    {
        public int Id { get; set; } 
        public int JobId { get; set; }
        public Job  Job {  get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Applied";

  
    }
}
