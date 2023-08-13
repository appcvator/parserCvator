using parser.Interfaces;
using parser.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace parser.Services
{
    public class UsaJobsWebParser : IWebParser
    {
        public string Key { get; } = "www.usajobs.gov";
        public string scrapeDescription(HtmlNode htmlNode, int keySectionLength)
        {
            string[] keywords = new string[] { "job", "description" };
            string[] keywords2 = new string[] { "role", "description" };
            string[] keywords3 = new string[] { "job", "summary" };
            string[] keywords4 = new string[] { "description" };
            string[] keywords5 = new string[] { "summary" };
            string[] keywords6 = new string[] { "role", "summary" };
            string thirdPreviousItemText = "";
            string secondPreviousItemText = "";
            string previousItemText = "";
            string itemText = "";
            foreach (var item in htmlNode.ChildNodes)
            {





                thirdPreviousItemText = secondPreviousItemText != null ? secondPreviousItemText : "";
                secondPreviousItemText = previousItemText != null ? previousItemText : "";
                previousItemText = itemText != null ? itemText : "";
                itemText = item.InnerText;

                if (itemText == string.Empty)
                {
                    continue;
                }
                if (previousItemText.Length < keySectionLength)
                {

                    previousItemText = previousItemText.ToLower();

                    if (
                        keywords.All(previousItemText.Contains) ||
                        keywords2.All(previousItemText.Contains) ||
                        keywords3.All(previousItemText.Contains) ||
                        keywords4.All(previousItemText.Contains) ||
                        keywords5.All(previousItemText.Contains) ||
                        keywords6.All(previousItemText.Contains))
                    {
                        return item.InnerText;
                    }
                }
                if (secondPreviousItemText.Length < keySectionLength)
                {

                    secondPreviousItemText = secondPreviousItemText.ToLower();

                    if (
                        keywords.All(secondPreviousItemText.Contains) ||
                        keywords2.All(secondPreviousItemText.Contains) ||
                        keywords3.All(secondPreviousItemText.Contains) ||
                        keywords4.All(secondPreviousItemText.Contains) ||
                        keywords5.All(secondPreviousItemText.Contains) ||
                        keywords6.All(secondPreviousItemText.Contains))
                    {



                        return item.InnerText;

                    }
                }
                if (thirdPreviousItemText.Length < keySectionLength)
                {

                    thirdPreviousItemText = thirdPreviousItemText.ToLower();

                    if (
                        keywords.All(thirdPreviousItemText.Contains) ||
                        keywords2.All(thirdPreviousItemText.Contains) ||
                        keywords3.All(thirdPreviousItemText.Contains) ||
                        keywords4.All(thirdPreviousItemText.Contains) ||
                        keywords5.All(thirdPreviousItemText.Contains) ||
                        keywords6.All(thirdPreviousItemText.Contains))
                    {



                        return item.InnerText;

                    }
                }




            }


            return "";
        }
        public HtmlNodeCollection? scrapeList(string[] keywords, string[] keywords2, string[] keywords3, string[] keywords4, HtmlNode htmlNode)
        {
            var keySectionLength = 60;
            string thirdPreviousItemText = "";
            string secondPreviousItemText = "";
            string previousItemText = "";
            string itemText = "";
            foreach (var item in htmlNode.ChildNodes)
            {

                thirdPreviousItemText = secondPreviousItemText;
                secondPreviousItemText = previousItemText;
                previousItemText = itemText;
                itemText = item.InnerText;
                if (item.ChildNodes.Count > 0)
                {

                    if (previousItemText.Length < keySectionLength)
                    {
                        previousItemText = previousItemText.ToLower();

                        if (keywords.All(previousItemText.Contains) ||

                        keywords2.All(previousItemText.Contains) ||

                        keywords3.All(previousItemText.Contains) ||

                        keywords4.All(previousItemText.Contains))

                        {
                            if (item.ChildNodes.Count < 2)
                            {
                                continue;
                            }
                            return item.ChildNodes;

                        }
                    }
                    if (secondPreviousItemText.Length < keySectionLength)
                    {
                        secondPreviousItemText = secondPreviousItemText.ToLower();

                        if (keywords.All(secondPreviousItemText.Contains) ||

                        keywords2.All(secondPreviousItemText.Contains) ||

                        keywords3.All(secondPreviousItemText.Contains) ||

                        keywords4.All(secondPreviousItemText.Contains))

                        {
                            if (item.ChildNodes.Count < 2)
                            {
                                continue;
                            }
                            return item.ChildNodes;

                        }
                    }
                    if (thirdPreviousItemText.Length < keySectionLength)
                    {
                        thirdPreviousItemText = thirdPreviousItemText.ToLower();

                        if (keywords.All(thirdPreviousItemText.Contains) ||

                        keywords2.All(thirdPreviousItemText.Contains) ||

                        keywords3.All(thirdPreviousItemText.Contains) ||

                        keywords4.All(thirdPreviousItemText.Contains))

                        {
                            if (item.ChildNodes.Count < 2)
                            {
                                continue;
                            }
                            return item.ChildNodes;

                        }
                    }
                }
            }
            return null;
        }
        public Vacancy GetVacancy(string htmlContent)
        {
            var result = new Vacancy();


            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(htmlContent);


            var details = new Dictionary<string, string>();


            //var example =   "//div[@data-testid='inlineHeader-companyName']";
            var cityXpath = "//*[@id='locations-list']//following::ul//span[contains(@class,'city')]";
            var companyXpath = "//*[contains(@class, 'hiring-organization')]";
            var jobDescriptionXpath = "//*[@id='summary']";
            var jobTitleXpath = "//*[@id='job-title']";
            var jobTypeXpath = "//text()[contains(., 'Work schedule')]//following::p[1]";
            var salaryXpath = "//*[contains(@class, 'salary')]";
            var postDateXpath = "//*[@itemprop='datePosted']";
            var responsibilitiesXpath = "//*[@id='duties']";
            var skillsXpath = "//*[@id='requirements']//*[@id='qualifications']";
            var educationXpath = "//*[@id='qualifications']//*[contains(text(),'degree') or contains(text(),'diploma')]";


            var education = htmlDocument.DocumentNode.SelectSingleNode(educationXpath);
            result.Requirements.Education = education != null ? education.InnerText.Trim() : "";


            var city = htmlDocument.DocumentNode.SelectSingleNode(cityXpath);
            if (city != null)
            {
                var cityText=city.InnerText.Trim();
                result.JobDetails.City = cityText;
            }


            var company = htmlDocument.DocumentNode.SelectSingleNode(companyXpath);
            if (company != null)
            {
                var companyText = company.InnerText.Trim();
                result.JobDetails.Company = companyText;
            }


            var jobTitle = htmlDocument.DocumentNode.SelectSingleNode(jobTitleXpath);
            result.JobDetails.JobTitle= jobTitle != null ? jobTitle.InnerText : string.Empty;
            if (jobTitle != null)
            {
                var jobTitleText = jobTitle.InnerText.Trim();
                result.JobDetails.JobTitle = jobTitleText;
            }


            var jobDescription = htmlDocument.DocumentNode.SelectSingleNode(jobDescriptionXpath);
            if(jobDescription!=null)
            {
                var jobDescriptionText = jobDescription.InnerText.Trim();
                result.JobDetails.JobDescription= jobDescriptionText;
            }


            var jobType = htmlDocument.DocumentNode.SelectSingleNode(jobTypeXpath);
            if (jobType != null)
            {
                var jobTypeText = jobType.InnerText.Trim();
                result.JobDetails.JobType = jobTypeText;
            }


            var postDate = htmlDocument.DocumentNode.SelectSingleNode(postDateXpath);
            if (postDate != null)
            {
                var postDateText=postDate.InnerText.Trim();
                var array=postDateText.Split('/');
                DateTime postDateFull = new DateTime(int.Parse(array[2]), int.Parse(array[0]), int.Parse(array[1]));
               
                result.JobDetails.PostDate = postDateFull;
            }


            var salary = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[1]/div[2]/section/section/div/div/aside/div[2]/ul[2]/li[2]/p[1]");
            if (salary != null)
            {
                var salaryText = salary.InnerText.Trim();
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


            var responsibilities = htmlDocument.DocumentNode.SelectSingleNode(responsibilitiesXpath);
            if (responsibilities != null)
            {
                foreach (var childNode in responsibilities.ChildNodes)
                {
                    
                    result.Responsibilities.Add(childNode.InnerText.Trim());
                }
            }


            var skills = htmlDocument.DocumentNode.SelectSingleNode(skillsXpath);
            if (skills != null)
            {
                foreach (var childNode in skills.ChildNodes)
                {
                    result.Requirements.Skills.Add(childNode.InnerText.Trim());
                }
            }

            return result;
        }


    }
}