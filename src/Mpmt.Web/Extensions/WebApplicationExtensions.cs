using Microsoft.AspNetCore.Mvc.Infrastructure;
using Mpmt.Core;
using Mpmt.Core.Common;
using Mpmt.Core.Common.MvcHelper;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Core.Dtos.Users;
using Mpmt.Core.Models.Route;
using Mpmt.Data.Repositories.PartnerRoles;
using Mpmt.Data.Repositories.RoleModuleAction;
using Mpmt.Data.Repositories.Roles;
using Mpmt.Data.Repositories.Users;

namespace Mpmt.Web.Extensions;

/// <summary>
/// The web application extensions.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Runs the startup tasks async.
    /// </summary>
    /// <param name="app">The app.</param>
    /// <returns>A Task.</returns>
    public static async Task RunStartupTasksAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        var discriptor = services.GetRequiredService<IActionDescriptorCollectionProvider>();

        try
        {
            await SeedSystemRolesAsync(services);
            await SeedSystemPartnerRolesAsync(services);
            await SeedSystemUsersAsync(services);
            await SeedAllActionsAsync(services);
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<WebApplication>();
            logger.LogError(ex, ex.Message);
        }
    }

    /// <summary>
    /// Seeds the system roles async.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>A Task.</returns>
    internal static async Task SeedSystemRolesAsync(IServiceProvider services)
    {
        var role = new AppRole
        {
            RoleName = MpmtUserDefaults.SystemAdminRoleName,
            UserType = MpmtUserDefaults.SystemUserName,
            IsSystemRole = true,
            IsActive = true,
            IsDeleted = false,
        };

        var rolesRepository = services.GetRequiredService<IRolesRepository>();

        var roleExisting = await rolesRepository.GetRoleByNameAsync(role.RoleName);
        if (roleExisting is not null)
            return;

        _ = await rolesRepository.AddRoleAsync(role);
    }

    internal static async Task SeedSystemPartnerRolesAsync(IServiceProvider services)
    {
        var role = new PartnerAdminRole
        {
            RoleName = MpmtUserDefaults.SystemPartnerRoleName,
            UserType = MpmtUserDefaults.SystemUserName,
            Description = "Default Partner Role",
            IsSystemRole = true,
            IsActive = true
        };

        var rolesRepository = services.GetRequiredService<IPartnerRolesRepository>();

        var roleExisting = await rolesRepository.GetPartnerRoleByNameAsync(role.RoleName);
        if (roleExisting is not null)
            return;

        _ = await rolesRepository.AddPartnerRoleAsync(role);
    }

    /// <summary>
    /// Seeds the system users async.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>A Task.</returns>
    internal static async Task SeedSystemUsersAsync(IServiceProvider services)
    {
        var usersRepository = services.GetRequiredService<IUsersRepository>();
        var rolesRepository = services.GetRequiredService<IRolesRepository>();
        var usersRolesRepository = services.GetRequiredService<IUserRolesRepository>();

        var userAdminExisting = await usersRepository.GetUserByUserNameAsync(MpmtUserDefaults.AdminUserName);
        if (userAdminExisting is null)
        {
            var userAdminPassword = "MpmtA$m1n@1^&##";
            var userAdminPasswordSalt = PasswordUtils.GenerateBase64Key(64);
            var userAdminPasswordHash = HashUtils.HashHmacSha512ToBase64(userAdminPassword, userAdminPasswordSalt);

            var userAdmin = new AppUser
            {
                FullName = "admin",
                UserName = MpmtUserDefaults.AdminUserName,
                Email = "admin@mpmt.com.np",
                DateOfBirth = new DateTime(1970, 1, 1),
                DateOfJoining = DateTime.Now,
                IsActive = true,
                PasswordHash = userAdminPasswordHash,
                PasswordSalt = userAdminPasswordSalt,
            };
            var userAddStatus = await usersRepository.AddUserAsync(userAdmin);

            if (userAddStatus.StatusCode != 200)
                throw new MpmtException("Failed to seed user.");

            if (userAddStatus.StatusCode != 200)
                throw new MpmtException("Failed to seed user.");

            var role = await rolesRepository.GetRoleByNameAsync(MpmtUserDefaults.SystemAdminRoleName)
            ?? throw new MpmtException($"Role '{MpmtUserDefaults.SystemAdminRoleName}' not found.");

            var addToRoleStatus = await usersRolesRepository.AddUserToRolesAsync(userAddStatus.IdentityVal.Value, role.Id);

            if (addToRoleStatus.StatusCode != 200)
                throw new MpmtException($"Failed to add role to user '{MpmtUserDefaults.AdminUserName}'.");
        }
        userAdminExisting = await usersRepository.GetUserByUserNameAsync("saroj.chaudhary");
        if (userAdminExisting is null)
        {
            var userAdminPassword = "Secure#$@12";
            var userAdminPasswordSalt = PasswordUtils.GenerateBase64Key(64);
            var userAdminPasswordHash = HashUtils.HashHmacSha512ToBase64(userAdminPassword, userAdminPasswordSalt);

            var userAdmin = new AppUser
            {
                FullName = "Saroj Kumar Chaudhary",
                UserName = "saroj.chaudhary",
                Email = "saroj.chaudhary@mypay.com.np",
                DateOfBirth = new DateTime(1970, 1, 1),
                DateOfJoining = DateTime.Now,
                IsActive = true,
                PasswordHash = userAdminPasswordHash,
                PasswordSalt = userAdminPasswordSalt,
            };
            var userAddStatus = await usersRepository.AddUserAsync(userAdmin);

            if (userAddStatus.StatusCode != 200)
                throw new MpmtException("Failed to seed user.");

            if (userAddStatus.StatusCode != 200)
                throw new MpmtException("Failed to seed user.");

            var role = await rolesRepository.GetRoleByNameAsync(MpmtUserDefaults.SystemAdminRoleName)
            ?? throw new MpmtException($"Role '{MpmtUserDefaults.SystemAdminRoleName}' not found.");

            var addToRoleStatus = await usersRolesRepository.AddUserToRolesAsync(userAddStatus.IdentityVal.Value, role.Id);

            if (addToRoleStatus.StatusCode != 200)
                throw new MpmtException($"Failed to add role to user '{MpmtUserDefaults.AdminUserName}'.");
        }

        return;
    }

    internal static async Task SeedAllActionsAsync(IServiceProvider services)
    {
        var descriptor = services.GetRequiredService<IActionDescriptorCollectionProvider>();
        var RolemoduleRepository = services.GetRequiredService<IRoleModuleActionRepository>();
        //todo bring all data from tbl and check maching in here  itself
        var controllerActionRoutes = Helper.GetControllerActionRoutes(descriptor);
        var areaControllerActions = await RolemoduleRepository.GetAreaControllerAction();
        //var areaControllerActions = areaControllerAction.Take(100).ToList();

        var uniqueList = controllerActionRoutes.Select(i => new { i.Area, i.Controller, i.Action })
            .Except(areaControllerActions.ToList().Select(x => new { x.Area, x.Controller, x.Action }))
            .Select(result => new Routelocation { Area = result.Area, Controller = result.Controller, Action = result.Action })
            .ToList();

        foreach (var menu in uniqueList)
        {
            var actioncontroller = (menu.Area ?? "") + (menu.Controller ?? "") + (menu.Action ?? "");
            var data = new Controlleraction()
            {
                Areacontrolleraction = actioncontroller,
                Action = menu.Action,
                Controller = menu.Controller,
                Area = menu.Area,
                CreatedBy = "system"
            };

            await RolemoduleRepository.InsertControllerActionAsync(data);
        }
    }
}