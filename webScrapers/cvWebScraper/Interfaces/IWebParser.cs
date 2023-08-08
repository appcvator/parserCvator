using cvWebScraper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvWebScraper.Interfaces
{
    public interface IWebParser
    {
        string Key { get; }
        Vacancy GetVacancy(string htmlContent);
    }
}
