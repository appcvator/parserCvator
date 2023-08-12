using parser.Interfaces;
using parser.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace parser.Services
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


            var cityXpath = "//title";
            var companyXpath = "//title";
            //var companyAddressXpath = "";
            var jobDescriptionXpath = "";
            var jobTitleXpath = "//title";
            var jobTypeXpath = "//span[string-length(text())<24 and string-length(text())>8]";
            var salaryMaxMinXpath = "//*[@class='min-max-sal']";
            var postDateXpath = "";



            var city = htmlDocument.DocumentNode.SelectSingleNode(jobTitleXpath);
            if(city!=null)
            {
                var cityText = city.InnerText;
                var amount = 0;


                var splitIndex=cityText.IndexOf('-');
                while (splitIndex>=0)
                {
                    amount++;
                    splitIndex=cityText.IndexOf("-",splitIndex+1);
                }

                if (amount == 4)
                {
                    var index = cityText.IndexOf('-');
                    var index2 = cityText.IndexOf('-',index+1);
                    var index3 = cityText.IndexOf('-', index2 + 1);
                    cityText = cityText.Substring(index2 + 1, index3 - index2 - 1);
                    
                }
                else
                {



                    var index = cityText.IndexOf('-');
                    var index2 = cityText.IndexOf('-', index + 1);
                    cityText = cityText.Substring(index + 1, index2 - index - 1);
                    
                }
                result.JobDetails.City = cityText;

            }


            var company = htmlDocument.DocumentNode.SelectSingleNode(jobTitleXpath);
            result.JobDetails.Company = company != null ? company.InnerText : string.Empty;
            if (company != null)
            {
                var companyText=company.InnerText;


                var amount = 0;
                var splitIndex = companyText.IndexOf('-');
                while (splitIndex >= 0)
                {
                    amount++;
                    splitIndex = companyText.IndexOf("-", splitIndex + 1);
                }

                if (amount == 4)
                {
                    var index = companyText.IndexOf('-');
                    var index2 = companyText.IndexOf('-', index + 1);
                    var index3 = companyText.IndexOf('-', index2 + 1);
                    var index4 = companyText.IndexOf('-', index3 + 1);
                    companyText = companyText.Substring(index3 + 1, index4 - index3 - 1);
                    
                }
                else
                {


                    var index = companyText.IndexOf('-');
                    var index2 = companyText.IndexOf('-', index + 1);
                    var index3 = companyText.IndexOf('-', index2 + 1);
                    companyText = companyText.Substring(index2 + 1, index3 - index2 - 1);
                }
                result.JobDetails.Company = companyText;
                
            }



            var jobTitle = htmlDocument.DocumentNode.SelectSingleNode(jobTitleXpath);
            if (jobTitle != null)
            {
                var jobTitleText = jobTitle.InnerText;


                var amount = 0;
                var splitIndex = jobTitleText.IndexOf('-');
                while (splitIndex >= 0)
                {
                    amount++;
                    splitIndex = jobTitleText.IndexOf("-", splitIndex + 1);
                }

                if (amount == 4)
                {
                    var index = jobTitleText.IndexOf('-');
                    var index2 = jobTitleText.IndexOf('-', index + 1);
                    jobTitleText = jobTitleText.Substring(0,index2-1);
                }
                else
                {
                    var index = jobTitleText.IndexOf('-');
                    jobTitleText = jobTitleText.Substring(0, index - 1);

                }
                
                
                result.JobDetails.JobTitle = jobTitleText;
            }


            var jobType = htmlDocument.DocumentNode.SelectNodes(jobTypeXpath);
            string[] availableJobTypes = new string[] { "fulltime", "parttime", "contract" };
            if (jobType!=null)
            {
                Regex reg = new Regex("[^a-zA-Z']");
                foreach (var item in jobType)
                {
                    var itemText=item.InnerText;
                    if (itemText.Length > 20)
                    {
                        continue;
                    }
                    var processedItem = reg.Replace(itemText, string.Empty);
                    processedItem = processedItem.ToLower();
                    if (availableJobTypes.Any(processedItem.Contains))
                    {
                        result.JobDetails.JobType = processedItem;
                        break;
                    }
                }


            }


            var salaryMaxMin = htmlDocument.DocumentNode.SelectSingleNode(salaryMaxMinXpath);
            if (salaryMaxMin != null)
            {

                var salaryMin = "";
                var salaryMax = "";
                var count = 0;
                foreach (var item in salaryMaxMin.ChildNodes)
                { 
                    if (count == 0)
                    {
                        salaryMin = item.InnerText;
                    }
                    else
                    {
                        salaryMax = item.InnerText;
                    }
                    count++;
                }


                var salaryPeriod = salaryMax.Substring(salaryMax.LastIndexOf(" "));
                salaryPeriod = salaryPeriod.Trim();


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


            //var postDate = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"root\"]/main/div[2]/div[1]/section[1]/div[2]/div[1]/span[1]/span");
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
            //var jobDescription = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"root\"]/main/div[2]/div[1]/section[2]/div[1]/ul[1]");
            //if (jobDescription != null)
            //{

            //    foreach (var childNode in jobDescription.ChildNodes)
            //    {
            //        result.JobDetails.JobDescription += childNode.InnerText;
            //    }
            //}


            //var preferred = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"root\"]/main/div[2]/div[1]/section[2]/div[1]/p[2]/strong[2]");
            //if (preferred != null)
            //{
            //    result.Requirements.PreferredQualifications.Add(preferred.InnerText);
            //}


            //var skills = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"root\"]/main/div[2]/div[1]/section[2]/div[1]/ul[2]/li");
            //if (skills != null)
            //{
            //    result.Requirements.Skills.Add(skills.InnerText);
            //}


            //var education = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"root\"]/main/div[2]/div[1]/section[2]/div[3]");
            //if (education != null)
            //{
            //    result.Requirements.Education=education.InnerText;
               
            //}


            
            return result;
        }


    }
}