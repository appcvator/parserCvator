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
    public class NaukriWebParser : IWebParser
    {
        public string Key { get; } = "www.naukri.com";

        public Vacancy GetVacancy(string htmlContent)
        {
            var result = new Vacancy();


            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(htmlContent);


            var details = new Dictionary<string, string>();


            
            var city = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"root\"]/main/div[2]/div[1]/section[1]/div[1]/div[2]/div[2]/span/a");
            if(city!=null)
            {
                var cityText=city.InnerText;
                var index=cityText.IndexOf("/");
                cityText = cityText.Substring(0, index);
                result.JobDetails.City = cityText;
            }


            var company = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"root\"]/main/div[2]/div[1]/section[1]/div[1]/div[1]/div/a[1]");
            result.JobDetails.Company = company != null ? company.InnerText : string.Empty;



            var jobDescription = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"root\"]/main/div[2]/div[1]/section[2]/div[1]/ul[1]");
            if (jobDescription != null)
            {

                foreach (var childNode in jobDescription.ChildNodes)
                {
                    result.JobDetails.JobDescription+=childNode.InnerText;
                }
            }


            var jobTitle = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"root\"]/main/div[2]/div[1]/section[1]/div[1]/div[1]/header/h1");
            result.JobDetails.JobTitle = jobTitle != null ? jobTitle.InnerText : string.Empty;


            var jobType = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"root\"]/main/div[2]/div[1]/section[2]/div[2]/div[4]/span/span");
            result.JobDetails.JobType = jobType != null ? jobType.InnerText : string.Empty;




            var postDate = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"root\"]/main/div[2]/div[1]/section[1]/div[2]/div[1]/span[1]/span");
            if (postDate != null)
            {
                DateTime currentTime = DateTime.Now;
                var postDateProcessed = string.Empty;
                foreach (var c in postDate.InnerText)
                {
                    int ascii = (int)c;
                    if ((ascii >= 48 && ascii <= 57) || ascii == 44 || ascii == 46)
                        postDateProcessed += c;
                }
                var postDateParsed = int.Parse(postDateProcessed);
                var postDateDateTime = currentTime.AddDays(-postDateParsed);
                result.JobDetails.PostDate = postDateDateTime;
            }



            


            var preferred = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"root\"]/main/div[2]/div[1]/section[2]/div[1]/p[2]/strong[2]");
            if (preferred != null)
            {
                result.Requirements.PreferredQualifications.Add(preferred.InnerText);
            }


            var skills = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"root\"]/main/div[2]/div[1]/section[2]/div[1]/ul[2]/li");
            if (skills != null)
            {
                result.Requirements.Skills.Add(skills.InnerText);
            }


            var education = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"root\"]/main/div[2]/div[1]/section[2]/div[3]");
            if (education != null)
            {
                result.Requirements.Education=education.InnerText;
               
            }


            
            return result;
        }


    }
}
