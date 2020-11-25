using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebScrape.Entity;

namespace WebScrape.Dal
{
    public class JobBoardRepository : BaseRepository, IJobBoardRepository
    {
        public JobBoardRepository(DbContext context) : base(context) { }
        public void Add(List<JobBoard> entity)
        {
            context.Set<JobBoard>().AddRange(entity);
            context.SaveChanges();
        }
    }
}
