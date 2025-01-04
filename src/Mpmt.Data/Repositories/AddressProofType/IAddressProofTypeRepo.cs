using Mpmt.Core.Dtos.AddressProofType;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.AddressProofType;

public interface IAddressProofTypeRepo
{
    Task<IEnumerable<AddressProofTypeDetails>> GetAddressProofTypesAsync();
    Task<AddressProofTypeDetails> GetAddressProofTypeByIdAsync(int id);
    Task<SprocMessage> IUDAddressProofTypeAsync(IUDAddressProofType addressProofType);
}