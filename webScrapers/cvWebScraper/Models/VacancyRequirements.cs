using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvWebScraper.Models
{
    public class VacancyRequirements
    {
        private List<string> _skills;
        private List<string> _preferredQualifications;
        private Experience _experience;

        public string Education { get; set; }
        public Experience Experience { get { return _experience; } set { _experience = value; } }
        public List<string> Skills { get { return _skills; } set { _skills = value; } }
        public List<string> PreferredQualifications { get { return _preferredQualifications; } set { _preferredQualifications = value; } }

        public VacancyRequirements()
        {
            _skills = new List<string>();
            _preferredQualifications = new List<string>();
            _experience = new Experience();
        }
    }
}
