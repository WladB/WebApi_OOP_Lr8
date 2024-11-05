using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Djini_OOP_Lab8
{
    public class Vacancy
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? Caption { get; set; }
        public string? Description { get; set; }
        public int CompanyId { get; set; }
        [JsonIgnore]
        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; }
    }
    public class Worker
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Adress { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
    public class Resume
    {
        public int VacancyId { get; set; }

        [JsonIgnore]
        [ForeignKey("VacancyId")]
        public virtual Vacancy? Vacancy { get; set; }

        public int WorkerId { get; set; }
        [JsonIgnore]
        [ForeignKey("WorkerId")]
        public virtual Worker? Worker { get; set; }
        public string? ResumeContent { get; set; }
      
    }
    public class Feedback
    {
        public int WorkerId { get; set; }

        [JsonIgnore]
        [ForeignKey("WorkerId")]
        public virtual Worker? Worker { get; set; }

        public int CompanyId { get; set; }

        [JsonIgnore]
        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; }

        public string? FeedbackContent { get; set; }
    }
    public class Company
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? Caption { get; set; }
    }
}
