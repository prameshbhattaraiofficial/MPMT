using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Mpmt.Core.Models.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Common.MvcHelper
{
    public class Helper
    {
        public static List<Routelocation> GetControllerActionRoutes(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
           
            var Data = new List<Routelocation>();

            var actionDescriptors = actionDescriptorCollectionProvider.ActionDescriptors.Items;

            foreach (var actionDescriptor in actionDescriptors)
            {
                if (actionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                {
                    var loc = new Routelocation();
                    var routevalue = controllerActionDescriptor.RouteValues;
                    routevalue.TryGetValue("Area", out var area);
                    routevalue.TryGetValue("Controller", out var controller);
                    routevalue.TryGetValue("Action", out var action);
                    loc.Area = area;
                    loc.Controller = controller;
                    loc.Action = action;
                    Data.Add(loc);

                    //controllerActionRoutes.Add(controllerName + "_" + actionName, actionName);

                }
            }

            return Data;
        }
        
    }
}
