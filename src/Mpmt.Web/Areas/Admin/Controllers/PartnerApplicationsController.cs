using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.PartnerApplications;
using Mpmt.Services.Services.PartnerApplications;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    [RolePremission]
    [AdminAuthorization]
    public class PartnerApplicationsController : BaseAdminController
    {
        private readonly IMapper _mapper;
        private readonly IRMPService _rMPService;
        private readonly IPartnerApplicationsService _partnerApplicationsService;

        public PartnerApplicationsController(IMapper mapper, IRMPService rMPService, IPartnerApplicationsService partnerApplicationsService)
        {
            _mapper = mapper;
            _rMPService = rMPService;
            _partnerApplicationsService = partnerApplicationsService;
        }

        [HttpGet]
        public async Task<IActionResult> PartnerApplicationsIndex([FromQuery] PartnerApplicationsFilter requestFilter)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            var actions = await _rMPService.GetActionPermissionListAsync("PartnerApplications");
            ViewBag.actions = actions;

            var data = await _partnerApplicationsService.GetPartnerApplicationsAsync(requestFilter);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_PartnerApplicationsList", data);
            }
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> ViewPartnerApplicationsDetail(PartnerApplicationsModel model)
        {
            return PartialView("_ViewPartnerApplicationsDetail", model);
        }

        [HttpGet]
        public async Task<IActionResult> ViewPublicFeedbacksDetail(PublicFeedbacksModel model)
        {
            return PartialView("_ViewPublicFeedbacksDetail", model);
        }

        [HttpGet]
        public async Task<IActionResult> PublicFeedbacksIndex([FromQuery] PublicFeedbacksFilter requestFilter)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            var actions = await _rMPService.GetActionPermissionListAsync("PublicFeedbacks");
            ViewBag.actions = actions;

            var data = await _partnerApplicationsService.GetPublicFeedbacksAsync(requestFilter);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_PublicFeedbacksList", data);
            }
            return View(data);
        }
    }
}
