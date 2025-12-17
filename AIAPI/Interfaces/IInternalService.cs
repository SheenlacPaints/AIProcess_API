using AIAPI.DTOs;

namespace AIAPI.Interfaces
{
    public interface IInternalService
    {

        Task<string> GetAllProcessmetaAsync(paramDTO model);
        Task<ProcessMetaResponse> GetAllProcessmetaAsyncnew(paramDTO model);
        Task<List<OrderDTO>> newGetAllProcessmetaAsync(paramDTO model);
        Task<List<SchemeDTO>> GetAllSchemeProcessmetaAsync(paramDTO model);
        Task<List<purchaselist>> GetAllPurchaseProcessmetaAsync(paramDTO model);
        Task<List<CustomerDTO>> FetchCustomerDetailAsync(paramDTO model);
    }
}
