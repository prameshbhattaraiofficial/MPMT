using Mpmt.Core.Dtos.Employee;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.PartnerEmployee;
using Mpmt.Core.Dtos.Users;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Employee
{
    /// <summary>
    /// The employee repo.
    /// </summary>
    public interface IEmployeeRepo
    {
        /// <summary>
        /// Adds the employee async.
        /// </summary>
        /// <param name="emp">The emp.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddEmployeeAsync(IUDEmployee emp);
        /// <summary>
        /// Gets the employee async.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<EmployeeDetails>> GetEmployeeAsync();
        /// <summary>
        /// Gets the employee by id async.
        /// </summary>
        /// <param name="EmployeeId">The employee id.</param>
        /// <returns>A Task.</returns>
        Task<IUDEmployee> GetEmployeeByIdAsync(int EmployeeId);
        /// <summary>
        /// Removes the employee async.
        /// </summary>
        /// <param name="removeDocumentType">The remove document type.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveEmployeeAsync(IUDEmployee removeDocumentType);
        /// <summary>
        /// Updates the employee async.
        /// </summary>
        /// <param name="updateDocumentType">The update document type.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateEmployeeAsync(IUDEmployee updateDocumentType);
        Task<AppPartnerEmployee> GetPartnerEmployeeByEmailAsync(string email);
        Task<AppPartnerEmployee> GetPartnerEmployeeByUserNameAsync(string userName);
        Task UpdatePartnerEmployeeLoginActivityAsync(UserLoginActivity loginActivity);
        void UpdateAccountSecretKey(string email, string accountsecretkey);
        void UpdateIs2FAAuthenticated(string email, bool Is2FAAuthenticated);
        Task<TokenVerification> GetOtpBypartnerEmployeeCodeAsync(string partnercode, string UserName, string OtpVerificationFor);
        void UpdateEmailConfirmAsync(string partnercode, string UserName);
        Task<string> CheckPartnerOrEmployeeByEmail(string email);
        Task<string> CheckPartnerOrEmployeeByUserName(string UserName);


    }
}
