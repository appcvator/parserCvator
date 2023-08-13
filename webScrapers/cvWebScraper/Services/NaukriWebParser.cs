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


            var cityXpath = "//title";
            var companyXpath = "//title";
            //var companyAddressXpath = "";
            var jobDescriptionXpath = "//text()[contains(., 'Job description')]//following::*";
            var jobTitleXpath = "//title";
            var jobTypeXpath = "//span[string-length(text())<24 and string-length(text())>8]";
            var salaryMaxMinXpath = "//*[@class='min-max-sal']";
            var postDateXpath = "//*[contains(text(),'Posted:')]//following::*";
            var preferredXpath = "//*[contains(text(),'Key Skills')]/following::div[2]";
            var educationXpath = "//div[.= 'Education']";
            var skillsXpath = "//*[contains(text(),'Key Skills')]/following::div[3]";


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
                    jobTitleText = jobTitleText.Substring(0, index2 - 1);
                }
                else
                {
                    var index = jobTitleText.IndexOf('-');
                    jobTitleText = jobTitleText.Substring(0, index - 1);

                }


                result.JobDetails.JobTitle = jobTitleText;
            }


            var jobDescription = htmlDocument.DocumentNode.SelectSingleNode(jobDescriptionXpath);
            if(jobDescription!=null)
            {
                result.JobDetails.JobDescription=scrapeDescription2(jobDescription, jobTitle);
            }


            var education = htmlDocument.DocumentNode.SelectSingleNode(educationXpath);
            if(education!=null)
            {
                foreach(var item in education.ChildNodes)
                {
                    result.Requirements.Education = item.InnerText;
                }
            }


            var preferred = htmlDocument.DocumentNode.SelectNodes(preferredXpath);
            if(preferred!=null)
            {
                result.Requirements.PreferredQualifications.AddRange(preferred.Select(x=>x.InnerText));
            }


            var city = htmlDocument.DocumentNode.SelectSingleNode(jobTitleXpath);
            if(city!=null)
            {
                var cityText = city.InnerText.Trim();
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
                var companyText=company.InnerText.Trim();


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


            var skills = htmlDocument.DocumentNode.SelectNodes(skillsXpath);


            if (skills != null)
            {
                result.Requirements.Skills.AddRange(skills.Select(x => x.InnerText));

 
            }


            return result;
        }


    }
}