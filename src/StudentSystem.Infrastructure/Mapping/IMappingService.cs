namespace StudentSystem.Infrastructure.Mapping
{
    public interface IMappingService
    {
        T Map<T>(object source);
    }
}