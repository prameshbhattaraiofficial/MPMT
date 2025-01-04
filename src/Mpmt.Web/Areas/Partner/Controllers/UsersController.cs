using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Domain.Partners.Senders;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.ViewModel.User;
using Mpmt.Services.Partner;
using Mpmt.Services.Services.Common;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;
using System.Net.Http.Headers;

namespace Mpmt.Web.Areas.Partner.Controllers
{
    /// <summary>
    /// The users controller.
    /// </summary>
    [PartnerAuthorization]
    [RolePremission]
    public class UsersController : BasePartnerController
    {
        private readonly IPartnerSenderService _partnerSenderService;
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;
        private readonly ICommonddlServices _commonddl;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="partnerSenderService">The partner sender service.</param>
        /// <param name="notyfService">The notyf service.</param>
        /// <param name="commonddl">The commonddl.</param>
        public UsersController(IPartnerSenderService partnerSenderService,
            INotyfService notyfService,
            IMapper mapper,
             ICommonddlServices commonddl)
        {
            _partnerSenderService = partnerSenderService;
            _notyfService = notyfService;
            _mapper = mapper;
            _commonddl = commonddl;
        }

        /// <summary>
        /// Indices the.
        /// </summary>
        /// <returns>An IActionResult.</returns>
        public IActionResult Index()
        {
            return RedirectToAction(nameof(UserList));
        }


        /// <summary>
        /// Users the list.
        /// </summary>
        /// <param name="senderPagedRequest">The sender paged request.</param>
        /// <returns>A Task.</returns>
        public async Task<IActionResult> UserList([FromQuery] SenderPagedRequest senderPagedRequest)
        {
            senderPagedRequest.PartnerCode = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;

            var userlist = await _partnerSenderService.GetSenderListAsync(senderPagedRequest);

            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_UserListTable", userlist));
            return View(userlist);
        }
        public async Task<GetDocumentCharcterModel> GetCharacterByDocumentType(string documentTypeId)
        {
            var GetDocumentCharacter = await _commonddl.GetDocumentCharacterByDocumentType(documentTypeId);
            return GetDocumentCharacter;
        }
        [HttpGet]
        public async Task<IActionResult> AddUser(string documentTypeId)
        {
            var data = await _commonddl.GetCountryddl();
            ViewBag.Countryddl = new SelectList(data, "value", "Text");
            var Provision = await _commonddl.Getprovinceddl("NP");
            ViewBag.Provisionddl = new SelectList(Provision, "value", "Text");
            var Document = await _commonddl.GetDocumentTypeddl();
            ViewBag.DocumentType = new SelectList(Document, "value", "Text");
            var genderType = await _commonddl.Getgenderddl();
            ViewBag.genderType = new SelectList(genderType, "value", "Text");
            var IncomeSource = await _commonddl.GetIncomeSourceAsyncddl();
            ViewBag.IncomeSourceType = new SelectList(IncomeSource, "value", "Text");
            var occupation = await _commonddl.GetoccupationAsyncddl();
            ViewBag.occupationType = new SelectList(occupation, "value", "Text");
            var CallingCode = await _commonddl.GetCallingCodeddl();
            ViewBag.CallingCode = new SelectList(CallingCode, "Text", "lookup");


            return PartialView(nameof(AddUser));
        }
        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="SenderAddUpdate">The sender add update.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(AddUserViewModel SenderAddUpdate)
        {
            var data = await _commonddl.GetCountryddl();
            ViewBag.Countryddl = new SelectList(data, "value", "Text");
            var Document = await _commonddl.GetDocumentTypeddl();
            ViewBag.DocumentType = new SelectList(Document, "value", "Text");
            var genderType = await _commonddl.Getgenderddl();
            ViewBag.genderType = new SelectList(genderType, "value", "Text");
            var IncomeSource = await _commonddl.GetIncomeSourceAsyncddl();
            ViewBag.IncomeSourceType = new SelectList(IncomeSource, "value", "Text");
            var occupation = await _commonddl.GetoccupationAsyncddl();
            ViewBag.occupationType = new SelectList(occupation, "value", "Text");

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView(SenderAddUpdate);
            }

            var addUserResult = await _partnerSenderService.AddSenderAsync(SenderAddUpdate);

            if (!addUserResult.Success)
            {
                Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                ViewBag.Error = addUserResult.Errors.First();
                return PartialView(SenderAddUpdate);
            }

