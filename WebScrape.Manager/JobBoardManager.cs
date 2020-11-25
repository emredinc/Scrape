using AutoMapper;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebScrape.Dal;
using WebScrape.Dto;
using WebScrape.Entity;
using WebScrape.Manager.HttpHelper;

namespace WebScrape.Manager
{
    public class JobBoardManager : IJobBoardService
    {
        private readonly CrawlerHttpClient webClient;
        private readonly HtmlDocument document;
        private readonly IJobBoardRepository jobBoardRepository;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        public JobBoardManager(IJobBoardRepository _jobBoardRepository, IMapper _mapper, IConfiguration _configuration)
        {
            jobBoardRepository = _jobBoardRepository;
            mapper = _mapper;
            webClient = new CrawlerHttpClient();
            document = new HtmlDocument();
            configuration = _configuration;
        }

        public void Add(List<JobBoardDto> entityList)
        {
            jobBoardRepository.Add(mapper.Map<List<JobBoardDto>, List<JobBoard>>(entityList));
        }

        public async Task<List<JobBoardDto>> StartTheCrawler()
        {

            string CurrentDomain = configuration.GetSection("WebSiteUrl").GetSection("CurrentDomain").Value;
            string CurrentPath = CurrentDomain + configuration.GetSection("WebSiteUrl").GetSection("CurrentPath").Value;

            string pageNaviButtonsNodes = "//ul[@class='pageNaviButtons']//a[@href]";
            string JobBoardDetailNodes = "//table[@id='searchResultsTable']//tbody[@class='searchResultsRowClass']//tr[@class='searchResultsItem     career-category-height']//td[@class='searchResultsTitleValue ']//a[@class=' classifiedTitle']";
            List<string> pagingLinkList = new List<string>(); //sayfalama da kullanılan numaralar üzerindeki diğer sayfaların linkleri listelenir
            List<string> JobBoardLinkList = new List<string>(); //tüm ilanların href linkleri tutulur

            #region Main Page Header
          
            webClient.Headers.Clear();
            webClient.Headers.Add($"Host", $"www.sahibinden.com");
            webClient.Headers.Add($"Cache-Control", $"max-age=0");
            webClient.Headers.Add($"Upgrade-Insecure-Requests", $"1");
            webClient.Headers.Add($"User-Agent", $"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36");
            webClient.Headers.Add($"Accept", $"text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            webClient.Headers.Add($"Sec-Fetch-Site", $"none");
            webClient.Headers.Add($"Sec-Fetch-Mode", $"navigate");
            webClient.Headers.Add($"Sec-Fetch-User", $"?1");
            webClient.Headers.Add($"Sec-Fetch-Dest", $"document");
            webClient.Headers.Add($"Accept-Encoding", $"gzip, deflate, br");
            webClient.Headers.Add($"Accept-Language", $"tr-TR,tr;q=0.9,en-US;q=0.8,en;q=0.7");

            #endregion


            //ana link üzerinden ilk sayfa yüklenir
            var response = await webClient.Get($"{CurrentPath}");
            document.LoadHtml(response);

            var pageNaviLink = document.DocumentNode.SelectNodes(pageNaviButtonsNodes).ToList();
            foreach (HtmlNode item in pageNaviLink)
            {
                string hrefValue = CurrentDomain + item.GetAttributeValue("href", string.Empty);
                pagingLinkList.Add(hrefValue);
            }
            pagingLinkList.RemoveAt(pagingLinkList.Count - 1); //numaralardan sonuncusu sonraki butonu listeden kaldırılır

            //ilk sayfadaki ilanlar üzerindeki ilan linkleri alınır
            var hrefListForMainPage = document.DocumentNode.SelectNodes(JobBoardDetailNodes).ToList();
            foreach (var item in hrefListForMainPage)
            {
                string hrefValue = CurrentDomain + item.GetAttributeValue("href", string.Empty);
                JobBoardLinkList.Add(hrefValue);
            }

            //tüm sayfalar gezilir ve tüm ilanların linkleri alınır
            foreach (var item in pagingLinkList)
            {
                var res = await webClient.Get($"{item}");
                document.LoadHtml(res);

                var hrefListForPager = document.DocumentNode.SelectNodes(JobBoardDetailNodes).ToList();
                foreach (var items in hrefListForPager)
                {
                    string hrefValue = CurrentDomain + items.GetAttributeValue("href", string.Empty);
                    JobBoardLinkList.Add(hrefValue);
                }
            }



            #region Detail Pages Header

            webClient.Headers.Clear();
            webClient.Headers.Add($"Host", $"www.sahibinden.com");
            webClient.Headers.Add($"Upgrade-Insecure-Requests", $"1");
            webClient.Headers.Add($"User-Agent", $"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36");
            webClient.Headers.Add($"Accept", $"text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            webClient.Headers.Add($"Sec-Fetch-Site", $"same-origin");
            webClient.Headers.Add($"Sec-Fetch-Mode", $"navigate");
            webClient.Headers.Add($"Sec-Fetch-User", $"?1");
            webClient.Headers.Add($"Sec-Fetch-Dest", $"document");
            webClient.Headers.Add($"Referer", $"https://www.sahibinden.com/insaat-ve-yapi-is-ilanlari?pagingSize=50");
            webClient.Headers.Add($"Accept-Encoding", $"gzip, deflate, br");
            webClient.Headers.Add($"Accept-Language", $"tr-TR,tr;q=0.9,en-US;q=0.8,en;q=0.7");

            #endregion

            List<JobBoardDto> jobBoardList = new List<JobBoardDto>();
            //ilan detayları alınır
            //int counter = 0;
            foreach (var item in JobBoardLinkList)
            {
                //counter++;
                //if (counter == 3) return jobBoardList;
               
                var res = await webClient.Get($"{item}");
                document.LoadHtml(res);

                JobBoardDto dto = new JobBoardDto();

                var detailList = document.DocumentNode.SelectNodes("//ul[@class='classifiedInfoList']//li").ToList();

                var AnnouncementNoText = detailList[0].QuerySelector("strong").InnerText.Trim().Replace("\r", "").Replace("\n", "");
                var AnnouncementNoValue = detailList[0].QuerySelector("span").InnerText;
                if (AnnouncementNoText == "İlan No")
                {
                    dto.AnnouncementNo = Regex.Replace(AnnouncementNoValue, @"( |\&nbsp;|\t|\r?\n)\1+", "$1").Trim().Replace(" ", "").Replace("\r", "").Replace("\n", "");
                }

                var AnnouncementDateText = detailList[1].QuerySelector("strong").InnerText.Trim().Replace("\r", "").Replace("\n", "");
                var AnnouncementDateValue = detailList[1].QuerySelector("span").InnerText;
                if (AnnouncementDateText == "İlan Tarihi")
                {
                    dto.AnnouncementDate = AnnouncementDateValue.Trim().Replace("\r", "").Replace("\n", "");
                }

                var CityDistrictText = detailList[2].QuerySelector("strong").InnerText.Trim().Replace(" ", "").Replace("\r", "").Replace("\n", " ");
                var CityDistrictValue = detailList[2].QuerySelector("span").InnerText;
                if (CityDistrictText == "İl/İlçe")
                {
                    string[] cityDist = CityDistrictValue.Split("/");
                    dto.City = cityDist[0].Trim().Replace(" ", "").Replace("\r", "").Replace("\n", " ");
                    dto.District = cityDist[1].Trim().Replace(" ", "").Replace("\r", "").Replace("\n", " ");
                }

                var WorkingAreaText = detailList[3].QuerySelector("strong").InnerText.Trim().Replace("\r", "").Replace("\n", " ");
                var WorkingAreaValue = detailList[3].QuerySelector("span").InnerText;
                if (WorkingAreaText == "İş Alanı")
                {
                    dto.WorkingArea = WorkingAreaValue.Trim().Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "");
                }

                var StatusText = detailList[4].QuerySelector("strong").InnerText.Trim().Replace("\r", "").Replace("\n", " ");
                var StatusValue = detailList[4].QuerySelector("span").InnerText;
                if (StatusText == "Pozisyon")
                {
                    dto.Status = StatusValue.Trim().Replace("\r", "").Replace("\n", " ").Replace("&nbsp;", "").Replace("&amp;", "");
                }

                var MannerOfWorkText = detailList[5].QuerySelector("strong").InnerText.Trim().Replace("\r", "").Replace("\n", " ");
                var MannerOfWorkValue = detailList[5].QuerySelector("span").InnerText;
                if (MannerOfWorkText == "Çalışma Şekli")
                {
                    dto.MannerOfWork = MannerOfWorkValue.Trim().Replace("\r", "").Replace("\n", "");
                }

                var EducationalStatusText = detailList[6].QuerySelector("strong").InnerText.Trim().Replace("\r", "").Replace("\n", " ");
                var EducationalStatusValue = detailList[6].QuerySelector("span").InnerText;
                if (EducationalStatusText == "Eğitim Durumu")
                {
                    dto.EducationalStatus = EducationalStatusValue.Trim().Replace("\r", "").Replace("\n", "");
                }

                var ExperienceText = detailList[7].QuerySelector("strong").InnerText.Trim().Replace("\r", "").Replace("\n", " ");
                var ExperienceValue = detailList[7].QuerySelector("span").InnerText;
                if (ExperienceText == "Deneyim")
                {
                    dto.Experience = ExperienceValue.Trim().Replace("\r", "").Replace("\n", "");
                }

                var IsHandicappedText = detailList[8].QuerySelector("strong").InnerText.Trim().Replace("\r", "").Replace("\n", " ");
                var IsHandicappedValue = detailList[8].QuerySelector("span").InnerText;
                if (IsHandicappedText == "Engelliye Uygun")
                {
                    dto.IsHandicapped = IsHandicappedValue.Trim().Replace("\r", "").Replace("\n", "");
                }

                var JobInfo = document.QuerySelector("#classifiedDescription").InnerText.Trim();
                dto.JobInfo = Regex.Replace(JobInfo, @"( |\&nbsp;|\t|\r?\n)\1+", "$1").Replace("\r", "").Replace("\n", " ");
                jobBoardList.Add(dto);
            }

            return jobBoardList;
        }
    }
}
