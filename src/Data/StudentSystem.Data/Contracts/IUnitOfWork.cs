namespace StudentSystem.Data.Contracts
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}