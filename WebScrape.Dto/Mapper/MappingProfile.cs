using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebScrape.Entity;

namespace WebScrape.Dto.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<JobBoardDto, JobBoard>().ReverseMap();
        }
    }
}
