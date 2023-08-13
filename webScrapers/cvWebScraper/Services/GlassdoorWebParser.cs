using parser.Interfaces;
using parser.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;






namespace parser.Services
{
    public class GlassdoorWebParser : IWebParser
    {
        public string Key { get; } = "www.glassdoor.com";
        
        public HtmlNodeCollection? scrapeList(string[] keywords, string[] keywords2, string[] keywords3, string[] keywords4,HtmlNode htmlNode)
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

                    if (previousItemText.Length<keySectionLength)
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
        public string scrapeDescription( HtmlNode htmlNode)
        {
            var keySectionLength = 30;
            string[] keywords = new string[] { "job", "description" };
            string[] keywords2 = new string[] { "role", "description" };
            string[] keywords3 = new string[] { "job", "summary" };
            string[] keywords4 = new string[] { "description" };
            string[] keywords5 = new string[] { "summary" };
            string[] keywords6 = new string[] { "role", "summary" };
            string[] keywords7 = new string[] { "role", "about" };
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
                        keywords6.All(previousItemText.Contains) ||
                        keywords7.All(previousItemText.Contains))
                    {



                        return item.InnerText;

                    }
                }
                if (secondPreviousItemText.Length < keySectionLength)
                {

                    secondPreviousItemText = secondPreviousItemText.ToLower();

                    if (
                        keywords.All(previousItemText.Contains) ||
                        keywords2.All(previousItemText.Contains) ||
                        keywords3.All(previousItemText.Contains) ||
                        keywords4.All(previousItemText.Contains) ||
                        keywords5.All(previousItemText.Contains) ||
                        keywords6.All(previousItemText.Contains) ||
                        keywords7.All(previousItemText.Contains))
                    {



                        return item.InnerText;

                    }
                }
                if (thirdPreviousItemText.Length < keySectionLength)
                {

                    thirdPreviousItemText = thirdPreviousItemText.ToLower();

                    if (
                        keywords.All(previousItemText.Contains) ||
                        keywords2.All(previousItemText.Contains) ||
                        keywords3.All(previousItemText.Contains) ||
                        keywords4.All(previousItemText.Contains) ||
                        keywords5.All(previousItemText.Contains) ||
                        keywords6.All(previousItemText.Contains) ||
                        keywords7.All(previousItemText.Contains))
                    {

                        return item.InnerText;

                    }
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




            
            var cityXpath = "//*[@data-test='location']";
            var companyXpath = "//*[@data-test='employer-name']//div[1]";
            var companyAddressXpath = "//span[@class='value website']//a";
            var jobDescriptionXpath = "//div[@id='JobDescriptionContainer']//*[string-length(text())>150]/parent::*";
            //var jobDescriptionXpath2 = "//div[contains(@id,'JobDesc')]//*//*";
            //var jobDescriptionXpath2 = "//div[@id='JobDescriptionContainer']//p[string-length(text())>150]";
            var jobTitleXpath = "//*[@data-test='job-title']";
            //var jobTypeXpath = "//div[@id='JobDescriptionContainer']//*[string-length(text())<20]";
            var jobTypeXpath2 = "//p | //div[not(@*)][string-length(text())<15]";
            var salaryXpath = "//*[@id='JobView']//*[contains(@class,'css-1v5elnn e11nt52q2')]/parent::div//div[4]//span";
            var salaryPeriodXpath = "//div[@data-test='salaryTabContent']//div[2]//div//span";
            var educationXpath = "//div[@id='JobDescriptionContainer']//*[string-length(text())>150]/parent::*//*[contains(text(),'degree') or contains(text(),'diploma')]";


            var education = htmlDocument.DocumentNode.SelectSingleNode(educationXpath);
            result.Requirements.Education = education != null ? education.InnerText : "";


            var city=htmlDocument.DocumentNode.SelectSingleNode(cityXpath);
            result.JobDetails.City = city != null ? city.InnerText : "";

            
            var company = htmlDocument.DocumentNode.SelectSingleNode(companyXpath);
            result.JobDetails.Company = company != null ? company.InnerText : "";


            //нужно открыть вкладку company
            var companyAddress = htmlDocument.DocumentNode.SelectSingleNode(companyAddressXpath);
            result.JobDetails.CompanyAddress = companyAddress != null ? companyAddress.Attributes["href"].Value : "";


            var jobTitle = htmlDocument.DocumentNode.SelectSingleNode(jobTitleXpath);
            result.JobDetails.JobTitle = jobTitle.InnerText;


            var jobDescription = htmlDocument.DocumentNode.SelectSingleNode(jobDescriptionXpath);
  
            
            if (jobDescription != null)
            {
                var description=scrapeDescription(jobDescription);


                result.JobDetails.JobDescription= description;
            }


            var jobType = htmlDocument.DocumentNode.SelectNodes(jobTypeXpath2);
            string[] availableJobTypes = new string[] { "fulltime", "parttime", "contract"};
            if(jobType != null)
            {
                Regex reg = new Regex("[^a-zA-Z']");
                foreach (var item in jobType)
                {
                    if(item.InnerText.Length>20)
                    {
                        continue;
                    }
                    var processedItem= reg.Replace(item.InnerText, string.Empty);
                    processedItem=processedItem.ToLower();
                    if (availableJobTypes.Any(processedItem.Contains))
                    {
                        result.JobDetails.JobType= processedItem;
                        break;
                    }
                }
            }


            var salary = htmlDocument.DocumentNode.SelectSingleNode(salaryXpath);
            if (salary != null)
            {
                var salaryString = salary.InnerText;
                var index = salaryString.IndexOf("-");
                var salaryMin = salaryString.Substring(0, index);
                var salaryMax = salaryString.Substring(index);


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
                
                salaryMinProcessed = Regex.Replace(salaryMinProcessed, "[^0-9.]", "");
                if (salaryMinProcessed.Contains('.'))
                {
                    salaryMinProcessed=salaryMinProcessed.Substring(salaryMinProcessed.IndexOf('.')+1);
                }
                if (salaryMinProcessed.Contains('.'))
                {
                    salaryMinProcessed = salaryMinProcessed.Substring(0, salaryMinProcessed.IndexOf("."));
                }


                var salaryMaxProcessed = string.Empty;
                foreach (var c in salaryMax)
                {
                    int ascii = (int)c;
                    if ((ascii >= 48 && ascii <= 57) || ascii == 44 || ascii == 46)
                        salaryMaxProcessed += c;
                }
                salaryMinProcessed.Replace(".", "");
                if (salaryMaxProcessed.Contains('.'))
                {
                    salaryMaxProcessed = salaryMaxProcessed.Substring(0,salaryMaxProcessed.IndexOf("."));
                }

                result.JobDetails.SalaryMin = Decimal.Parse(salaryMinProcessed);
                result.JobDetails.SalaryMax = Decimal.Parse(salaryMaxProcessed);
            }


            var salaryPeriod = htmlDocument.DocumentNode.SelectSingleNode(salaryPeriodXpath);
           
            if (salaryPeriod != null)
            {
                var salaryPeriodText = salaryPeriod.InnerText;
                var index = salaryPeriodText.IndexOf("/");
                salaryPeriodText = salaryPeriodText.Substring(index + 1);
                result.JobDetails.SalaryPeriod = salaryPeriodText;
            }

            var skills = htmlDocument.DocumentNode.SelectSingleNode(jobDescriptionXpath);
            string[] skillsKeywords = new string[] { "required", "skills" };
            string[] skillsKeywords2 = new string[] { "you", "need" };
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

            var responsibilities = htmlDocument.DocumentNode.SelectSingleNode(jobDescriptionXpath);
            string[] responsibilityKeywords = new string[] { "responsibilities" };
            string[] responsibilityKeywords2 = new string[] { "duties" };
            string[] responsibilityKeywords3 = new string[] { "what", "do" };
            string[] responsibilityKeywords4 = responsibilityKeywords3;
            if (responsibilities != null)
            {
                
                var collection=scrapeList(responsibilityKeywords, responsibilityKeywords2, responsibilityKeywords3,responsibilityKeywords4, responsibilities);
                if (collection != null)
                {
                    result.Responsibilities.AddRange(collection.Select(x => x.InnerText));
                }
            }
            
            
            return result;
        }


    }
}