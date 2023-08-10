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
    public class ZipRecruiterWebParser : IWebParser
    {
        public string Key { get; } = "www.ziprecruiter.com";

        public Vacancy GetVacancy(string htmlContent)
        {
            var result = new Vacancy();


            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(htmlContent);


            var details = new Dictionary<string, string>();


            var cityCountry = htmlDocument.DocumentNode.SelectSingleNode("/html/body/main/div[2]/div/div[1]/div[1]/div[2]/a[2]");
            if (cityCountry != null)
            {
                string cityCountryString = cityCountry.InnerText;
                var index = cityCountryString.IndexOf(",");
                cityCountryString=cityCountryString.Substring(0, index);
                result.JobDetails.City = cityCountryString != null ? cityCountryString : "";
            }

            var company = htmlDocument.DocumentNode.SelectSingleNode("/html/body/main/div[2]/div/div[1]/div[1]/div[2]/a[1]");
            result.JobDetails.Company = company != null ? company.InnerText : "";


            


            
            if(cityCountry != null)
            {
                var cityCountryText= cityCountry.InnerText;
                var index = cityCountryText.IndexOf(",");
                var index2 = cityCountryText.IndexOf(",",index+1);
                var country=cityCountryText.Substring(index2 +1);
                result.JobDetails.Country = country;
                
            }


            var jobDescription = htmlDocument.DocumentNode.SelectSingleNode("/html/body/main/div[2]/div/div[1]/div[1]/div[3]/div[2]/div/div[2]/div/p");
            result.JobDetails.JobDescription =jobDescription != null ? jobDescription.InnerText:"";


            var jobTitle = htmlDocument.DocumentNode.SelectSingleNode("/html/body/main/div[2]/div/div[1]/div[1]/div[2]/h1");
            result.JobDetails.JobTitle = jobTitle != null ? jobTitle.InnerText : "";


            var jobType = htmlDocument.DocumentNode.SelectSingleNode("/html/body/main/div[2]/div/div[1]/div[1]/div[3]/ul/li[1]/span");
            result.JobDetails.JobType = jobType != null ? jobType.InnerText : "";


            var postDate = htmlDocument.DocumentNode.SelectSingleNode("/html/body/main/div[2]/div/div[1]/div[1]/div[3]/div[4]/p[1]/span[2]");
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


            var salary = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"main\"]/div[2]/div/div[1]/div[1]/div[3]/ul/li[2]/span");
            if(salary !=null)
            {
                var salaryText=salary.InnerText;
                var salaryPeriod = "";
                if(salaryText.Contains("hour"))
                {
                    salaryPeriod = "hourly";
                }
                var index = salaryText.IndexOf("to");
                var salaryMin=salaryText.Substring(0, index);
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



            return result;
        }


    }
}
