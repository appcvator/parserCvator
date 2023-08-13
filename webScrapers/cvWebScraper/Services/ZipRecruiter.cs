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
            var jobTitleTextTrimmed=jobTitleText.Trim();
            string[] stringOfWords = jobTitleTextTrimmed.Split(' ');
            var stringOfWordsSorted=stringOfWords.OrderBy(aux => aux.Length).ToArray();


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

            var cityXpath = "//*[contains(@class,'_location')]";
            var companyXpath = "//*[contains(@class,'_company')]";
            var jobDescriptionXpath = "//*[contains(@class,'description')]//*[string-length(text())>50]/parent::*";
            var jobTitleXpath = "//*[contains(@class,'job_title')]";
            var jobTypeXpath = "//*[contains(@class,'employment_type')]";
            var salaryXpath = "//*[contains(@class,'compensation')]//span";
            var postDateXpath = "//*[@id='report-job-app-root']/following::*/descendant::*[contains(text(),'date')]//following::*";
            var experienceXpath = jobDescriptionXpath+ "//*[contains(text(),'experience') and contains(text(),'years')]";
            var educationXpath = jobDescriptionXpath+ "//*[contains(text(),'degree') or contains(text(),'diploma')]";
            


            var education = htmlDocument.DocumentNode.SelectSingleNode(educationXpath);
            result.Requirements.Education = education != null ? education.InnerText : "";


            var preferred = htmlDocument.DocumentNode.SelectSingleNode(jobDescriptionXpath);
            string[] preferredKeywords = new string[] { "preferred","skills" };
            string[] preferredKeywords2 = new string[] { "nice", "haves" };
            string[] preferredKeywords3 = preferredKeywords2;
            string[] preferredKeywords4 = preferredKeywords3;
            if (preferred != null)
            {
                var collection = scrapeList(preferredKeywords, preferredKeywords2, preferredKeywords3, preferredKeywords4, preferred);
                if (collection != null)
                {
                    result.Requirements.PreferredQualifications.AddRange(collection.Select(x => x.InnerText));

                }
            }


            var experience = htmlDocument.DocumentNode.SelectSingleNode(experienceXpath);
            if (experience != null)
            {
                var experienceText = experience.InnerText;
                result.Requirements.Experience.Description = experienceText;


                var years = Regex.Replace(experienceText, "[^0-9.]", "");
                result.Requirements.Experience.Years = years;
            }


            var city = htmlDocument.DocumentNode.SelectSingleNode(cityXpath);
            if (city != null)
            {
                result.JobDetails.City = city.InnerText.Trim();

            }

            var jobTitle = htmlDocument.DocumentNode.SelectSingleNode(jobTitleXpath);
            if (jobTitle != null)
            {
                var jobTitleText = jobTitle.InnerText.Trim();
                result.JobDetails.JobTitle = jobTitleText;
            }


            var jobDescription = htmlDocument.DocumentNode.SelectSingleNode(jobDescriptionXpath);
            if (jobDescription != null)
            {
                result.JobDetails.JobDescription=scrapeDescription2(jobDescription, jobTitle);

            }



            var jobType = htmlDocument.DocumentNode.SelectSingleNode(jobTypeXpath);
            if(jobType != null)
            {
                var jobTypeProcessed=jobType.InnerText.Trim();
                result.JobDetails.JobType = jobTypeProcessed;
            }


            var postDate = htmlDocument.DocumentNode.SelectSingleNode(postDateXpath);
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


            var salary = htmlDocument.DocumentNode.SelectSingleNode(salaryXpath);
            if(salary !=null)
            {
                var salaryText=salary.InnerText;

                
                var index2 = salaryText.LastIndexOf(' ');
                var salaryPeriod = salaryText.Substring(index2 + 1);


                var index = salaryText.IndexOf("to");


                var salaryMin = "";
                var salaryMax = "";


                if (index < 0)
                {
                    salaryMin = salaryText;
                    salaryMax = salaryText;
                }
                else
                {
                    salaryMin = salaryText.Substring(0, index);
                    salaryMax = salaryText.Substring(index);
                }


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
            if(company!=null)
            {
                result.JobDetails.Company=company.InnerText.Trim();
                
            }


            var responsibilities = htmlDocument.DocumentNode.SelectSingleNode(jobDescriptionXpath);
            string[] responsibilityKeywords = new string[] { "responsibilities" };
            string[] responsibilityKeywords2 = new string[] { "duties" };
            string[] responsibilityKeywords3 = new string[] { "what", "do" };
            string[] responsibilityKeywords4 = new string[] { "(responsibilities)" };
            
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