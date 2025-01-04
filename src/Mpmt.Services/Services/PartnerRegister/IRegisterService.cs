using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Domain.Partners.Register;
using Mpmt.Core.Dtos.PartnerSignUp;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.PartnerRegister
{
    public interface IRegisterService
    {
        Task<SprocMessage> RegisterPartner(SignUpPartnerdetail data);
        Task<SprocMessage> ResetOtp(string Email, string Opt);
        Task<SprocMessage> ValidateOtp(string Email, string Otp);

        Task<SprocMessage> RegisterPartnerstep1(SignUpStep1 data);

        Task<SprocMessage> RegisterPartnerstep2(SignUpStep2 data);

        Task<SprocMessage> RegisterPartnerstep3(SignUpStep3 data);

        Task<PartnerDetailSignup> GetRegisterPartner(string Email);
        Task<PartnerDetailSignup> GetRegisterPartnerByID(string Id);


        // Task<List<string>> AddressSearch(string query);

        // Task<List<PlaceSuggestion>> AddressSearch(string query);


        // Task<AddressDetails> AddressSearch(string placeId);


        Task<List<PlaceSuggestion>> AddressSearch(string query);

        Task<AddressDetails> GetPlaceDetails(string placeId);

    }
}
