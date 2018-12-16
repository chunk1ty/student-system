namespace StudentSystem.Persistence.Contracts
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}