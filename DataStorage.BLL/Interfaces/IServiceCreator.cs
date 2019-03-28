namespace DataStorage.BLL.Interfaces
{
    public interface IServiceCreator
    {
        IUserService CreateUserService(string connectionString);
    }
}
