using parser.Interfaces;
using parser.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

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


            var cityXpath = "//*[@id='main-content']//a//following::*";
            var companyXpath = "//*[@id='main-content']//a";
            var companyAddressXpath = "//*[@id='main-content']//a";
            var jobDescriptionXpath = "//button[contains(text(),'Show more')]/preceding-sibling::*";
            var jobTitleXpath = "//*[@id='main-content']//section[2]//h1";
            var jobTypeXpath = "//text()[contains(., 'Employment type')]//following::*";
            


            var city = htmlDocument.DocumentNode.SelectSingleNode(cityXpath);
            result.JobDetails.City = city != null ? city.InnerText : "";


            var company = htmlDocument.DocumentNode.SelectSingleNode(companyXpath);
            result.JobDetails.Company = company != null ? company.InnerText : "";


            var companyAddress = htmlDocument.DocumentNode.SelectSingleNode(companyAddressXpath);
            result.JobDetails.CompanyAddress = companyAddress != null ? companyAddress.Attributes["href"].Value : "";



            var jobDescription = htmlDocument.DocumentNode.SelectSingleNode(jobDescriptionXpath);
            if(jobDescription!=null)
            {
                foreach(var item in jobDescription.ChildNodes )
                {
                    if(item.InnerText.Length>20)
                    {
                        result.JobDetails.JobDescription = item.InnerText;
                        break;
                    }
                }
            }


            var jobTitle = htmlDocument.DocumentNode.SelectSingleNode(jobTitleXpath);
            result.JobDetails.JobTitle = jobTitle != null ? jobTitle.InnerText : "";


            var jobType = htmlDocument.DocumentNode.SelectSingleNode(jobTypeXpath);
            result.JobDetails.JobType = jobType != null ? jobType.InnerText : "";


            //var responsibilities = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"main-content\"]/section[1]/div/div/section[1]/div/div/section/div/ul[1]");
            //if (responsibilities != null)
            //{
            //    foreach (var childNode in responsibilities.ChildNodes)
            //    {
            //        result.Responsibilities.Add(childNode.InnerText);
            //    }
            //}

            //var requirements = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"main-content\"]/section[1]/div/div/section[1]/div/div/section/div/ul[2]");
            //if(requirements != null)
            //{
            //    var experience=requirements.FirstChild.InnerText;
            //    var number = Regex.Match(experience, @"\d+").Value;
            //    experience = Regex.Replace(experience, @"[\d-]", string.Empty);
            //    result.Requirements.Experience.Years = number;
            //    result.Requirements.Experience.Description = experience;
            //}


            //if(requirements!= null)
            //{
            //    var count = 0;
            //    foreach(var requirement in requirements.ChildNodes)
            //    {
            //        if(count==0)
            //        {
            //            count = 1;
            //            continue;
            //        }
            //        result.Requirements.Skills.Add(requirement.InnerText);
                    
            //    }
            //}


            return result;
        }


    }
}