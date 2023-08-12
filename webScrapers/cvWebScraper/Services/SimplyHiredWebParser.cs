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


            var cityXpath = "//*[@data-testid='viewJobCompanyLocation']";
            var companyXpath = "//*[@data-testid='viewJobCompanyName']";
            //var companyAddressXpath = "";
            var jobTitleXpath = "//*[@data-testid='viewJobTitle']";
            var jobTypeXpath = "//*[@data-testid='viewJobBodyJobDetailsJobType']//*[2]";
            var salaryXpath = "//*[@data-testid='viewJobBodyJobCompensation']";
            var jobDescriptionXpath = "";
            var postDateXpath = "";


            var jobType = htmlDocument.DocumentNode.SelectSingleNode(jobTypeXpath);
            result.JobDetails.JobType = jobType != null ? jobType.InnerText : string.Empty;


            var city = htmlDocument.DocumentNode.SelectSingleNode(cityXpath);
            result.JobDetails.City = city != null ? city.InnerText : string.Empty;


            var company = htmlDocument.DocumentNode.SelectSingleNode(companyXpath);
            result.JobDetails.Company = company != null ? company.InnerText : string.Empty;


            var jobTitle = htmlDocument.DocumentNode.SelectSingleNode(jobTitleXpath);
            result.JobDetails.JobTitle = jobTitle != null ? jobTitle.InnerText : string.Empty;


            //var postDate = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"__next\"]/div/main/div/div/aside/div/div[1]/div/div[1]/div/span[3]/span/span");
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


            var salary = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"__next\"]/div/main/div/div/aside/div/div[1]/div/div[1]/div/span[2]/span/span");
            if (salary != null)
            {
                var salaryText = salary.InnerText;
                salaryText = salaryText.ToLower();


                
                var index2=salaryText.LastIndexOf(' ');
                var salaryPeriod = salaryText.Substring(index2+1);



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


            //var jobDescription = htmlDocument.DocumentNode.SelectSingleNode(jobDescriptionXpath);
            //if (jobDescription != null)
            //{
            //    var jobText = jobDescription.InnerText;
            //    jobText = jobText.ToLower();
            //    var index = jobText.IndexOf("key responsibilities");
            //    jobText = jobText.Substring(0, index);
            //    result.JobDetails.JobDescription = jobText;
            //}


            //var responsibilities = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"__next\"]/div/main/div/div/aside/div/div[1]/div/div[4]/div/ul[1]");
            //if (responsibilities != null)
            //{

            //    foreach (var childNode in responsibilities.ChildNodes)
            //    {
            //        result.Responsibilities.Add(childNode.InnerText);
            //    }
            //}

            //var qualifications = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"__next\"]/div/main/div/div/aside/div/div[1]/div/div[4]/div/ul[2]");
            //if (qualifications != null)
            //{
            //    foreach (var item in qualifications.ChildNodes)
            //    {
            //        var education = item.InnerText;
            //        education = education.ToLower();
            //        var searchWord = "degree";
            //        var index = education.IndexOf(searchWord);
            //        if (index < 0)
            //        {
            //            continue;
            //        }
            //        education = education.Substring(0, index + searchWord.Length);

            //        result.Requirements.Education = education;

            //        break;
            //    }
            //}
            //if (qualifications != null)
            //{
            //    foreach (var item in qualifications.ChildNodes)
            //    {
            //        var experience = item.InnerText;
            //        experience = experience.ToLower();
            //        var searchWord = "year";
            //        if (experience.Contains(searchWord))
            //        {
            //            var index = experience.IndexOf(searchWord);
            //            var years = experience.Substring(0, index + searchWord.Length);
            //            var description = experience.Substring(index + searchWord.Length);


            //            result.Requirements.Experience.Description = description;
            //            result.Requirements.Experience.Years = years;
            //            break;
            //        }
            //    }
            //}


            //if (qualifications != null)
            //{
            //    List<string> preferred = new List<string>();
            //    foreach (var item in qualifications.ChildNodes)
            //    {
            //        var itemText = item.InnerText;
            //        itemText = itemText.ToLower();
            //        var searchWord = "preferred";

            //        if (itemText.Contains(searchWord))
            //        {
            //            var searchWordIndex = itemText.IndexOf(searchWord);
            //            var sentenceEndIndex = itemText.IndexOf(";");
            //            while (sentenceEndIndex > 0 && searchWordIndex > sentenceEndIndex)
            //            {
            //                itemText = itemText.Substring(sentenceEndIndex+1);
            //                sentenceEndIndex = itemText.IndexOf(";");

            //            }
            //            preferred.Add(itemText);

            //        }


            //    }
            //    foreach (var item in preferred)
            //    {
            //        result.Requirements.PreferredQualifications.Add(item);
            //    }
            //}


            //if (qualifications != null)
            //{
            //    foreach (var item in qualifications.ChildNodes)
            //    {
            //        var itemText = item.InnerText;
            //        itemText = itemText.ToLower();
            //        var searchWord = "skill";

            //        if (itemText.Contains(searchWord))
            //        {
            //            result.Requirements.Skills.Add(itemText);
            //        }
            //    }
            //}


            return result;
    }
    }
}