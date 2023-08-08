using cvWebScraper.Interfaces;
using cvWebScraper.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvWebScraper.Services
{
    public class IndeedWebParser : IWebParser
    {
        public string Key { get; } = "indeed.com";

        public Vacancy GetVacancy(string htmlContent)
        {
            var result = new Vacancy();

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);

            var details = new Dictionary<string, string>();

            // Extract the vacancy title
            var titleNode = htmlDocument.DocumentNode.SelectSingleNode("//h1[contains(@class, 'jobsearch-JobInfoHeader-title')]");
            result.JobDetails.JobTitle = titleNode != null ? titleNode.InnerText : string.Empty;


            // Extract the company name
            var companyNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@data-testid='inlineHeader-companyName']");
            result.JobDetails.Company = companyNode != null ? companyNode.InnerText : "";

            // Extract the company address
            var addressNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@id='location']");
            result.JobDetails.CompanyAddress = addressNode != null ? addressNode.InnerText : "";

            // Extract the skills
            var skillNodes = htmlDocument.DocumentNode.SelectNodes("//ul[@class='resumeMatch-TileContext-interactive-list']//span[@class='resumeMatch-Tile-primaryText']");
            if (skillNodes != null)
            {
                result.Requirements.Skills.AddRange(skillNodes.Select(c => c.InnerText));
            }

            // Extract the education
            var educationNodes = htmlDocument.DocumentNode.SelectNodes("//ul[@class='resumeMatch-TileContext-interactive-list']//span[@class='resumeMatch-Tile-primaryText']");
            if (educationNodes != null)
            {
                result.Requirements.Education = string.Join(", ", educationNodes.Select(c => c.InnerText));
            }

            // Extract the job description
            var jobDescriptionNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@id='jobDescriptionText']");
            result.JobDetails.JobDescription = jobDescriptionNode != null ? jobDescriptionNode.InnerText : "";

            return result;
        }

        
    }
}
