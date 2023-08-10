using parser.Interfaces;
using parser.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parser.Services
{
    public class FoundItInWebParser : IWebParser
    {
        public string Key { get; } = "www.foundit.in";

        public Vacancy GetVacancy(string htmlContent)
        {
            var result = new Vacancy();


            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(htmlContent);


            var details = new Dictionary<string, string>();


            
            var city = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"jobHighlight\"]/div[1]/div/div[1]/div[2]/a");
            result.JobDetails.City = city != null ? city.InnerText : string.Empty;


            var company = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"jdData\"]/div[1]/div/div[2]/span/a");
            result.JobDetails.Company=company !=null ? company.InnerText : string.Empty;


            var jobDescription = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"jobDescription\"]/div/div/p");
            result.JobDetails.JobDescription = jobDescription != null ? jobDescription.InnerText : string.Empty;


            var jobTitle = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"jdData\"]/div[1]/div/div[1]/h1/div");
            result.JobDetails.JobTitle = jobTitle != null ? jobTitle.InnerText : string.Empty;


            var jobType = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"jobSummary\"]/div[1]/div[2]/div/a");
            result.JobDetails.JobType = jobType != null ? jobType.InnerText : string.Empty;


            //var titleNode = htmlDocument.DocumentNode.SelectSingleNode("");


            //var titleNode = htmlDocument.DocumentNode.SelectSingleNode("");



            //var titleNode = htmlDocument.DocumentNode.SelectSingleNode("");





            return result;
        }


    }
}