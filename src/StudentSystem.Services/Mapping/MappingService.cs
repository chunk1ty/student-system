using AutoMapper;

namespace StudentSystem.Services.Mapping
{
    public class MappingService : IMappingService
    {
        public T Map<T>(object source)
        {
            return Mapper.Map<T>(source);
        }
    }
}