namespace University.Data.UnitOfWork.Base
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}
