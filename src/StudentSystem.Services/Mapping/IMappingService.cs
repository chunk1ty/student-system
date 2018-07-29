namespace StudentSystem.Services.Mapping
{
    public interface IMappingService
    {
        T Map<T>(object source);
    }
}