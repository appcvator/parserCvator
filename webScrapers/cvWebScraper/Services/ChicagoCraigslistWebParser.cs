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
    public class ChicagoCraigslistWebParser : IWebParser
    {
        public string Key { get; } = "www.chicago.craigslist.org";

        public Vacancy GetVacancy(string htmlContent)
        {
            var result = new Vacancy();


            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(htmlContent);


            var details = new Dictionary<string, string>();


            var company = htmlDocument.DocumentNode.SelectSingleNode("/html/body/section/section/h2");
            result.JobDetails.Company = company != null ? company.InnerText : "";


            var jobDescription = htmlDocument.DocumentNode.SelectSingleNode("/html/body/section/section/section/section/p[6]");
            result.JobDetails.JobDescription = jobDescription != null ? jobDescription.InnerText : "";


            var jobTitle = htmlDocument.DocumentNode.SelectSingleNode("/html/body/section/section/section/div[1]/p/span[3]/b");
            result.JobDetails.JobTitle = jobTitle != null ? jobTitle.InnerText : "";


            var jobType = htmlDocument.DocumentNode.SelectSingleNode("/html/body/section/section/section/div[1]/p/span[2]/b");
            result.JobDetails.JobType = jobType != null ? jobType.InnerText : "";


            var postDate = htmlDocument.DocumentNode.SelectSingleNode("/html/body/section/section/header/div[2]/p[1]/time");
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

            var salary = htmlDocument.DocumentNode.SelectSingleNode("/html/body/section/section/section/section/ul[1]/li[3]");
            if (salary != null)
            {
                var salaryText=salary.InnerText;
                salaryText=salaryText.ToLower();


                var salaryPeriod = "";
                if(salaryText.Contains("year"))
                {
                    salaryPeriod = "yearly";
                }
                else if(salaryText.Contains("month"))
                {
                    salaryPeriod = "monthly";
                }
                else if(salaryText.Contains("hour"))
                {
                    salaryPeriod = "hourly";
                }
                else { salaryPeriod = ""; }


                var number = Regex.Match(salaryText, @"\d+").Value;
                var index=salaryText.IndexOf(number);
                salaryText=salaryText.Substring(index);
                while (salaryText.Contains("K"))
                {
                    salaryText = salaryText.Replace("K", "000");
                }
                
                var index2 = salaryText.IndexOf("-");
                var salaryMin = salaryText.Substring(0, index2);
                var salaryMax = salaryText.Substring(index2);


                var salaryMinProcessed = string.Empty;
                foreach (var c in salaryMin)
                {
                    int ascii = (int)c;
                    if ((ascii >= 48 && ascii <= 57) || ascii == 44 || ascii == 46)
                        salaryMinProcessed += c;
                }
                var salaryMaxProcessed = string.Empty;
                foreach (var c in salaryMax)
                {
                    int ascii = (int)c;
                    if ((ascii >= 48 && ascii <= 57) || ascii == 44 || ascii == 46)
                        salaryMaxProcessed += c;
                }
                result.JobDetails.SalaryMin = Decimal.Parse(salaryMinProcessed);
                result.JobDetails.SalaryMax = Decimal.Parse(salaryMaxProcessed);
                result.JobDetails.SalaryPeriod = salaryPeriod;
            }

            var responsibilities = htmlDocument.DocumentNode.SelectSingleNode("/html/body/section/section/section/section/ul[3]");
            if (responsibilities != null)
            {
                foreach (var childNode in responsibilities.ChildNodes)
                {
                    result.Responsibilities.Add(childNode.InnerText);
                }
            }


            



            return result;
        }


    }
}
