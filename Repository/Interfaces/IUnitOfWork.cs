namespace newplgapi.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IAuthRepository AuthRepository { get; }
        IPlgRepository PlgRepository { get; }
    }
}