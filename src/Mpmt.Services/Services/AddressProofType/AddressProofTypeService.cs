using Mpmt.Core.Dtos.AddressProofType;
using Mpmt.Data.Repositories.AddressProofType;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Services.AddressProofType;

public class AddressProofTypeService : BaseService, IAddressProofTypeService
{
    private readonly IAddressProofTypeRepo _addressProofTypeRepo;

    public AddressProofTypeService(IAddressProofTypeRepo addressProofTypeRepo)
    {
        _addressProofTypeRepo = addressProofTypeRepo;
    }

    public async Task<SprocMessage> AddAddressProofTypeAsync(IUDAddressProofType addressProofType, ClaimsPrincipal claim)
    {
        addressProofType.Event = 'I';
        addressProofType.LoggedInUser = claim?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        addressProofType.UserType = claim?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
        var response = await _addressProofTypeRepo.IUDAddressProofTypeAsync(addressProofType);
        return response;
    }

    public async Task<SprocMessage> DeleteAddressProofTypeAsync(IUDAddressProofType addressProofType, ClaimsPrincipal claim)
    {
        addressProofType.Event = 'D';
        addressProofType.LoggedInUser = claim?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        addressProofType.UserType = claim?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
        var response = await _addressProofTypeRepo.IUDAddressProofTypeAsync(addressProofType);
        return response;
    }

    public async Task<AddressProofTypeDetails> GetAddressProofTypeByIdAsync(int id)
    {
        var response = await _addressProofTypeRepo.GetAddressProofTypeByIdAsync(id);
        return response;
    }

    public async Task<IEnumerable<AddressProofTypeDetails>> GetAddressProofTypesAsync()
    {
        var response = await _addressProofTypeRepo.GetAddressProofTypesAsync();
        return response;
    }

    public async Task<SprocMessage> UpdateAddressProofTypeAsync(IUDAddressProofType addressProofType, ClaimsPrincipal claim)
    {
        addressProofType.Event = 'U';
        addressProofType.LoggedInUser = claim?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        addressProofType.UserType = claim?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
        var response = await _addressProofTypeRepo.IUDAddressProofTypeAsync(addressProofType);
        return response;
    }
}
