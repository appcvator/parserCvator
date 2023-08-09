using parser.Interfaces;
using parser.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace parser.Services
{
    public class GlassdoorWebParser : IWebParser
    {
        public string Key { get; } = "www.glassdoor.com";

        public Vacancy GetVacancy(string htmlContent)
        {
            var result = new Vacancy();

            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(htmlContent);

            var titleNode = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div/div[1]/div[1]/div[2]/div/div/div[2]/div/div[1]/div[2]/div/div/div[2]");
            result.JobDetails.JobTitle = titleNode != null ? titleNode.InnerText : string.Empty;


            var companyNode = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div/div[1]/div[1]/div[2]/div/div/div[2]/div/div[1]/div[2]/div/div/div[1]/div/text()");
            result.JobDetails.Company = companyNode != null ? companyNode.InnerText : "";


            var skillNodes = htmlDocument.DocumentNode.SelectNodes("/html/body/div[2]/div/div/div[1]/div[2]/div/div/div/div[1]/div/div[2]/div/div/ul[2]");
            if (skillNodes != null)
            {
                result.Requirements.Skills.AddRange(skillNodes.Select(c => c.InnerText));
            }

            
            var country = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div/div[1]/div[1]/div[2]/div/div/div[2]/div/div[1]/div[2]/div/div/div[3]/span");
            result.JobDetails.Country = country != null ? country.InnerText : "";


            var city = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div/div[1]/div[2]/div/div/div/div[1]/div/div[1]/div[1]/div");
            result.JobDetails.City = city != null ? city.InnerText : "";


            var jobDesciption=htmlDocument.DocumentNode.SelectNodes("/html/body/div[2]/div/div/div[1]/div[2]/div/div/div/div[1]/div/div[2]/div/div/ul[1]");
            if (jobDesciption != null)
            {
                var textFromJobDescription = "";
                foreach (var text in jobDesciption)
                {
                    textFromJobDescription += text.InnerText;
                }

                result.JobDetails.JobDescription = textFromJobDescription;
            }


            var jobType = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div/div[1]/div[2]/div/div/div/div[1]/div/div[1]/div[2]/div");
            result.JobDetails.JobType = jobType != null ? jobType.InnerText : "";


            var salary = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div/div[1]/div[1]/div[2]/div/div/div[2]/div/div[1]/div[2]/div/div/div[4]/span/text()");
            if (salary != null)
            {
                var salaryString = salary.InnerText;
                var index = salaryString.IndexOf("-");
                var salaryMin = salaryString.Substring(0, index);
                var salaryMax = salaryString.Substring(index);
                

                if(salaryMin.Contains("K"))
                {
                    salaryMin= salaryMin.Replace("K", "000");
                }
                if (salaryMax.Contains("K"))
                {
                    salaryMax=salaryMax.Replace("K", "000");
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
                result.JobDetails.SalaryMin =Decimal.Parse(salaryMinProcessed);
                result.JobDetails.SalaryMax = Decimal.Parse(salaryMaxProcessed);
            }


            var averageSalary = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div/div[1]/div[2]/div/div/div/div[2]/div[1]");
            if (averageSalary != null)
            {
                var averageSalaryString = averageSalary.InnerText;
                var index = averageSalaryString.IndexOf("/");
                var index2 = averageSalaryString.IndexOf("(");
                var salaryPeriod = averageSalaryString.Substring(index + 1, index2 - index);
                result.JobDetails.SalaryPeriod = salaryPeriod;
            }

            return result;
        }


    }
}
