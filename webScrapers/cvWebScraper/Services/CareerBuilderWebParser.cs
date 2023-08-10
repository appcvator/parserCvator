using cvWebScraper.Interfaces;
using cvWebScraper.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvWebScraper.Services
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


            var company = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div[1]/main/div/div[2]/div/div/div[2]/div[1]/div[1]/div[1]/div[2]/div/div[1]/div[1]/span[1]");
            result.JobDetails.Company = company != null ? company.InnerText : string.Empty;


            var jobTitle = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[1]/main/div/div[2]/div/div/div[2]/div[1]/div[1]/div[1]/div[2]/div/div[1]/h2");
            result.JobDetails.JobTitle=jobTitle !=null ? jobTitle.InnerText : string.Empty;


            var jobDescription = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[1]/main/div/div[2]/div/div/div[2]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/p[1]");
            if (jobDescription != null)
            {
                var text = jobDescription.InnerText;
                text = text.ToLower();
                var index=text.IndexOf("duties");
                text=text.Substring(0,index);
                result.JobDetails.JobDescription = text;
            }


            var jobType = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"jdp-data\"]/div[1]/div[2]/div/div[1]/div[1]");
            if(jobType != null)
            {
                var text = jobType.InnerText;
                text = text.ToLower();
                if(text.Contains("full"))
                {
                    result.JobDetails.JobType = "Full-Time";
                }
                else if(text.Contains("part"))
                {
                    result.JobDetails.JobType = "Part-time";

                }
                else
                {
                    result.JobDetails.JobType = "";
                }
            }


            var salary = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"cb-salcom-info\"]/div");
            if (salary != null)
            {
                var salaryText = salary.InnerText;
                salaryText=salaryText.ToLower();
                var salaryPeriod = "";
                if (salaryText.Contains("year"))
                {
                    salaryPeriod = "yearly";
                }
                else if (salaryText.Contains("month"))
                {
                    salaryPeriod = "monthly";
                }
                else if (salaryText.Contains("hour"))
                {
                    salaryPeriod = "hourly";
                }
                else { salaryPeriod = ""; }
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


            var responsibilities = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"jdp_description\"]/div[1]/div[1]/p[1]");
            if (responsibilities != null)
            {
                var text = responsibilities.InnerText;
                text = text.ToLower();
                var index = text.IndexOf("duties");
                var index2 = text.IndexOf("who");
                text = text.Substring( index,index2-index-1);
                result.Responsibilities.Add( text);
            }


            var skills = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"jdp_description\"]/div[1]/div[1]/p[1]");
            if(skills!=null)
            {
                var text = skills.InnerText;
                var search = "Who we're looking for";
                var index=text.IndexOf(search);
                var index2 = text.IndexOf("Benefits");
                text = text.Substring(index+search.Length+1,index2-index-search.Length-1);
                result.Requirements.Skills.Add(text);

            }






            return result;
        }


    }
}
