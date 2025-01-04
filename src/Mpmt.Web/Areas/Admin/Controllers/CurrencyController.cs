using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.ViewModel.Currency;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.Currency;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The currency controller.
    /// </summary>
    [RolePremission]
    [AdminAuthorization]
    public class CurrencyController : BaseAdminController
    {
        private readonly ICommonddlServices _commonddl;
        private readonly IRMPService _rMPService;
        private readonly ICurrencyServices _currencyServices;
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyController"/> class.
        /// </summary>
        /// <param name="notyfService">The notyf service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="currencyServices">The currency services.</param>
        /// <param name="commonddl">The commonddl.</param>
        public CurrencyController(INotyfService notyfService, IMapper mapper, ICurrencyServices currencyServices, ICommonddlServices commonddl,IRMPService rMPService)
        
        {

            _notyfService = notyfService;
            _mapper = mapper;
            _currencyServices = currencyServices;
            _commonddl = commonddl;
            _rMPService = rMPService;
        }

        #region Currency-Index
        /// <summary>
        /// Currencies the index.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IActionResult> CurrencyIndex()
        {
            var actions = await _rMPService.GetActionPermissionListAsync("Currency");
            ViewBag.actions = actions;
            var Result = await _currencyServices.GetCurrencyAsync();
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_CurrencyIndex", Result));

            return await Task.FromResult(View(Result));
        }
        #endregion

        #region Currency-Add

        /// <summary>
        /// Adds the currency.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> AddCurrency()
        {
            var data = await _commonddl.GetCountryddl();
            ViewBag.Country = new SelectList(data, "value", "Text");

            return await Task.FromResult(PartialView());
        }

        /// <summary>
        /// Adds the currency.
        /// </summary>
        /// <param name="addCurrencyVm">The add currency vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("added currency")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCurrency([FromForm] AddCurrencyVm addCurrencyVm)
        {
            var data = await _commonddl.GetCountryddl();
            ViewBag.Country = new SelectList(data, "value", "Text");

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
           
            
            else
            {
                var ResponseStatus = await _currencyServices.AddCurrencyAsync(addCurrencyVm);
                if (ResponseStatus.StatusCode == 200)
                {
                    _notyfService.Success(ResponseStatus.MsgText);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    ViewBag.Error = ResponseStatus.MsgText;
                    return PartialView();
                }
            }

        }
        #endregion

        #region Currency-Update

        /// <summary>
        /// Updates the currency.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> UpdateCurrency(int id)
        {
            var data = await _commonddl.GetCountryddl();
            ViewBag.Country = new SelectList(data, "value", "Text");

            var Result = await _currencyServices.GetCurrencyByIdAsync(id);
            var mappeddata = _mapper.Map<UpdateCurrencyVm>(Result);
            return await Task.FromResult(PartialView(mappeddata));
        }

        /// <summary>
        /// Updates the currency.
        /// </summary>
        /// <param name="updateCurrency">The update currency.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("updated currency")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCurrency([FromForm] UpdateCurrencyVm updateCurrency)
        {
            var data = await _commonddl.GetCountryddl();
            ViewBag.Country = new SelectList(data, "value", "Text");

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView(updateCurrency);
            }

            else
            {
                var ResponseStatus = await _currencyServices.UpdateCurrencyAsync(updateCurrency);
                if (ResponseStatus.StatusCode == 200)
                {
                    _notyfService.Success(ResponseStatus.MsgText);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    ViewBag.Error = ResponseStatus.MsgText;
                    return PartialView(updateCurrency);
                }
            }

        }
        #endregion

        #region Currency-Delete

        /// <summary>
        /// Deletes the currency.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteCurrency(int id)
        {
            var Result = await _currencyServices.GetCurrencyByIdAsync(id);
            var mappeddata = _mapper.Map<UpdateCurrencyVm>(Result);
            return await Task.FromResult(PartialView(mappeddata));
        }

        /// <summary>
        /// Deletes the currency.
        /// </summary>
        /// <param name="updateCurrency">The update currency.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("deleted currency")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCurrency([FromForm] UpdateCurrencyVm updateCurrency)
        {
            if (updateCurrency.Id != 0)
            {
                var ResponseStatus = await _currencyServices.RemoveCurrencyAsync(updateCurrency);
                if (ResponseStatus.StatusCode == 200)
                {
                    _notyfService.Success(ResponseStatus.MsgText);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    TempData["Error"] = ResponseStatus.MsgText;
                }
            }
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return PartialView();
        }
        #endregion

    }
}
