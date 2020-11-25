using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebScrape.Dal
{
    public class BaseRepository
    {
        protected readonly DbContext context;
        public BaseRepository(DbContext _context)
        {
            this.context = _context;
        }
    }
}
