namespace HonestyBar.Infrastructure.Interfaces.Repositories
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
