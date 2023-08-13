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
    public class MonsterWebParser: IWebParser
    {
        public string Key { get; } = "www.monster.com";
        public HtmlNodeCollection? scrapeList(string[] keywords, string[] keywords2, string[] keywords3, string[]keywords4, HtmlNode htmlNode)
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
        public Vacancy GetVacancy(string htmlContent)
        {
            var result = new Vacancy();


            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(htmlContent);


            var details = new Dictionary<string, string>();


            var cityXpath = "//h3";
            var companyXpath = "//h2";
            var jobDescriptionXpath = "//*[@class='descriptionstyles__DescriptionContainer-sc-13ve12b-0 iCEVUR']//*[string-length(text())>150]/parent::*";
            var jobTitleXpath = "//h1";
            var jobTypeXpath = "//*[@data-test-id='svx-jobview-employmenttype-or-hq-header']//following::*";
            var postDateXpath = "//*[@data-test-id='svx-jobview-posted']";
            var educationXpath = jobDescriptionXpath+ "//*[contains(text(),'degree') or contains(text(),'diploma')]";
            var experienceXpath =jobDescriptionXpath+ "//*[contains(text(),'experience') and contains(text(),'years')]";
            var preferredXpath = jobDescriptionXpath + "//*[contains(text(),'preferred')]";

            var education = htmlDocument.DocumentNode.SelectSingleNode(educationXpath);
            result.Requirements.Education = education != null ? education.InnerText : "";


            var preferred = htmlDocument.DocumentNode.SelectSingleNode(preferredXpath);
            if (preferred != null)
            {
                result.Requirements.PreferredQualifications.Add(preferred.InnerText);
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
            result.JobDetails.City = city != null ? city.InnerText : "";


            var company = htmlDocument.DocumentNode.SelectSingleNode(companyXpath);
            result.JobDetails.Company = company != null ? company.InnerText : "";


            var jobType = htmlDocument.DocumentNode.SelectSingleNode(jobTypeXpath);
            result.JobDetails.JobType = jobType != null ? jobType.InnerText : "";




            var jobDescription = htmlDocument.DocumentNode.SelectSingleNode(jobDescriptionXpath);
            
            int keySectionLength = 30;
            if (jobDescription != null)
            {
                result.JobDetails.JobDescription = scrapeDescription( jobDescription, keySectionLength);

            }


            var jobTitle = htmlDocument.DocumentNode.SelectSingleNode(jobTitleXpath);
            result.JobDetails.JobTitle= jobTitle != null ? jobTitle.InnerText:"";


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


            var responsibilities = htmlDocument.DocumentNode.SelectSingleNode(jobDescriptionXpath);
            string[] responsibilityKeywords = new string[] { "responsibilities" };
            string[] responsibilityKeywords2 = new string[] { "duties" };
            string[] responsibilityKeywords3 = new string[] { "(responsibilities)" };
            string[] responsibilityKeywords4 = responsibilityKeywords3;
            if (responsibilities != null)
            {
                var collection=scrapeList(responsibilityKeywords, responsibilityKeywords2, responsibilityKeywords3,responsibilityKeywords4, responsibilities);
                if (collection != null)
                {
                    result.Responsibilities.AddRange(collection.Select(x => x.InnerText));
                }
            }


            var skills= htmlDocument.DocumentNode.SelectSingleNode(jobDescriptionXpath);
            string[] skillsKeywords = new string[] { "required", "skills" };
            string[] skillsKeywords2 = new string[] { "need", "you" };
            string[] skillsKeywords3 = new string[] { "qualifications" };
            string[] skillsKeywords4 = new string[] { "requirements" };
            
            if (skills!=null)
            {
                var collection=scrapeList(skillsKeywords, skillsKeywords2, skillsKeywords3,skillsKeywords4, skills);
                if (collection != null)
                {
                    result.Requirements.Skills.AddRange(collection.Select(x => x.InnerText));

                }
            }


            return result;
        }


    }
}