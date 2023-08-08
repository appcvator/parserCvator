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
                { "indeed.com", new IndeedWebParser() }
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
