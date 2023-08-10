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
    public class UsaJobsWebParser : IWebParser
    {
        public string Key { get; } = "www.usajobs.gov";

        public Vacancy GetVacancy(string htmlContent)
        {
            var result = new Vacancy();


            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(htmlContent);


            var details = new Dictionary<string, string>();


            
            var city = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[1]/div[2]/section/section/div/div/aside/div[2]/ul[2]/li[4]/ul/li/div/span[2]");
            result.JobDetails.City = city != null ? city.InnerText : string.Empty;
            result.JobDetails.Country = city != null ? city.InnerText : string.Empty;


            var company = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[1]/div[2]/div/div/h5[2]");
            result.JobDetails.Company= company!=null ? company.InnerText : string.Empty;


            var companyAddress = htmlDocument.DocumentNode.SelectSingleNode("/html/body/footer/div[3]/div[1]/div[2]/div/a[2]");
            result.JobDetails.CompanyAddress = companyAddress != null ? companyAddress.Attributes["href"].Value:"";


            var jobDescription = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[1]/div[2]/section/section/div/div/div/div[3]/div/p");
            result.JobDetails.JobDescription = jobDescription != null ? jobDescription.InnerText : string.Empty;


            var jobTitle = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[1]/div[2]/div/div/h1");
            result.JobDetails.JobTitle= jobTitle != null ? jobTitle.InnerText : string.Empty;


            var jobType = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[1]/div[2]/section/section/div/div/div/div[4]/ul[2]/li[10]/p");
            result.JobDetails.JobType= jobType!=null ? jobType.InnerText : string.Empty;


            var datePosted = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[1]/div[2]/section/section/div/div/aside/div[2]/ul[2]/li[1]/p/span[1]");
            if (datePosted != null)
            {
                var datePostedText = datePosted.InnerText;
                var index = datePostedText.IndexOf("/");
                var index2 = datePostedText.IndexOf("/", index + 1);
                var date = datePostedText.Substring(0, index);
                var month = datePostedText.Substring(index + 1, index2 - index - 1);
                var year = datePostedText.Substring(index2 + 1);
                DateTime datePostedFullDate = new DateTime(int.Parse(year), int.Parse(month), int.Parse(date));
                result.JobDetails.PostDate = datePostedFullDate;
            }


            var salary = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[1]/div[2]/section/section/div/div/aside/div[2]/ul[2]/li[2]/p[1]");
            if (salary != null)
            {
                var salaryText = salary.InnerText;
                salaryText = salaryText.ToLower();
                var salaryPeriod = "";
                if (salaryText.Contains("year"))
                {
                    salaryPeriod = "yearly";
                }
                
                var index = salaryText.IndexOf("-");
                var salaryMin = salaryText.Substring(0,index);
                var salaryMax = salaryText.Substring(index);


                if (salaryMin.Contains("K"))
                {
                    salaryMin = salaryMin.Replace("K", "000");
                }
                if (salaryMax.Contains("K"))
                {
                    salaryMax = salaryMax.Replace("K", "000");
                }

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


            var responsibilities = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[1]/div[2]/section/section/div/div/div/div[7]/ul");
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
