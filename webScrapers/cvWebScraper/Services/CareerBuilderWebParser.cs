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
    public class CareerBuilderWebParser : IWebParser
    {
        public string Key { get; } = "www.careerbuilder.co.uk";
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


            var cityXpath = "//div[@id='jdp-data']//div[@class='data-details']";
            var companyXpath = "//div[@id='jdp-data']//div[@class='data-details']";
            //var companyAddressXpath = "";
            var jobDescriptionXpath = "//*[@id='jdp_description']//*[string-length(text())>50]/parent::*";
            var jobTitleXpath = "//div[@id='jdp-data']//div[@class='data-details']//..//*";
            var jobTypeXpath = "//div[@id='jdp-data']//div[@class='data-details']";
            var salaryXpath = "//*[@id='cb-salcom-info']//*";
            var skillsXpath = "//*[contains(@class,'jdp-required-skills')]";
            var experienceXpath = jobDescriptionXpath + "//*[contains(text(),'experience') and contains(text(),'years')]";



            var company = htmlDocument.DocumentNode.SelectSingleNode(companyXpath);
            result.JobDetails.Company = company != null ? company.InnerText : string.Empty;
            if (company != null)
            {
                
                foreach (var item in company.ChildNodes)
                {
                    if(item.InnerText.Length<3)
                    {
                        continue;
                    }
                    result.JobDetails.Company = item.InnerText;
                    break;
                }
            }

            var jobTitle = htmlDocument.DocumentNode.SelectSingleNode(jobTitleXpath);
            result.JobDetails.JobTitle=jobTitle !=null ? jobTitle.InnerText : string.Empty;


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
            var city=htmlDocument.DocumentNode.SelectSingleNode(cityXpath);
            if (city != null)
            {
                var count = 0;
                foreach (var item in city.ChildNodes)
                {
                    if (item.InnerText.Length < 3)
                    {
                        continue;
                    }
                    if (count == 1)
                    {
                        result.JobDetails.City = item.InnerText;
                        break;
                    }

                    count++;
                }
            }


            var jobType = htmlDocument.DocumentNode.SelectSingleNode(jobTypeXpath);
            if (jobType != null)
            {
                var count = 0;
                foreach(var item in jobType.ChildNodes)
                {
                    if (item.InnerText.Length < 3)
                    {
                        continue;
                    }
                    if (count ==2)
                    {
                        result.JobDetails.JobType= item.InnerText;
                        break;
                    }
                    
                    count++;
                }
            }


            var salary = htmlDocument.DocumentNode.SelectSingleNode(salaryXpath);
            if (salary != null)
            {
                var salaryText = salary.InnerText;
                salaryText = salaryText.ToLower();

                
                var salaryPeriod =salaryText.Substring(salaryText.IndexOf("/")+1);


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


            var skills = htmlDocument.DocumentNode.SelectSingleNode(skillsXpath);
            if(skills!=null)
            {
                result.Requirements.Skills.Add(skills.InnerText);
            }


            var experience = htmlDocument.DocumentNode.SelectSingleNode(experienceXpath);
            if (experience != null)
            {
                var experienceText = experience.InnerText;
                result.Requirements.Experience.Description = experienceText;

                if (experienceText.IndexOf('-')>0)
                {
                    var index=experienceText.IndexOf('-');
                    var substring=experienceText.Substring(0, index);
                    var substring2=experienceText.Substring(index+1);
                    var years = Regex.Replace(substring, "[^0-9.]", "");
                    var years2= Regex.Replace(substring2, "[^0-9.]", "");
                    result.Requirements.Experience.Years = years+'-'+years2;
                }
                else
                {
                    var years = Regex.Replace(experienceText, "[^0-9.]", "");
                    result.Requirements.Experience.Years = years;
                }
            }


            return result;
        }


    }
}