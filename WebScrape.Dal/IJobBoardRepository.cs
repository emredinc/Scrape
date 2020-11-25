using System;
using System.Collections.Generic;
using System.Text;
using WebScrape.Entity;

namespace WebScrape.Dal
{
    public interface IJobBoardRepository
    {
        void Add(List<JobBoard> entity);
    }
}
