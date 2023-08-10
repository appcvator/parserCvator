using parser.Interfaces;
using parser.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace parser.Services
{
    public class LinkedInWebParser : IWebParser
    {
        public string Key { get; } = "www.linkedin.com";

        public Vacancy GetVacancy(string htmlContent)
        {
            var result = new Vacancy();


            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(htmlContent);


            var details = new Dictionary<string, string>();


            var cityCountry = htmlDocument.DocumentNode.SelectSingleNode("/html/body/main/section[1]/div/section[2]/div/div[1]/div/h4/div[1]/span[2]");
            if (cityCountry != null)
            {
                var cityCountryText = cityCountry.InnerText;
                var index = cityCountryText.IndexOf(",");
                var city = cityCountryText.Substring(0, index);
                var country = cityCountryText.Substring(index + 1);
                result.JobDetails.City = city;
                result.JobDetails.Country = country;
            }


            var companyAddress = htmlDocument.DocumentNode.SelectSingleNode("/html/body/main/section[1]/div/section[2]/div/div[1]/div/h4/div[1]/span[1]/a");
            result.JobDetails.CompanyAddress = companyAddress != null ? companyAddress.Attributes["href"].Value : "";


            var company = htmlDocument.DocumentNode.SelectSingleNode("/html/body/main/section[1]/div/section[2]/div/div[1]/div/h4/div[1]/span[1]/a");
            result.JobDetails.Company = company != null ? company.InnerText : "";


            //unavailable
            //var jobDescription = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"main-content\"]/section[1]/div/div/section[1]/div/div/section/div/text()[1]");
            //result.JobDetails.JobDescription =jobDescription!=null ?  jobDescription.InnerText:"";


            var jobTitle = htmlDocument.DocumentNode.SelectSingleNode("/html/body/main/section[1]/div/section[2]/div/div[1]/div/h1");
            result.JobDetails.JobTitle = jobTitle != null ? jobTitle.InnerText : "";


            var jobType = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"main-content\"]/section[1]/div/div/section[1]/div/ul/li[2]/span");
            result.JobDetails.JobType = jobType != null ? jobType.InnerText : "";


            var responsibilities = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"main-content\"]/section[1]/div/div/section[1]/div/div/section/div/ul[1]");
            if (responsibilities != null)
            {
                foreach (var childNode in responsibilities.ChildNodes)
                {
                    result.Responsibilities.Add(childNode.InnerText);
                }
            }

            var requirements = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"main-content\"]/section[1]/div/div/section[1]/div/div/section/div/ul[2]");
            if(requirements != null)
            {
                var experience=requirements.FirstChild.InnerText;
                var number = Regex.Match(experience, @"\d+").Value;
                experience = Regex.Replace(experience, @"[\d-]", string.Empty);
                result.Requirements.Experience.Years = number;
                result.Requirements.Experience.Description = experience;
            }


            if(requirements!= null)
            {
                var count = 0;
                foreach(var requirement in requirements.ChildNodes)
                {
                    if(count==0)
                    {
                        count = 1;
                        continue;
                    }
                    result.Requirements.Skills.Add(requirement.InnerText);
                    
                }
            }


            return result;
        }


    }
}