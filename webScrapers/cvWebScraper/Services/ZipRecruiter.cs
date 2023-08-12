using parser.Interfaces;
using parser.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Text.Json.Serialization;

namespace parser.Services
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

            var cityXpath = "//*[contains(@class,'hiring_location')]";
            var companyXpath = "//*[contains(@class,'hiring_company')]";
            var jobDescriptionXpath = "//*[contains(@class,'description')]//*[string-length(text())>50]/parent::*";
            var jobTitleXpath = "//*[contains(@class,'job_title')]";
            var jobTypeXpath = "//*[contains(@class,'employment_type')]";
            var salaryXpath = "//*[contains(@class,'compensation')]//span";
            var postDateXpath = "";



            var city = htmlDocument.DocumentNode.SelectSingleNode(cityXpath);
            result.JobDetails.City = city != null ? city.InnerText : "";


            
            //if(cityCountry != null)
            //{
            //    var cityCountryText= cityCountry.InnerText;
            //    var index = cityCountryText.IndexOf(",");
            //    var index2 = cityCountryText.IndexOf(",",index+1);
            //    var country=cityCountryText.Substring(index2 +1);
            //    result.JobDetails.Country = country;
                
            //}


            var jobDescription = htmlDocument.DocumentNode.SelectSingleNode(jobDescriptionXpath);
            if (jobDescription != null)
            {
                foreach (var item in jobDescription.ChildNodes)
                {
                    var a = item.InnerText;
                    if (item.InnerText.Length > 150)
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


            //var postDate = htmlDocument.DocumentNode.SelectSingleNode(postDateXpath);
            //if (postDate != null)
            //{
            //    DateTime currentTime = DateTime.Now;
            //    var postDateProcessed = string.Empty;
            //    foreach (var c in postDate.InnerText)
            //    {
            //        int ascii = (int)c;
            //        if ((ascii >= 48 && ascii <= 57) || ascii == 44 || ascii == 46)
            //            postDateProcessed += c;
            //    }
            //    var postDateParsed = int.Parse(postDateProcessed);
            //    var postDateDateTime = currentTime.AddDays(-postDateParsed);
            //    result.JobDetails.PostDate = postDateDateTime;
            //}


            var salary = htmlDocument.DocumentNode.SelectSingleNode(salaryXpath);
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



            var company = htmlDocument.DocumentNode.SelectSingleNode(companyXpath);
            result.JobDetails.Company = company != null ? company.InnerText : "";


            


            return result;
        }


    }
}