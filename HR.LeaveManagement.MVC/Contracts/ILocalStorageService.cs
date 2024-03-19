namespace HR.LeaveManagement.MVC.Contracts
{
    public interface ILocalStorageService
    {
        void ClearStorage(List<string> keys);
        void SetStorageValue<T>(string key, T value);
        T GetStorageValue<T>(string key);
        bool Exists(string key);
    }
}
