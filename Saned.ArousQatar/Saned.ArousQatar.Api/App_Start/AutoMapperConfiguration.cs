using AutoMapper;
using Saned.ArousQatar.Api.Infrastructure;
using Saned.ArousQatar.Api.Infrastructure.Mapping;

namespace Saned.ArousQatar.Api
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<DomainToViewModelMappingProfile>();
            });
        }
    }
}