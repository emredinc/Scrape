using System;
using System.Collections.Generic;
using System.Text;

namespace WebScrape.Entity
{
    public class JobBoard
    {
        public int Id { get; set; }
        public string AnnouncementNo { get; set; }
        public string AnnouncementDate { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string WorkingArea { get; set; }
        public string Status { get; set; }
        public string MannerOfWork { get; set; }
        public string EducationalStatus { get; set; }
        public string Experience { get; set; }
        public string IsHandicapped { get; set; }
        public string JobInfo { get; set; }
    }
}
