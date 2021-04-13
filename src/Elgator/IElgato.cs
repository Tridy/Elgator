using Refit;
using System.Threading.Tasks;

namespace Elgator
{
    public interface IElgato
    {
        [Get("/elgato/accessory-info")]
        Task<AccessoryInfo> GetAccessoryInfo();

        [Put("/elgato/lights")]
        Task<StateChangeResult> SetState([Body] string stateInfo);

        [Get("/elgato/lights")]
        Task<StateChangeResult> GetState([Body] string stateInfo);
    }
}