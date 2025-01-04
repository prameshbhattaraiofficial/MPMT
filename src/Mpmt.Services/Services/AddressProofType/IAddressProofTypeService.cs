using Mpmt.Core.Dtos.AddressProofType;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Services.AddressProofType;

public interface IAddressProofTypeService
{
    Task<IEnumerable<AddressProofTypeDetails>> GetAddressProofTypesAsync();
    Task<AddressProofTypeDetails> GetAddressProofTypeByIdAsync(int id);
    Task<SprocMessage> AddAddressProofTypeAsync(IUDAddressProofType addressProofType, ClaimsPrincipal claim);
    Task<SprocMessage> UpdateAddressProofTypeAsync(IUDAddressProofType addressProofType, ClaimsPrincipal claim);
    Task<SprocMessage> DeleteAddressProofTypeAsync(IUDAddressProofType addressProofType, ClaimsPrincipal claim);
}
