using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace cvWebScraper.Models
{
    public class VacancyDetails
    {
        [JsonPropertyName("job_title")]
        public string JobTitle { get; set; }


        [JsonPropertyName("company")]
        public string Company { get; set; }


        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("companyAddress")]
        public string CompanyAddress { get; set; }


        [JsonPropertyName("country")]
        public string Country { get; set; }


        [JsonPropertyName("job_description")]
        public string JobDescription { get; set; }


        [JsonPropertyName("job_type")]
        public string JobType { get; set; }


        [JsonPropertyName("salary_min")]
        public decimal SalaryMin { get; set; }


        [JsonPropertyName("salary_max")]
        public decimal SalaryMax { get; set; }


        [JsonPropertyName("salary_period")]
        public string SalaryPeriod { get; set; }


        [JsonPropertyName("job_details")]
        public DateTime PostDate { get; set; }
    }
}
