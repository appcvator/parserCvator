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
        public string scrapeDescription2(HtmlNode jobDescription, HtmlNode jobTitle)
        {
            int keySectionLength = 50;
            string jobTitleText = jobTitle.InnerText;
            var jobTitleTextTrimmed = jobTitleText.Trim();
            string[] stringOfWords = jobTitleTextTrimmed.Split(' ');
            var stringOfWordsSorted = stringOfWords.OrderBy(aux => aux.Length).ToArray();


            foreach (var item in jobDescription.ChildNodes)
            {
                var itemText = item.InnerText;
                if (itemText.Length > keySectionLength && (stringOfWordsSorted.Any(itemText.Contains)))
                {
                    return itemText;
                }
            }
            return "";
        }
        public Vacancy GetVacancy(string htmlContent)
        {
            var result = new Vacancy();


            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(htmlContent);


            var details = new Dictionary<string, string>();


            var cityXpath = "//*[@data-testid='viewJobCompanyLocation']";
            var companyXpath = "//*[@data-testid='viewJobCompanyName']";
            var jobTitleXpath = "//*[@data-testid='viewJobTitle']";
            var jobTypeXpath = "//*[@data-testid='viewJobBodyJobDetailsJobType']//*[2]";
            var salaryXpath = "//*[@data-testid='viewJobBodyJobCompensation']";
            var jobDescriptionXpath = "//*[@data-testid='viewJobBodyJobFullDescriptionContent']";
            var postDateXpath = "//*[@data-testid='viewJobBodyJobPostingTimestamp']";
            var educationXpath = "//span[contains(text(),'degree') or contains(text(),'diploma')]";


            var education = htmlDocument.DocumentNode.SelectSingleNode(educationXpath);
            result.Requirements.Education = education != null ? education.InnerText.Trim() : "";


            var jobType = htmlDocument.DocumentNode.SelectSingleNode(jobTypeXpath);
            result.JobDetails.JobType = jobType != null ? jobType.InnerText : string.Empty;


            var city = htmlDocument.DocumentNode.SelectSingleNode(cityXpath);
            result.JobDetails.City = city != null ? city.InnerText : string.Empty;


            var company = htmlDocument.DocumentNode.SelectSingleNode(companyXpath);
            result.JobDetails.Company = company != null ? company.InnerText : string.Empty;


            var jobTitle = htmlDocument.DocumentNode.SelectSingleNode(jobTitleXpath);
            result.JobDetails.JobTitle = jobTitle != null ? jobTitle.InnerText : string.Empty;


            var posted = htmlDocument.DocumentNode.SelectSingleNode(postDateXpath);
            if (posted != null)
            {
                var postedText = posted.InnerText;


                postedText = postedText.Trim();
                DateTime currentTime = DateTime.Now;
                TimeSpan timespan = TimeSpan.Zero;
                if (postedText.Contains("minutes"))
                {
                    var postDateProcessed = string.Empty;
                    foreach (var c in postedText)
                    {
                        int ascii = (int)c;
                        if ((ascii >= 48 && ascii <= 57) || ascii == 44 || ascii == 46)
                            postDateProcessed += c;
                    }
                    int minutes = int.Parse(postDateProcessed);
                    timespan = new TimeSpan(0, minutes, 0);
                }
                else if (postedText.Contains("hours"))
                {
                    var postDateProcessed = string.Empty;
                    foreach (var c in postedText)
                    {
                        int ascii = (int)c;
                        if ((ascii >= 48 && ascii <= 57) || ascii == 44 || ascii == 46)
                            postDateProcessed += c;
                    }
                    int hours = int.Parse(postDateProcessed);
                    timespan = new TimeSpan(hours, 0, 0);
                }
                else if (postedText.Contains("days"))
                {
                    var postDateProcessed = string.Empty;
                    foreach (var c in postedText)
                    {
                        int ascii = (int)c;
                        if ((ascii >= 48 && ascii <= 57) || ascii == 44 || ascii == 46)
                            postDateProcessed += c;
                    }
                    int days = int.Parse(postDateProcessed);
                    timespan = new TimeSpan(days, 0, 0, 0);
                }
                else if (postedText.Contains("week"))
                {
                    var postDateProcessed = string.Empty;
                    foreach (var c in postedText)
                    {
                        int ascii = (int)c;
                        if ((ascii >= 48 && ascii <= 57) || ascii == 44 || ascii == 46)
                            postDateProcessed += c;
                    }
                    int weeks = int.Parse(postDateProcessed);
                    int days = weeks * 7;
                    timespan = new TimeSpan(days, 0, 0, 0);
                }
                else if (postedText.Contains("month"))
                {
                    var postDateProcessed = string.Empty;
                    foreach (var c in postedText)
                    {
                        int ascii = (int)c;
                        if ((ascii >= 48 && ascii <= 57) || ascii == 44 || ascii == 46)
                            postDateProcessed += c;
                    }
                    int months = int.Parse(postDateProcessed);
                    int days = months * 30;
                    timespan = new TimeSpan(days, 0, 0, 0);
                }
                else
                {

                }
                var postedResult = currentTime.Subtract(timespan);
                result.JobDetails.PostDate = postedResult;

            }


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


            var jobDescription = htmlDocument.DocumentNode.SelectSingleNode(jobDescriptionXpath);

            int keySectionLength = 30;
            if (jobDescription != null)
            {
                result.JobDetails.JobDescription = scrapeDescription2(jobDescription,jobTitle);

            }


            var responsibilities = htmlDocument.DocumentNode.SelectSingleNode(jobDescriptionXpath);
            string[] responsibilityKeywords = new string[] { "responsibilities" };
            string[] responsibilityKeywords2 = new string[] { "duties" };
            string[] responsibilityKeywords3 = new string[] { "(responsibilities)" };
            string[] responsibilityKeywords4 = responsibilityKeywords3;
            if (responsibilities != null)
            {
                var collection = scrapeList(responsibilityKeywords, responsibilityKeywords2, responsibilityKeywords3, responsibilityKeywords4, responsibilities);
                if (collection != null)
                {
                    result.Responsibilities.AddRange(collection.Select(x => x.InnerText));
                }
            }


            var skills = htmlDocument.DocumentNode.SelectSingleNode(jobDescriptionXpath);
            string[] skillsKeywords = new string[] { "required", "skills" };
            string[] skillsKeywords2 = new string[] { "need", "you" };
            string[] skillsKeywords3 = new string[] { "qualifications" };
            string[] skillsKeywords4 = new string[] { "requirements" };

            if (skills != null)
            {
                var collection = scrapeList(skillsKeywords, skillsKeywords2, skillsKeywords3, skillsKeywords4, skills);
                if (collection != null)
                {
                    result.Requirements.Skills.AddRange(collection.Select(x => x.InnerText));

                }
            }
            

            return result;
    }
    }
}