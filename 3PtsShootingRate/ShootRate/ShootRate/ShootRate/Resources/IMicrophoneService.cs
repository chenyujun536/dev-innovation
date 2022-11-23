using System.Threading.Tasks;

namespace ShootRate.Resources
{
    public interface IMicrophoneService
    {
        Task<bool> GetPermissionAsync();
        void OnRequestPermissionResult(bool isGranted);
    }
}
