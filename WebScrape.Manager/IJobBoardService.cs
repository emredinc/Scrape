using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebScrape.Dto;

namespace WebScrape.Manager
{
    public interface IJobBoardService
    {
        Task<List<JobBoardDto>> StartTheCrawler();
        void Add(List<JobBoardDto> entity);
    }
}
