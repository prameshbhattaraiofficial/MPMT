using Microsoft.AspNetCore.Mvc.Infrastructure;
using Mpmt.Core.Common.MvcHelper;
using Mpmt.Core.Models.Route;
using Mpmt.Data.Repositories.RoleModuleAction;

namespace Mpmt.Agent.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task RunStartupTasksAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var discriptor = services.GetRequiredService<IActionDescriptorCollectionProvider>();

            try
            {
               
                await SeedAllActionsAsync(services);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<WebApplication>();
                logger.LogError(ex, ex.Message);
            }
        }
        internal static async Task SeedAllActionsAsync(IServiceProvider services)
        {
            var descriptor = services.GetRequiredService<IActionDescriptorCollectionProvider>();
            var RolemoduleRepository = services.GetRequiredService<IRoleModuleActionRepository>();
            //todo bring all data from tbl and check maching in here  itself
            var controllerActionRoutes = Helper.GetControllerActionRoutes(descriptor);
            var areaControllerActions = await RolemoduleRepository.GetAreaControllerActionAgent();
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

                await RolemoduleRepository.InsertControllerActionAgentAsync(data);
            }


        }
    }
}
