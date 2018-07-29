using AutoMapper;

namespace StudentSystem.Clients.Mvc.Services
{
    public interface ICustomMappings
    {
        void CreateMappings(IMapperConfigurationExpression configuration);
    }
}