            _notyfService.Success(addUserResult.Message);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUser(int UserId)
        {
            if (UserId <= 0)
            {
                return PartialView("Error");
            }
            var PartnerCode = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;

            var userdetail = await _partnerSenderService.GetSenderByIdAsync(UserId, PartnerCode);
            if (userdetail == null)
            {
                return PartialView("Error");
            }
            var updatedata = _mapper.Map<UpdateUserVM>(userdetail);
            var data = await _commonddl.GetCountryddl();
            ViewBag.Countryddl = new SelectList(data, "value", "Text");

            var Document = await _commonddl.GetDocumentTypeddl();
            ViewBag.DocumentType = new SelectList(Document, "value", "Text");
            var genderType = await _commonddl.Getgenderddl();
            ViewBag.genderType = new SelectList(genderType, "value", "Text");
            var IncomeSource = await _commonddl.GetIncomeSourceAsyncddl();
            ViewBag.IncomeSourceType = new SelectList(IncomeSource, "value", "Text");
            var occupation = await _commonddl.GetoccupationAsyncddl();
            ViewBag.occupationType = new SelectList(occupation, "value", "Text");



            return PartialView(updatedata);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(UpdateUserVM SenderAddUpdate)
        {
            var data = await _commonddl.GetCountryddl();
            ViewBag.Countryddl = new SelectList(data, "value", "Text");
            var Document = await _commonddl.GetDocumentTypeddl();
            ViewBag.DocumentType = new SelectList(Document, "value", "Text");
            var genderType = await _commonddl.Getgenderddl();
            ViewBag.genderType = new SelectList(genderType, "value", "Text");
            var IncomeSource = await _commonddl.GetIncomeSourceAsyncddl();
            ViewBag.IncomeSourceType = new SelectList(IncomeSource, "value", "Text");
            var occupation = await _commonddl.GetoccupationAsyncddl();
            ViewBag.occupationType = new SelectList(occupation, "value", "Text");

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView(SenderAddUpdate);
            }

            var updateUserResult = await _partnerSenderService.UpdateSenderAsync(SenderAddUpdate);
            if (!updateUserResult.Success)
            {
                Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                ViewBag.Error = updateUserResult.Errors.First();
                return PartialView(SenderAddUpdate);
            }

            _notyfService.Success(updateUserResult.Message);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(int UserId = 0)
        {
            var data = new DeleteUserVM()
            {
                Id = UserId
            };
            return await Task.FromResult(PartialView("DeleteUser", data));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(DeleteUserVM SenderDelete)
        {
            if (SenderDelete.Id <= 0)
            {
                _notyfService.Error("Unable to delete");
                return Ok();
            }


            var response = await _partnerSenderService.RemoveSenderAsync(SenderDelete.Id);
            if (response.StatusCode == 200)
            {
                _notyfService.Success(response.MsgText);
                return Ok();
            }
            _notyfService.Error(response.MsgText);
            return Ok();

        }

        [HttpGet]
        public async Task<IActionResult> UserDetail(int UserId = 0)
        {
            if (UserId <= 0)
            {
                return PartialView("Error");
            }
            var data = await _commonddl.GetCountryddl();
            ViewBag.Countryddl = new SelectList(data, "value", "Text");

            var Document = await _commonddl.GetDocumentTypeddl();
            ViewBag.DocumentType = new SelectList(Document, "value", "Text");
            var genderType = await _commonddl.Getgenderddl();
            ViewBag.genderType = new SelectList(genderType, "value", "Text");
            var IncomeSource = await _commonddl.GetIncomeSourceAsyncddl();
            ViewBag.IncomeSourceType = new SelectList(IncomeSource, "value", "Text");
            var occupation = await _commonddl.GetoccupationAsyncddl();
            ViewBag.occupationType = new SelectList(occupation, "value", "Text");

            var PartnerCode = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode")?.Value;

            var userdetail = await _partnerSenderService.GetSenderByIdAsync(UserId, PartnerCode);
            if (userdetail == null)
            {
                return PartialView("Error");
            }
            return PartialView(userdetail);
        }
        [HttpGet]
        public async Task<IActionResult> UserBankDetail(int UserId = 0)
        {
            var PartnerCode = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode")?.Value;
            if (UserId <= 0)
            {
                return PartialView("Error");
            }

            var userdetail = await _partnerSenderService.GetSenderByIdAsync(UserId, PartnerCode);
            if (userdetail == null)
            {
                return PartialView("Error");
            }
            return PartialView(userdetail);
        }


        /// <summary>
        /// Kycs the list.
        /// </summary>
        /// <returns>An IActionResult.</returns>
        public IActionResult KycList()
        {
            return View();
        }
    }
}
