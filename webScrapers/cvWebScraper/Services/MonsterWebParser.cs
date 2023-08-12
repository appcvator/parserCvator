﻿using parser.Interfaces;
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

        public Vacancy GetVacancy(string htmlContent)
        {
            var result = new Vacancy();


            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(htmlContent);


            var details = new Dictionary<string, string>();


            var cityXpath = "//h3";
            var companyXpath = "//h2";
            var jobDescriptionXpath = "//h2[@class='descriptionstyles__DescriptionHeader-sc-13ve12b-1 fHkPYa']/following::*";
            var jobTitleXpath = "//h1";
            var jobTypeXpath = "//*[@data-test-id='svx-jobview-employmenttype-or-hq-header']//following::*";
            var postDateXpath = "";
            var salaryXpath = "";


            var city = htmlDocument.DocumentNode.SelectSingleNode(cityXpath);
            result.JobDetails.City = city != null ? city.InnerText : "";


            var company = htmlDocument.DocumentNode.SelectSingleNode(companyXpath);
            result.JobDetails.Company = company != null ? company.InnerText : "";




            var jobDescription = htmlDocument.DocumentNode.SelectSingleNode(jobDescriptionXpath);
            if(jobDescription!=null)
            {
                foreach(var item in jobDescription.ChildNodes)
                {
                    if(item.InnerText.Length>50)
                    {
                        result.JobDetails.JobDescription = item.InnerText;
                        break;
                    }
                }
            }


            var jobTitle = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[1]/div/div[1]/div[2]/h1");
            result.JobDetails.JobTitle= jobTitle != null ? jobTitle.InnerText:"";


            var postDate = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[2]/div/div/div[1]/div[1]/div[4]/div[2]");
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

            var responsibilities = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[2]/div/div/div[2]/div/ul[1]");
            if (responsibilities != null)
            {
                foreach (var childNode in responsibilities.ChildNodes)
                {
                    result.Responsibilities.Add(childNode.InnerText);
                }
            }


            
            var education=htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[2]/div/div/div[2]/div/ul[2]/li[15]");
            if (education != null)
            {
                result.Requirements.Education = education.InnerText;
            }


            var experience = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[2]/div/div/div[2]/div/ul[2]/li[1]");
            if (experience != null)
            {
                string experienceString = experience.InnerText;
                var number=Regex.Match(experienceString, @"\d+").Value;
                experienceString =Regex.Replace(experienceString, @"[\d-]", string.Empty);
                result.Requirements.Experience.Years= number;
                result.Requirements.Experience.Description = experienceString;

            }
            


            List<HtmlNode> skills= new List<HtmlNode>();
            skills.Add(htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[2]/div/div/div[2]/div/ul[2]/li[3]"));
            skills.Add(htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[2]/div/div/div[2]/div/ul[2]/li[4]"));
            skills.Add(htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[2]/div/div/div[2]/div/ul[2]/li[5]"));
            skills.Add(htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[2]/div/div/div[2]/div/ul[2]/li[6]"));
            skills.Add(htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[2]/div/div/div[2]/div/ul[2]/li[7]"));
            skills.Add(htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[2]/div/div/div[2]/div/ul[2]/li[8]"));
            skills.Add(htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[2]/div/div/div[2]/div/ul[2]/li[9]"));
            skills.Add(htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[2]/div/div/div[2]/div/ul[2]/li[10]"));
            skills.Add(htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[2]/div/div/div[2]/div/ul[2]/li[11]"));
            skills.Add(htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[2]/div/div/div[2]/div/ul[2]/li[12]"));
            skills.Add(htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[2]/div/div/div[2]/div/ul[2]/li[13]"));
            skills.Add(htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[2]/div/div/div[2]/div/ul[2]/li[14]"));
            skills.Add(htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[2]/div/div/div[2]/div/ul[2]/li[16]"));
            skills.Add(htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[2]/div/div/div[2]/div/ul[2]/li[17]"));
            skills.Add(htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[2]/div/div/div[2]/div/ul[2]/li[18]"));
            skills.Add(htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[2]/div/div/div[2]/div/ul[2]/li[19]"));
            skills.Add(htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[2]/div/div/div[2]/div/ul[2]/li[20]"));
            skills.Add(htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[2]/div/div/div[2]/div/ul[2]/li[21]"));
            skills.Add(htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div/div[2]/div/div/div[2]/div/ul[2]/li[22]"));
            foreach(var skill in skills)
            {
                result.Requirements.Skills.Add(skill.InnerText);
            }



            return result;
        }


    }
}