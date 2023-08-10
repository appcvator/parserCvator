using cvWebScraper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvWebScraper.Services
{
    public class WebParser
    {
        private readonly Dictionary<string, IWebParser> _parsers;
        public WebParser() {

            //can do reflection later. for now use Dictionary for fast implementation of IWebParser
            _parsers = new Dictionary<string, IWebParser>
            {
                { "indeed.com", new IndeedWebParser() },
                {"glassdoor.com", new GlassdoorWebParser() },
                {"linkedin.com", new LinkedInWebParser() },
                {"monster.com", new MonsterWebParser() },
                {"ziprecruiter.com", new ZipRecruiterWebParser() },
                {"chicago.craigslist.org", new ChicagoCraigslistWebParser() },
                {"usajobs.gov", new UsaJobsWebParser() },
                {"careerbuilder.co.uk", new CareerBuilderWebParser() },
                {"simplyhired.com", new SimplyHiredWebParser() },
                {"naukri.com", new NaukriWebParser() },
                {"foundit.in", new FoundItInWebParser() }
            };
        }

        public async Task<string> GetVacancyDetails(string url, string htmlContent)
        {
            var parser = GetWebScraper(url);
            if(parser != null)
            {
                var result = parser.GetVacancy(htmlContent);
            }
            return "no parser found";
        }

        private IWebParser GetWebScraper(string url)
        {
            var uri = new Uri(url);
            var key = uri.Host.Replace("www.", "");

            if (_parsers.ContainsKey(key))
                return _parsers[key];

            //todo: return default parser
            return null;

        }
    }
}
