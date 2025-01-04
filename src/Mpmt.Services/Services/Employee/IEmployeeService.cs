using Mpmt.Core.Dtos.Employee;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.Employee
{
    /// <summary>
    /// The employee service.
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>
        /// Adds the employee async.
        /// </summary>
        /// <param name="addEmployee">The add employee.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddEmployeeAsync(IUDEmployee addEmployee);
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
        /// Removes the document type async.
        /// </summary>
        /// <param name="removeEmployee">The remove employee.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveEmployeeAsync(IUDEmployee removeEmployee);
        /// <summary>
        /// Updates the document type async.
        /// </summary>
        /// <param name="updateEmployee">The update employee.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateEmployeeAsync(IUDEmployee updateEmployee);


    }
}
