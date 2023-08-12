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


            var cityXpath = "//div[@id='jobHighlight']//a";
            var companyXpath = "//*[contains(@class,'jdCompanyName')]";
            var companyAddressXpath = "//*[@class='jobCompany']//*[@class='companySource']//a";
            var jobDescriptionXpath = "";
            var jobTitleXpath = "//*[@class='jobTitle']";
            var jobTypeXpath = "//*[contains(text(),'JOB TYPE')]/parent::*/following::*";
            var postDateXpath = "";
            var experienceXpath = "";



            var city = htmlDocument.DocumentNode.SelectSingleNode(cityXpath);
            result.JobDetails.City = city != null ? city.InnerText : string.Empty;


            var company = htmlDocument.DocumentNode.SelectSingleNode(companyXpath);
            result.JobDetails.Company=company !=null ? company.InnerText : string.Empty;


            var companyAddress = htmlDocument.DocumentNode.SelectSingleNode(companyAddressXpath);
            result.JobDetails.CompanyAddress = companyAddress != null ? companyAddress.Attributes["href"].Value : "";


            //var jobDescription = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"jobDescription\"]/div/div/p");
            //result.JobDetails.JobDescription = jobDescription != null ? jobDescription.InnerText : string.Empty;


            var jobTitle = htmlDocument.DocumentNode.SelectSingleNode(jobTitleXpath);
            result.JobDetails.JobTitle = jobTitle != null ? jobTitle.InnerText : string.Empty;


            var jobType = htmlDocument.DocumentNode.SelectSingleNode(jobTypeXpath);
            result.JobDetails.JobType = jobType != null ? jobType.InnerText : string.Empty;


            //var experience = htmlDocument.DocumentNode.SelectSingleNode(experienceXpath);
            //if(experience!=null)
            //{
            //    //var experienceText = experience.InnerText;
            //    //experienceText = experienceText.ToLower();
            //    //var searchWord = "exp";
            //    //var index=experienceText.IndexOfAny("0123456789".ToCharArray());
            //    //var index2 = experienceText.IndexOf(searchWord);
            //    //experienceText = experienceText.Substring(index,index2+searchWord.Length);
            //    //var index3 = experienceText.IndexOf(searchWord);
            //    //var years= experienceText.Substring(index3);
            //    //var description = experienceText.Substring(0, index3);

            //    //result.Requirements.Experience.Years = experienceText.Substring(index3);
            //    //result.Requirements.Experience.Description = experienceText.Substring(0,index3);
            //}


            //var education = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"jobSummary\"]/div[6]/div[2]");
            //result.Requirements.Education = education != null ? education.InnerText : string.Empty ;


            //var skills = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"jobSummary\"]/div[5]/div[2]");
            //if(skills!=null)
            //{
            //    foreach(var item in skills.ChildNodes)
            //    {
            //        result.Requirements.Skills.Add(item.InnerText);
            //    }
            //}


            





            return result;
        }


    }
}