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
    public class SimplyHiredWebParser : IWebParser
    {
        public string Key { get; } = "www.simplyhired.com";

        public Vacancy GetVacancy(string htmlContent)
        {
            var result = new Vacancy();


            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(htmlContent);


            var details = new Dictionary<string, string>();


            
            var city = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"__next\"]/div/main/div/div/aside/header/div/div/div[2]/div[1]/span[2]/span/span");
            result.JobDetails.City = city != null ? city.InnerText : string.Empty;


            var company= htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"__next\"]/div/main/div/div/aside/header/div/div/div[2]/div[1]/span[1]/span/span[1]");
            result.JobDetails.Company = company != null ? company.InnerText : string.Empty;


            


            var jobDescription = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"__next\"]/div/main/div/div/aside/div/div[1]/div/div[4]/div");
            if(jobDescription != null )
            {
                var jobText=jobDescription.InnerText;
                jobText = jobText.ToLower();
                var index = jobText.IndexOf("key responsibilities");
                jobText = jobText.Substring(0, index);
                result.JobDetails.JobDescription = jobText;
            }


            var jobTitle = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"__next\"]/div/main/div/div/aside/header/div/div/div[1]/h2");
            result.JobDetails.JobTitle=jobTitle !=null ? jobTitle.InnerText : string.Empty;


            var jobType = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"__next\"]/div/main/div/div/aside/div/div[1]/div/div[1]/div/span[1]/span/span");
            result.JobDetails.JobType = jobType != null ? jobType.InnerText : string.Empty;


            var postDate = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"__next\"]/div/main/div/div/aside/div/div[1]/div/div[1]/div/span[3]/span/span");
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


            var salary = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"__next\"]/div/main/div/div/aside/div/div[1]/div/div[1]/div/span[2]/span/span");
            if (salary != null)
            {
                var salaryText = salary.InnerText;
                salaryText = salaryText.ToLower();
                var salaryPeriod = "";
                if (salaryText.Contains("year"))
                {
                    salaryPeriod = "yearly";
                }
                else if (salaryText.Contains("month"))
                {
                    salaryPeriod = "monthly";
                }
                else if (salaryText.Contains("hour"))
                {
                    salaryPeriod = "hourly";
                }
                else { salaryPeriod = ""; }
                var index = salaryText.IndexOf("-");
                var salaryMin = salaryText.Substring(0, index);
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


            var responsibilities = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"__next\"]/div/main/div/div/aside/div/div[1]/div/div[4]/div/ul[1]");
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