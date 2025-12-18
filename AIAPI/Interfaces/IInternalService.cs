using AIAPI.DTOs;

namespace AIAPI.Interfaces
{
    public interface IInternalService
    {

        Task<string> GetAllProcessmetaAsync(paramDTO model);
        Task<ProcessMetaResponse> GetAllProcessmetaAsyncnew(paramDTO model);    
        Task<List<OrderDTO>> GetAllPurchasehistorymetaAsync(LoginDTO model);
        Task<List<SchemeDTO>> GetAllSchemeProcessmetaAsync(LoginDTO model);
        Task<List<purchaselist>> GetAllPurchaseProcessmetaAsync(LoginDTO model);
        Task<List<CustomerDTO>> FetchCustomerDetailAsync(string username);

    }
}
