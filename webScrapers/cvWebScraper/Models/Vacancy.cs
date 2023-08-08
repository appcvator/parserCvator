using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace cvWebScraper.Models
{
    public class Vacancy
    {
        private List<string> _responsibilities;
        private VacancyDetails _jobDetails;
        private VacancyRequirements _requirements;

        [JsonPropertyName("job_details")]
        public VacancyDetails JobDetails { get { return _jobDetails; } set { _jobDetails = value; } }

        [JsonPropertyName("requirements")]
        public VacancyRequirements Requirements { get { return _requirements; } set { _requirements = value; } }

        [JsonPropertyName("responsibilities")]
        public List<string> Responsibilities { get { return _responsibilities; } set { _responsibilities = value; } }

        public Vacancy()
        {
            _responsibilities = new List<string>();
            _jobDetails = new VacancyDetails();
            _requirements = new VacancyRequirements();
        }
    }
}
