using AutoMapper;
using Mpmt.Core.Dtos.Employee;
using Mpmt.Data.Common;
using Mpmt.Data.Repositories.Employee;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.Employee
{
    /// <summary>
    /// The employee service.
    /// </summary>
    public class EmployeeService : BaseService, IEmployeeService
    {
        private readonly IEmployeeRepo _employeerepo;
        private readonly IMapper _mapper;
        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeService"/> class.
        /// </summary>
        /// <param name="employeeRepo">The employee repo.</param>
        /// <param name="mapper">The mapper.</param>
        public EmployeeService(IEmployeeRepo employeeRepo, IMapper mapper)
        {
            _employeerepo = employeeRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds the employee async.
        /// </summary>
        /// <param name="addDocumentType">The add document type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddEmployeeAsync(IUDEmployee addEmployee)
        {
            var passwordSalt = CryptoUtils.GenerateKeySalt();
            var passwordHash = CryptoUtils.HashHmacSha512Base64(addEmployee.Password, passwordSalt);
            addEmployee.PasswordHash = passwordHash;
            addEmployee.PasswordSalt = passwordSalt;
            //addEmployee.DateOfBirth = Convert.ToDateTime("01/01/1900");
            //addEmployee.DateOfJoining = Convert.ToDateTime("01/01/1900");
            var response = await _employeerepo.AddEmployeeAsync(addEmployee);
            return response;
        }


        /// <summary>
        /// Gets the employee async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<EmployeeDetails>> GetEmployeeAsync()
        {
            var response = await _employeerepo.GetEmployeeAsync();
            return response;
        }


        /// <summary>
        /// Gets the employee by id async.
        /// </summary>
        /// <param name="documentTypeId">The document type id.</param>
        /// <returns>A Task.</returns>
        public async Task<IUDEmployee> GetEmployeeByIdAsync(int EmployeeId)
        {
            var response = await _employeerepo.GetEmployeeByIdAsync(EmployeeId);
            return response;
        }


        /// <summary>
        /// Removes the document type async.
        /// </summary>
        /// <param name="removeDocumentType">The remove document type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveEmployeeAsync(IUDEmployee removeEmployee)
        {
            var mappedData = _mapper.Map<IUDEmployee>(removeEmployee);
            var response = await _employeerepo.RemoveEmployeeAsync(mappedData);
            return response;
        }




        /// <summary>
        /// Updates the document type async.
        /// </summary>
        /// <param name="updateDocumentType">The update document type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateEmployeeAsync(IUDEmployee updateEmployee)
        {
            // var mappedData = _mapper.Map<IUDEmployee>(updateEmployee);
            //var passwordSalt = CryptoUtils.GenerateKeySalt();
            //var passwordHash = CryptoUtils.HashHmacSha512Base64(updateEmployee.Password, passwordSalt);
            //updateEmployee.PasswordHash = passwordHash;
            //updateEmployee.PasswordSalt = passwordSalt;
            var response = await _employeerepo.UpdateEmployeeAsync(updateEmployee);
            return response;
        }
    }
}
