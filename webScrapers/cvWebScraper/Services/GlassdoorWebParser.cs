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

        public Vacancy GetVacancy(string htmlContent)
        {
            var result = new Vacancy();

            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(htmlContent);

            var details = new Dictionary<string, string>();




            //var example =   "//div[@data-testid='inlineHeader-companyName']";
            var cityXpath = "//*[@data-test='location']";
            var companyXpath = "//*[@data-test='employer-name']//div[1]";
            var companyAddressXpath = "//span[@class='value website']//a";
            var jobDescriptionXpath = "//div[@id='JobDescriptionContainer']//*[string-length(text())>250]/parent::*";
            //var jobDescriptionXpath2 = "//div[contains(@id,'JobDesc')]//*//*";
            //var jobDescriptionXpath2 = "//div[@id='JobDescriptionContainer']//p[string-length(text())>150]";
            var jobTitleXpath = "//*[@data-test='job-title']";
            //var jobTypeXpath = "//div[@id='JobDescriptionContainer']//*[string-length(text())<20]";
            var jobTypeXpath2 = "//p | //div[not(@*)][string-length(text())<15]";
            var salaryXpath = "//*[@id='JobView']//*[contains(@class,'css-1v5elnn e11nt52q2')]/parent::div//div[4]//span";
            var salaryPeriodXpath = "//div[@data-test='salaryTabContent']//div[2]//div//span";
            


            var city=htmlDocument.DocumentNode.SelectSingleNode(cityXpath);
            result.JobDetails.City = city != null ? city.InnerText : "";

            

            
            var company = htmlDocument.DocumentNode.SelectSingleNode(companyXpath);
            result.JobDetails.Company = company != null ? company.InnerText : "";

            //Company tab needs to get clicked on
            var companyAddress = htmlDocument.DocumentNode.SelectSingleNode(companyAddressXpath);
            result.JobDetails.CompanyAddress = companyAddress != null ? companyAddress.Attributes["href"].Value : "";


            var jobTitle = htmlDocument.DocumentNode.SelectSingleNode(jobTitleXpath);
            result.JobDetails.JobTitle = jobTitle.InnerText;


            var jobDesciption = htmlDocument.DocumentNode.SelectSingleNode(jobDescriptionXpath);
            if (jobDesciption != null)
            {
                //foreach (var item in jobDesciption.ChildNodes)
                //{
                //    var section = item.InnerText;


                //}
                foreach (var item in jobDesciption.ChildNodes)
                {
                    var section = item.InnerText;
                    var jobTitleText = jobTitle.InnerText;


                    var jobTitleSearchWord = "";
                    var jobTitleSearchWordOriginal = "";
                    var index = jobTitleText.IndexOf(' ');
                    if (index>0)
                    {
                        
                        var a = 0;
                        jobTitleSearchWord =jobTitleText.Substring(0,index);
                        jobTitleSearchWordOriginal= jobTitleText.Substring(0, index);

                        jobTitleSearchWord =jobTitleSearchWord.ToLower();
                    }
                    else
                    {
                        jobTitleSearchWord = jobTitleText;
                        jobTitleSearchWordOriginal = jobTitleText;
                        jobTitleSearchWord=jobTitleSearchWord.ToLower();
                    }
                        if (section.Length > 150 && (section.Contains(jobTitleSearchWord) || section.Contains(jobTitleSearchWordOriginal)))
                    {

                        result.JobDetails.JobDescription = section;
                        break;
                    }
                }
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


            //result.JobDetails.JobType = jobType != null ? jobType.InnerText : "";



            //if (jobDesciption != null)
            //{
            //    var textFromJobDescription = "";
            //    foreach (var text in jobDesciption)
            //    {
            //        textFromJobDescription += text.InnerText;
            //    }

            //    result.JobDetails.JobDescription = textFromJobDescription;
            //}

            //var skillNodes = htmlDocument.DocumentNode.SelectNodes("/html/body/div[2]/div/div/div[1]/div[2]/div/div/div/div[1]/div/div[2]/div/div/ul[2]");
            //if (skillNodes != null)
            //{
            //    result.Requirements.Skills.AddRange(skillNodes.Select(c => c.InnerText));
            //}


            //var country = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div/div[1]/div[1]/div[2]/div/div/div[2]/div/div[1]/div[2]/div/div/div[3]/span");
            //result.JobDetails.Country = country != null ? country.InnerText : "";


            //var city = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div/div[1]/div[2]/div/div/div/div[1]/div/div[1]/div[1]/div");
            //result.JobDetails.City = city != null ? city.InnerText : "";










            //var salary = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div/div[1]/div[1]/div[2]/div/div/div[2]/div/div[1]/div[2]/div/div/div[4]/span/text()");



            //var averageSalary = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div/div[1]/div[2]/div/div/div/div[2]/div[1]");
            //if (averageSalary != null)
            //{
            //    var averageSalaryString = averageSalary.InnerText;
            //    var index = averageSalaryString.IndexOf("/");
            //    var index2 = averageSalaryString.IndexOf("(");
            //    var salaryPeriod = averageSalaryString.Substring(index + 1, index2 - index);
            //    result.JobDetails.SalaryPeriod = salaryPeriod;
            //}

            return result;
        }


    }
}