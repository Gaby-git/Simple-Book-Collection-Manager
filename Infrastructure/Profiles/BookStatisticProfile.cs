using AutoMapper;
using Domaine.Model;


namespace Infrastructure.Profiles
{
    public class BookStatisticProfile : Profile
    {
        public BookStatisticProfile()
        {
            CreateMap<Entities.Bookstatistics, BookStatistics>();

        }
    }
}
