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
    public class CareerBuilderWebParser : IWebParser
    {
        public string Key { get; } = "www.careerbuilder.co.uk";

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
            var postDateXpath = "";


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


            //var responsibilities = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"jdp_description\"]/div[1]/div[1]/p[1]");
            //if (responsibilities != null)
            //{
            //    var text = responsibilities.InnerText;
            //    text = text.ToLower();
            //    var index = text.IndexOf("duties");
            //    var index2 = text.IndexOf("who");
            //    text = text.Substring( index,index2-index-1);
            //    result.Responsibilities.Add( text);
            //}


            //var skills = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"jdp_description\"]/div[1]/div[1]/p[1]");
            //if(skills!=null)
            //{
            //    var text = skills.InnerText;
            //    var search = "Who we're looking for";
            //    var index=text.IndexOf(search);
            //    var index2 = text.IndexOf("Benefits");
            //    text = text.Substring(index+search.Length+1,index2-index-search.Length-1);
            //    result.Requirements.Skills.Add(text);

            //}






            return result;
        }


    }
}