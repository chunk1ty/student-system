using AutoMapper;

namespace StudentSystem.Clients.Mvc.Services
{
    public interface IMappingService
    {
        T Map<T>(object source);
    }

    // Implement as static class ?
    public class MappingService : IMappingService
    {
        public T Map<T>(object source)
        {
            return Mapper.Map<T>(source);
        }
    }
}