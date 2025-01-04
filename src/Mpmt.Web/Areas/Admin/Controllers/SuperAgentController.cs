using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Common;
using Mpmt.Core.Domain;
using Mpmt.Core.Domain.Payout;
using Mpmt.Core.Dtos.Agent;
using Mpmt.Core.Dtos.AgentFundTransfer;
using Mpmt.Core.Dtos.BulkAgent;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Dtos.CashAgent.Commission;
using Mpmt.Core.Dtos.PartnerStatement;
using Mpmt.Core.Dtos.SuperAgent;
using Mpmt.Core.Dtos.WalletLoad.Statement;
using Mpmt.Core.Extensions;
using Mpmt.Core.ViewModel.CashAgent;
using Mpmt.Core.ViewModel.SuperAgent;
using Mpmt.Services.CashAgents;
using Mpmt.Services.Services.Common;
using Mpmt.Web.Areas.Admin.ViewModels.Agent;
using Mpmt.Web.Areas.Admin.ViewModels.Paetner;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using OfficeOpenXml;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net;
using System.Text;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    public class SuperAgentController : BaseAdminController
    {
        private readonly IAgentCredentialsService _agentCredentialsService;
        private readonly INotyfService _notyfService;
        private readonly ICashAgentUserService _cashAgentUserService;
        private readonly ICommonddlServices _commonddlService;
        private readonly ICashAgentCommissionService _cashAgentCommissionService;
        private readonly IMapper _mapper;
        private readonly IAgentReportService _agentrReportServices;

        public SuperAgentController(IAgentCredentialsService agentCredentialsService,
            INotyfService notyfService,
            ICashAgentUserService cashAgentUserService,
            ICommonddlServices commonddlServices,
            ICashAgentCommissionService cashAgentCommissionService,
            IMapper mapper,
            IAgentReportService agentrReportServices)
        {
            _agentCredentialsService = agentCredentialsService;
            _notyfService = notyfService;
            _cashAgentUserService = cashAgentUserService;
            _commonddlService = commonddlServices;
            _cashAgentCommissionService = cashAgentCommissionService;
            _mapper = mapper;
            _agentrReportServices = agentrReportServices;
        }

        public async Task<IActionResult> Index([FromQuery] AgentFilter AgentFilter)
        {
            var districtDdl = await _commonddlService.GetAllDistrictddl();
            ViewBag.DistrictDdl = new SelectList(districtDdl, "value", "Text");

            var data = await _cashAgentUserService.GetAgentUserAsync(AgentFilter);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_AgentUserList", data);
            }
            return View(data);
        }


        [HttpGet]
        [ActionName("AgentListBySuperAgent")]
        public async Task<IActionResult> AgentListBySuperAgentIndex([FromQuery] AgentFilter AgentFilter)
        {
            var districtDdl = await _commonddlService.GetAllDistrictddl();
            ViewBag.DistrictDdl = new SelectList(districtDdl, "value", "Text");

            var data = await _cashAgentUserService.GetAgentBySuperAgentAsync(AgentFilter);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_AgentListBySuperAgentUserList", data);
            }
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> AgentListBySuperAgent(AgentFilter AgentFilter)
        {
            var districtDdl = await _commonddlService.GetAllDistrictddl();
            ViewBag.DistrictDdl = new SelectList(districtDdl, "value", "Text");

            ViewBag.SuperAgentCode = AgentFilter.SuperAgentCode;
            //ViewBag.AgentCode = AgentFilter.AgentCode;
            ViewBag.FullName = AgentFilter.AgentName;

            var data = await _cashAgentUserService.GetAgentBySuperAgentAsync(AgentFilter);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_AgentUserList", data);
            }
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAgentDetailsByAgentCode(string AgentCode)
        {
            var data = await _agentCredentialsService.GetDetailsByAgentCode(AgentCode);
            return PartialView("_AgentDetailsByAgentCode", data);
        }



        [HttpGet]
        public async Task<IActionResult> AddFundRequest(string agentCode, string superAgentName)
        {
            var result = await _cashAgentUserService.GetAgentPrefundByAgentCode(agentCode);
            var fundRequest = new AddAgentFundRequestVm
            {
                AgentCode = agentCode,
                SuperAgentName = superAgentName
            };
            if (result != null)
            {
                fundRequest.NotificationBalance = (decimal)(result.NotificationBalance != null ? result.NotificationBalance : 0m);
            }
            return await Task.FromResult(PartialView(fundRequest));
        }

        [HttpPost]
        [LogUserActivity("added agent fund request")]
        public async Task<IActionResult> AddFundRequest(AddAgentFundRequestVm addUpdateFundRequest)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView(addUpdateFundRequest);
            }
            addUpdateFundRequest.FundType = "PREFUNDING";
            addUpdateFundRequest.SourceCurrency = "NPR";
            var mappeddata = _mapper.Map<AddAgentFundRequest>(addUpdateFundRequest);
            var addFundStatus = await _cashAgentUserService.AddFundRequestAsync(mappeddata, User);
            if (!addFundStatus.Success)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ViewBag.Error = addFundStatus.Errors.First();
                return PartialView(addUpdateFundRequest);
            }

            _notyfService.Success(addFundStatus.Message);
            return Ok();
        }

        public async Task<IActionResult> AgentLedger(AgentLedgerFilter AgentLedgerFilter)
        {
            var data = await _cashAgentUserService.GetAgentLedgerAsync(AgentLedgerFilter);
            ViewBag.AgentCode = AgentLedgerFilter.AgentCode;
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_AgentLedger", data);
            }
            return View(data);
        }

        [HttpGet("ExportAgentLedgerToCsv")]
        public async Task<IActionResult> ExportAgentLedgerToCsv(AgentLedgerFilter request)
        {
            request.Export = 1;
            var result = await _cashAgentUserService.GetAgentLedgerAsync(request);
            var data = result.Items;
            var (bytes, fileformate, filename) = ExportHelper.GenerateCsv(data, new string[] { }, null, "AgentLedgerReports", true);
            return File(bytes, fileformate, filename);
        }

        [HttpGet("ExportAgentLedgerToExcel")]
        public async Task<IActionResult> ExportAgentLedgerToExcel(AgentLedgerFilter request)
        {
            request.Export = 1;
            var result = await _cashAgentUserService.GetAgentLedgerAsync(request);
            var data = result.Items;
            List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync(data, 500000);
            var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "AgentLedgerReports", true);
            return File(excelFileByteArr, fileFormat, fileName);
        }

        [HttpGet("ExportAgentLedgerToPdf")]
        public async Task<FileContentResult> ExportAgentLedgerToPdf(AgentLedgerFilter request)
        {
            request.Export = 1;
            var result = await _cashAgentUserService.GetAgentLedgerAsync(request);
            var data = result;
            var (bytedata, format) = await ExportHelper.TopdfAsync(data, "AgentLedgerReports");
            return File(bytedata, format, "AgentLedgerReport.pdf");
        }

        [HttpGet]
        public async Task<IActionResult> AddAgent()
        {
            var districtList = await _commonddlService.GetAllDistrictddl();
            var agentListddl = await _commonddlService.GetAgentListddl();
            var agentCategory = await _commonddlService.GetServiceChargeCategoryddl();
            ViewBag.AgentListDdl = new SelectList(agentListddl, "value", "Text");
            ViewBag.District = new SelectList(districtList, "value", "Text");
            ViewBag.AgentCategory = new SelectList(agentCategory, "value", "Text");

            return PartialView("_AddAgent");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAgent(string AgentCode)
        {
            var districtList = await _commonddlService.GetAllDistrictddl();
            var agentListddl = await _commonddlService.GetAgentListddl();
            var agentCategory = await _commonddlService.GetServiceChargeCategoryddl();
            ViewBag.AgentListDdl = new SelectList(agentListddl, "value", "Text");
            ViewBag.District = new SelectList(districtList, "value", "Text");
            ViewBag.AgentCategory = new SelectList(agentCategory, "value", "Text");

            if (!string.IsNullOrEmpty(AgentCode))
            {
                ViewBag.AgentCode = AgentCode;
                var getUpdatedList = await _cashAgentUserService.GetCashAgentByAgentCodeAsync(AgentCode);
                var mapObject = MapToCashAgentUserVm(getUpdatedList);
                return PartialView("_UpdateAgent", mapObject);
            }
            return PartialView("_UpdateAgent");
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyUserName(string userName)
        {
            if (!await _cashAgentUserService.VerifyUserName(userName))
            {
                return Json($"User Name {userName} is already in use.");
            }
            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyContactNumber(string contactNumber)
        {
            if (!await _cashAgentUserService.VerifyContactNumber(contactNumber))
            {
                return Json($"Contact Number is already in use.");
            }
            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyRegistrationNumber(string registrationNumber)
        {
            if (!await _cashAgentUserService.VerifyRegistrationNumber(registrationNumber))
            {
                return Json($"Registration Number is already in use.");
            }
            return Json(true);
        }

        [HttpPost]
        [LogUserActivity("added new agent")]
        public async Task<IActionResult> AddAgent([FromForm] CashAgentUserVm CashAgentVm)
        {
            var addResult = (dynamic)null;
            var districtList = await _commonddlService.GetAllDistrictddl();
            var agentListddl = await _commonddlService.GetAgentListddl();
            ViewBag.District = new SelectList(districtList, "value", "Text");
            ViewBag.AgentListDdl = new SelectList(agentListddl, "value", "Text");
            var agentCategory = await _commonddlService.GetServiceChargeCategoryddl();
            ViewBag.AgentCategory = new SelectList(agentCategory, "value", "Text");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_AddAgent", CashAgentVm);
            }

            addResult = await _cashAgentUserService.AddAgentUserAsync(CashAgentVm);

            if (!addResult.Success)
            {
                Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                ViewBag.Error = addResult.Errors;
                return PartialView("_AddAgent", CashAgentVm);
            }
            _notyfService.Success(addResult.Message);
            return Ok();
        }

        [HttpPost]
        [LogUserActivity("updated a agent")]
        public async Task<IActionResult> UpdateAgent([FromForm] CashAgentUpdateVm CashAgentVm)
        {
            var updateResult = (dynamic)null;
            var districtList = await _commonddlService.GetAllDistrictddl();
            var agentListddl = await _commonddlService.GetAgentListddl();
            ViewBag.District = new SelectList(districtList, "value", "Text");
            ViewBag.AgentListDdl = new SelectList(agentListddl, "value", "Text");
            var agentCategory = await _commonddlService.GetServiceChargeCategoryddl();
            ViewBag.AgentCategory = new SelectList(agentCategory, "value", "Text");
            ViewBag.AgentCode = CashAgentVm.AgentCode;

            updateResult = await _cashAgentUserService.UpdateAgentUserAsync(CashAgentVm);

            if (!updateResult.Success)
            {
                Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                ViewBag.Error = updateResult.Errors;
                return PartialView("_UpdateAgent", CashAgentVm);
            }
            _notyfService.Success(updateResult.Message);
            return Ok();
        }

        public async Task<IActionResult> ActivateUser(string AgentCode, bool isActive)
        {
            var user = new ActivateAgent()
            {
                AgentCode = AgentCode,
                IsActive = isActive
            };
            return PartialView(user);
        }

        public async Task<IActionResult> MarkasSuperAgent(string AgentCode, bool isActive)
        {
            var user = new ActivateAgent()
            {
                AgentCode = AgentCode,
                IsActive = isActive
            };
            return PartialView(user);
        }

        [HttpPost]
        [LogUserActivity("marked agent as superagent")]
        public async Task<IActionResult> MarkasSuperAgent(ActivateAgent activateAgent)
        {
            if (activateAgent != null && !string.IsNullOrEmpty(activateAgent.AgentCode))
            {
                var ResponseStatus = await _cashAgentUserService.MarkasSuperAgent(activateAgent);
                if (ResponseStatus.StatusCode == 200)
                {
                    _notyfService.Success(ResponseStatus.MsgText);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    TempData["Error"] = ResponseStatus.MsgText;
                    return PartialView(activateAgent);
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return PartialView(activateAgent);
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddCommissionRules([Required] string AgentCode, string agentType)
        {
            var agentCommissionDtl = await _cashAgentCommissionService.GetAgentCommissionDetailsAsync(AgentCode, agentType);
            if (agentCommissionDtl is null)
            {
                //_notyfService.Error("Commission rules not defined for the agent!");
                return BadRequest();
            }

            var vm = new AddOrUpdateCommissionVM
            {
                AgentCode = agentCommissionDtl.AgentCode,
                SuperAgentCode = agentCommissionDtl.SuperAgentCode,
                AgentType = agentCommissionDtl.AgentType,
                CommissionRules = agentCommissionDtl?.CommissionRuleList is null || !agentCommissionDtl.CommissionRuleList.Any()
                ? new List<AgentCommissionRule> { new() { AgentCode = AgentCode, FromDate = DateTime.Now, ToDate = DateTime.Now } }
                : agentCommissionDtl.CommissionRuleList.ToList()
            };

            return PartialView("_AddCommissionRules", vm);
        }

        [HttpPost]
        [LogUserActivity("added commission rules to agent")]
        public async Task<IActionResult> AddCommissionRules(AddOrUpdateCommissionVM model)
        {
            var agentCommissionRule = await _cashAgentCommissionService.GetCommissionSlabsBySuperAgent(model.AgentCode);

            foreach (var newRule in model.CommissionRules)
            {
                foreach (var agentRule in agentCommissionRule)
                {
                    bool isOverlapping = newRule.MinTxnCount <= agentRule.MaxTxnCount && newRule.MaxTxnCount >= agentRule.MinTxnCount;

                    if (isOverlapping && newRule.Commission < agentRule.Commission)
                    {
                        Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                        ViewBag.Error = $"SuperAgent's commission for transactions between {newRule.MinTxnCount} and {newRule.MaxTxnCount} cannot be lower than agent's commission for transactions between {agentRule.MinTxnCount} and {agentRule.MaxTxnCount}.";
                        return PartialView("_AddCommissionRules", model);
                    }
                }
            }

            var rules = model.CommissionRules
                .Select(x => _mapper.Map<AgentCommissionRuleType>(x))
                .ToList();

            var addUpdateResult = await _cashAgentCommissionService.AddOrUpdateAsync(rules, model.AgentCode, model.SuperAgentCode, model.AgentType, "ADMIN");
            if (!addUpdateResult.Success)
            {
                Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                ViewBag.Error = addUpdateResult.Errors.First();

                var agentCommissionDtl = await _cashAgentCommissionService.GetAgentCommissionDetailsAsync(model.AgentCode, null);

                var vm = new AddOrUpdateCommissionVM
                {
                    AgentCode = agentCommissionDtl.AgentCode ?? model.AgentCode,
                    SuperAgentCode = agentCommissionDtl?.SuperAgentCode,
                    AgentType = agentCommissionDtl?.AgentType,
                    CommissionRules = agentCommissionDtl?.CommissionRuleList is null || !agentCommissionDtl.CommissionRuleList.Any()
                    ? new List<AgentCommissionRule> { new() { AgentCode = agentCommissionDtl.AgentCode, FromDate = DateTime.Now, ToDate = DateTime.Now } }
                    : agentCommissionDtl.CommissionRuleList.ToList()
                };

                return PartialView("_AddCommissionRules", vm);
            }

            _notyfService.Success(addUpdateResult.Message);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> CommissionSettings()
        {
            var defaultCommissionRules = await _cashAgentCommissionService.GetAgentDefaultCommissionDetailsAsync();

            var vm = !defaultCommissionRules.Any()
                ? new List<AgentDefaultCommissionRule> { new() { FromDate = DateTime.Now, ToDate = DateTime.Now } }
                : defaultCommissionRules.ToList();

            return View(vm);
        }

        [HttpPost]
        [LogUserActivity("updated commission settings")]
        public async Task<IActionResult> CommissionSettings(List<AgentDefaultCommissionRule> commissionRules)
        {
            var rules = commissionRules
                .Select(x => _mapper.Map<AgentCommissionRuleType>(x))
                .ToList();

            var addUpdateResult = await _cashAgentCommissionService.AddOrUpdateDefaultRulesAsync(rules, "ADMIN");
            if (!addUpdateResult.Success)
            {
                var defaultCommissionRules = await _cashAgentCommissionService.GetAgentDefaultCommissionDetailsAsync();

                var vm = !defaultCommissionRules.Any()
                    ? new List<AgentDefaultCommissionRule> { new() { FromDate = DateTime.Now, ToDate = DateTime.Now } }
                    : defaultCommissionRules.ToList();

                _notyfService.Error(addUpdateResult.Errors.First());
                return View(vm);
            }

            _notyfService.Success(addUpdateResult.Message);
            return View(commissionRules);
        }

        [HttpPost]
        [LogUserActivity("activated a agent")]
        public async Task<IActionResult> ActivateUser(ActivateAgent activateAgent)
        {
            if (activateAgent != null && !string.IsNullOrEmpty(activateAgent.AgentCode))
            {
                var ResponseStatus = await _cashAgentUserService.ActivateAgentUserAsync(activateAgent);
                if (ResponseStatus.StatusCode == 200)
                {
                    _notyfService.Success(ResponseStatus.MsgText);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    TempData["Error"] = ResponseStatus.MsgText;
                    return PartialView(activateAgent);
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return PartialView(activateAgent);
            }
        }

        public async Task<IActionResult> DeleteUser(string AgentCode)
        {
            var user = new ActivateAgent()
            {
                AgentCode = AgentCode,
            };
            return PartialView(user);
        }

        [HttpPost]
        [LogUserActivity("deleted a agent")]
        public async Task<IActionResult> DeleteUser(ActivateAgent activateAgent)
        {
            if (activateAgent != null && !string.IsNullOrEmpty(activateAgent.AgentCode))
            {
                var ResponseStatus = await _cashAgentUserService.DeleteUser(activateAgent);
                if (ResponseStatus.StatusCode == 200)
                {
                    _notyfService.Success(ResponseStatus.MsgText);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    ViewBag.Error = ResponseStatus.MsgText;
                    return PartialView(activateAgent);
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return PartialView(activateAgent);
            }
        }

        public async Task<IActionResult> Withdraw(string AgentCode)
        {
            var user = new Withdraw()
            {
                AgentCode = AgentCode,
            };
            return PartialView(user);
        }

        [HttpPost]
        [LogUserActivity("Withdraw prefund")]
        public async Task<IActionResult> Withdraw(Withdraw withdraw)
        {
            if (withdraw != null && !string.IsNullOrEmpty(withdraw.AgentCode))
            {
                var ResponseStatus = await _cashAgentUserService.Withdraw(withdraw, User);
                if (ResponseStatus.StatusCode == 200)
                {
                    _notyfService.Success(ResponseStatus.MsgText);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    ViewBag.Error = ResponseStatus.MsgText;
                    return PartialView(withdraw);
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return PartialView(withdraw);
            }
        }

        private CashAgentUpdateVm MapToCashAgentUserVm(AgentUser details)
        {
            ArgumentNullException.ThrowIfNull(details, nameof(details));

            return new CashAgentUpdateVm
            {
                AgentCode = details.AgentCode,
                FirstName = details.FirstName,
                SuperAgentCode = details.SuperAgentCode,
                LastName = details.LastName,
                Email = details.Email,
                AgentCategoryId = details.AgentCategoryId,
                LicensedocImgPath = details.LicensedocImgPath,
                UserName = details.UserName,
                UserType = details.UserType,
                ContactNumber = details.ContactNumber,
                FullAddress = details.FullAddress,
                DistrictCode = details.DistrictCode,
                DocumentImagepath = details.CompanyLogoImgPath,
                OrganizationName = details.OrganizationName,
                RegistrationNumber = details.RegistrationNumber,
                IsActive = details.IsActive,
                IsPrefunding = details.IsPrefunding,
            };
        }

        [HttpGet]
        public async Task<IActionResult> AgentSettlementReport(AgentStatementFilter model, string AgentCode)
        {
            model.StartDate = string.IsNullOrEmpty(model.StartDateBS) ? model.StartDate : model.StartDateBS;
            model.EndDate = string.IsNullOrEmpty(model.EndDateBS) ? model.EndDate : model.EndDateBS;
            model.AgentCode = AgentCode;
            ViewBag.AgentCode = AgentCode;
            var data = await _cashAgentUserService.GetAgentAccountSettlementReport(model);
            if (model.Export == 1)
            {
                var datas = data.Items;
                List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<AgentAccountStatement>(datas, 500000);
                var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "AgentAccountSettlement", true);
                return File(excelFileByteArr, fileFormat, fileName);
            }
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_AgentSettlement", data));
            return await Task.FromResult(View(data));
        }

        #region credentials

        public async Task<IActionResult> AddApiKey(string AgentCode)
        {
            if (!string.IsNullOrEmpty(AgentCode))
            {
                var apikey = new AddApiKeysAgent()
                {
                    AgentCode = AgentCode,
                };
                return await Task.FromResult(PartialView("_AddAgentApiCredentials", apikey));
            }
            _notyfService.Error("Agent Not Found!");
            return View();
        }

        [HttpPost]
        [LogUserActivity("added credentials to a Agent")]
        public async Task<IActionResult> AddApiKey(AddApiKeysAgent apikey)
        {
            if (ModelState.IsValid)
            {
                var addapikeys = _mapper.Map<AgentCredentialInsertRequest>(apikey);
                addapikeys.AgentCode = apikey.AgentCode;
                var addResult = await _agentCredentialsService.AddCredentialsAsync(addapikeys);
                if (addResult.Success)
                {
                    _notyfService.Success(addResult.Message);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                    ViewBag.Error = addResult.Errors.FirstOrDefault();
                    return PartialView("_AddAgentApiCredentials", apikey);
                }
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView("_AddAgentApiCredentials", apikey);
        }


        [HttpGet]
        public async Task<IActionResult> UpdateIpAddress(string AgentCode)
        {
            if (!string.IsNullOrEmpty(AgentCode))
            {
                var data = await _agentCredentialsService.GetCredentialsByAgentCodeAsync(AgentCode);
                string[] ipAddressArray = data.IPAddress.Split(',');
                var mappeddata = new AgentCredentialUpdateRequest
                {
                    AgentCode = data.AgentCode,
                    ApiUserName = data.ApiUserName,
                    IsActive = data.IsActive,
                    CredentialId = data.CredentialId,
                    IPAddress = ipAddressArray
                };

                //   var data1 =  _mapper.Map<AgentCredentialUpdateRequest>(data);
                return await Task.FromResult(PartialView("_UpdateIpAddress", mappeddata));
            }
            _notyfService.Error("Agent Not Found!");
            return View();
        }

        [HttpPost]
        [LogUserActivity("updated agent IP address")]
        public async Task<IActionResult> UpdateIpAddress(AgentCredentialUpdateRequest agentCredential)
        {
            if (ModelState.IsValid)
            {
                var updateApikey = _mapper.Map<AgentCredentialUpdateRequest>(agentCredential);
                var data = await _agentCredentialsService.UpdateCredentialsAsync(updateApikey);
                _notyfService.Success(data.Message);
                return Ok();
            }
            _notyfService.Error("Agent Not Found!");
            return View();
        }


        //public async Task<IActionResult> UpdateApiKey(string AgentCode, string CredentialId)
        //{
        //    if (!string.IsNullOrEmpty(AgentCode))
        //    {
        //        var apikey = new AddApiKeysAgent()
        //        {
        //            AgentCode = AgentCode,

        //        };
        //        return await Task.FromResult(PartialView("_AddAgentApiCredentials", apikey));
        //    }
        //    _notyfService.Error("Agent Not Found!");
        //    return View();
        //}
        //[HttpPost]
        //[LogUserActivity("updated partner credentials")]
        //public async Task<IActionResult> UpdateCredentialsPartner(UpdateApiKeysAgent apikey)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var credential = await _agentCredentialsService.GetCredentialsByAgentCodeAsync(apikey.AgentCode);
        //        var addapikeys = _mapper.Map<AgentCredentialUpdateRequest>(apikey);
        //        addapikeys.ApiUserName = credential.ApiUserName;
        //        addapikeys.CredentialId = credential.CredentialId;
        //        var updateResult = await _agentCredentialsService.UpdateCredentialsAsync(addapikeys);
        //        if (updateResult.Success)
        //        {
        //            _notyfService.Success(updateResult.Message);
        //            return Ok();
        //        }
        //        else
        //        {
        //            Response.StatusCode = (int)(HttpStatusCode.BadRequest);
        //            ViewBag.Error = updateResult.Errors.FirstOrDefault();
        //            return PartialView("UpdateCredentials", apikey);
        //        }
        //    }
        //    Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //    return PartialView("UpdateCredentials", apikey);
        //}

        [HttpGet]
        public async Task<IActionResult> ViewCredentialsAgent(string AgentCode)
        {
            if (!string.IsNullOrEmpty(AgentCode))
            {
                var credential = await _agentCredentialsService.GetCredentialsByAgentCodeAsync(AgentCode);
                if (credential != null)
                {
                    var apikey = new UpdateApikeyAgentVM()
                    {
                        CredentialId = credential.CredentialId,
                        Apikey = credential.ApiKey,
                        AgentCode = credential.AgentCode,
                    };

                    return await Task.FromResult(PartialView("ViewAgentApiKeys", apikey));
                }
            }
            //display error view
            return await Task.FromResult(PartialView("Error"));
        }

        [HttpPost]
        [LogUserActivity("updated agent credentials")]
        public async Task<IActionResult> ViewCredentialsAgent(UpdateApikeyAgentVM ViewModel)
        {
            if (ViewModel != null)
            {
                if (!String.IsNullOrEmpty(ViewModel.AgentCode) && !String.IsNullOrEmpty(ViewModel.CredentialId))
                {
                    var (status, Apikey) = await _agentCredentialsService.RegenerateApiKeyAsync(ViewModel.AgentCode, ViewModel.CredentialId);
                    if (status.StatusCode == 200)
                    {
                        var apikey = new UpdateApikeyAgentVM()
                        {
                            CredentialId = ViewModel.CredentialId,
                            Apikey = Apikey,
                            AgentCode = ViewModel.AgentCode,
                        };
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        _notyfService.Success(status.MsgType);
                        return await Task.FromResult(PartialView("ViewAgentApiKeys", apikey));
                    }
                    _notyfService.Success(status.MsgType);
                    return Ok();
                }
            }
            _notyfService.Success("Agent Not Found!");
            return Ok();
        }

        [HttpPost]
        [LogUserActivity("exported Agent credentials")]
        public async Task<IActionResult> ExportCredential([FromBody] ExportCredentialsAgentModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(model.AgentCode))
                return BadRequest(ModelState);

            var credential = await _agentCredentialsService.GetCredentialsByAgentCodeAsync(model.AgentCode);
            if (credential is null)
                return BadRequest();

            switch (model.CredentialType.ToUpper())
            {
                case "USERPRIVATEKEY":
                    if (string.IsNullOrWhiteSpace(credential.UserPrivateKey))
                        return BadRequest();

                    var userPrivateKeyBytes = Encoding.UTF8.GetBytes(credential.UserPrivateKey);

                    Response.Headers.Add("Content-Disposition", "attachment; filename=" + "UserPrivateKey.pem");
                    Response.Headers.Add("Content-Type", "application/x-pem-file");

                    return File(userPrivateKeyBytes, "application/x-pem-file");

                case "USERPUBLICKEY":
                    if (string.IsNullOrWhiteSpace(credential.UserPublicKey))
                        return BadRequest();

                    var userPublicKeyBytes = Encoding.UTF8.GetBytes(credential.UserPublicKey);

                    Response.Headers.Add("Content-Disposition", "attachment; filename=" + "UserPublicKey.pem");
                    Response.Headers.Add("Content-Type", "application/x-pem-file");

                    return File(userPublicKeyBytes, "application/x-pem-file");

                case "SYSTEMPRIVATEKEY":
                    if (string.IsNullOrWhiteSpace(credential.SystemPrivateKey))
                        return BadRequest();

                    var systemPrivateKeyBytes = Encoding.UTF8.GetBytes(credential.SystemPrivateKey);

                    Response.Headers.Add("Content-Disposition", "attachment; filename=" + "SystemPrivateKey.pem");
                    Response.Headers.Add("Content-Type", "application/x-pem-file");

                    return File(systemPrivateKeyBytes, "application/x-pem-file");

                case "SYSTEMPUBLICKEY":
                    if (string.IsNullOrWhiteSpace(credential.SystemPublicKey))
                        return BadRequest();

                    var systemPublicKeyBytes = Encoding.UTF8.GetBytes(credential.SystemPublicKey);

                    Response.Headers.Add("Content-Disposition", "attachment; filename=" + "SystemPublicKey.pem");
                    Response.Headers.Add("Content-Type", "application/x-pem-file");

                    return File(systemPublicKeyBytes, "application/x-pem-file");

                default:
                    return BadRequest();
            };
        }

        [HttpGet]
        public async Task<IActionResult> ViewPasswordCredentialsAgent(string AgentCode)
        {
            if (!string.IsNullOrEmpty(AgentCode))
            {
                var credential = await _agentCredentialsService.GetCredentialsByAgentCodeAsync(AgentCode);
                if (credential != null)
                {
                    var apikey = new UpdateApiPasswordAgentVM()
                    {
                        CredentialId = credential.CredentialId,
                        ApiPassword = credential.ApiPassword,
                        AgentCode = credential.AgentCode,
                        ApiUserName = credential.ApiUserName,
                    };

                    return await Task.FromResult(PartialView("ViewAgentApiPassword", apikey));
                }
            }
            //display error view
            return await Task.FromResult(PartialView("Error"));
        }

        [HttpPost]
        [LogUserActivity("updated agent API password")]
        public async Task<IActionResult> ViewPasswordCredentialsAgent(UpdateApiPasswordAgentVM ViewModel)
        {
            if (ViewModel != null)
            {
                if (!string.IsNullOrEmpty(ViewModel.AgentCode) && !string.IsNullOrEmpty(ViewModel.CredentialId))
                {
                    var credential = await _agentCredentialsService.GetCredentialsByAgentCodeAsync(ViewModel.AgentCode);

                    if (ViewModel.CredentialId != credential.CredentialId)
                    {
                        _notyfService.Success("Agent Not Found!");
                        return Ok();
                    }

                    var (status, apiPassword) = await _agentCredentialsService.RegenerateApiPasswordAsync(ViewModel.AgentCode, ViewModel.CredentialId);
                    if (status.StatusCode == 200)
                    {
                        var apikey = new UpdateApiPasswordAgentVM()
                        {
                            CredentialId = ViewModel.CredentialId,
                            ApiPassword = apiPassword,
                            AgentCode = ViewModel.AgentCode,
                            ApiUserName = credential.ApiUserName,
                        };

                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        _notyfService.Success(status.MsgType);
                        return await Task.FromResult(PartialView("ViewAgentApiPassword", apikey));
                    }
                    _notyfService.Success(status.MsgType);
                    return Ok();
                }
            }
            _notyfService.Success("Agent Not Found!");
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ViewPasswordSystemRsakeyPair(string AgentCode)
        {
            if (!string.IsNullOrEmpty(AgentCode))
            {
                var credential = await _agentCredentialsService.GetCredentialsByAgentCodeAsync(AgentCode);
                if (credential != null)
                {
                    var apikey = new SystemRsakeyPairAgentVM()
                    {
                        CredentialId = credential.CredentialId,
                        PublicKey = credential.SystemPublicKey,
                        PrivetKey = credential.SystemPrivateKey,
                        AgentCode = credential.AgentCode,
                    };

                    return await Task.FromResult(PartialView("ViewAgentSystemRsaKeyPair", apikey));
                }
            }
            //display error view
            return await Task.FromResult(PartialView("Error"));
        }

        [HttpPost]
        [LogUserActivity("changed system RSA key pair")]
        public async Task<IActionResult> ViewPasswordSystemRsakeyPair(SystemRsakeyPairAgentVM ViewModel)
        {
            if (ViewModel != null)
            {
                if (!string.IsNullOrEmpty(ViewModel.AgentCode) && !string.IsNullOrEmpty(ViewModel.CredentialId))
                {
                    var (status, privatekey, publickey) = await _agentCredentialsService.RegenerateSystemRsaKeyPairAsync(ViewModel.AgentCode, ViewModel.CredentialId);
                    if (status.StatusCode == 200)
                    {
                        var apikey = new SystemRsakeyPairAgentVM()
                        {
                            CredentialId = ViewModel.CredentialId,
                            PublicKey = publickey,
                            PrivetKey = privatekey,
                            AgentCode = ViewModel.AgentCode,
                        };

                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        _notyfService.Success(status.MsgType);
                        return await Task.FromResult(PartialView("ViewAgentSystemRsaKeyPair", apikey));
                    }
                    _notyfService.Success(status.MsgType);
                    return Ok();
                }
            }
            _notyfService.Success("Agent Not Found!");
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ViewPasswordUserRsakeyPair(string AgentCode)
        {
            if (!string.IsNullOrEmpty(AgentCode))
            {
                var credential = await _agentCredentialsService.GetCredentialsByAgentCodeAsync(AgentCode);
                if (credential != null)
                {
                    var apikey = new SystemRsakeyPairAgentVM()
                    {
                        CredentialId = credential.CredentialId,
                        PublicKey = credential.UserPublicKey,
                        PrivetKey = credential.UserPrivateKey,
                        AgentCode = credential.AgentCode,
                    };

                    return await Task.FromResult(PartialView("ViewAgentUserRsaKeyPair", apikey));
                }
            }
            //display error view
            return await Task.FromResult(PartialView("Error"));
        }

        [HttpPost]
        [LogUserActivity("changed user RSA key pair")]
        public async Task<IActionResult> ViewPasswordUserRsakeyPair(SystemRsakeyPairAgentVM ViewModel)
        {
            if (ViewModel != null)
            {
                if (!string.IsNullOrEmpty(ViewModel.AgentCode) && !string.IsNullOrEmpty(ViewModel.CredentialId))
                {
                    var (status, privatekey, publickey) = await _agentCredentialsService.RegenerateUserRsaKeyPairAsync(ViewModel.AgentCode, ViewModel.CredentialId);
                    if (status.StatusCode == 200)
                    {
                        var apikey = new SystemRsakeyPairAgentVM()
                        {
                            CredentialId = ViewModel.CredentialId,
                            PrivetKey = privatekey,
                            PublicKey = publickey,
                            AgentCode = ViewModel.AgentCode,
                        };

                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        _notyfService.Success(status.MsgType);
                        return await Task.FromResult(PartialView("ViewAgentUserRsaKeyPair", apikey));
                    }
                    _notyfService.Success(status.MsgType);
                    return Ok();
                }
            }
            _notyfService.Success("Partner Not Found!");
            return Ok();
        }

        #endregion credentials

        #region agent fund request

        [HttpGet]
        public async Task<IActionResult> ReqtFundByAgent(AgentFundRequestFilter model)
        {
            var requestStatus = await _commonddlService.GetStatusListDdl();
            ViewBag.RequestStatus = new SelectList(requestStatus, "value", "Text");
            var details = await _agentrReportServices.GetAgentFundRequestReportAsync(model);
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_AgentFundRequestListForAdmin", details));

            return await Task.FromResult(View(details));
        }

        [HttpGet]
        public async Task<IActionResult> ApproveFundRequestByAgent(string AgentCode, string FundRequestId)
        {
            AgentFundApproveRejectModel model = new AgentFundApproveRejectModel();
            model.AgentCode = AgentCode;
            model.RequestFundId = FundRequestId;
            var getDetails = await _agentrReportServices.GetDetailsRequestFundAsync(model);
            if (getDetails is not null)
            {
                getDetails.RequestFundId = FundRequestId;
                getDetails.AgentCode = AgentCode;
                getDetails.TotalAmount = Math.Round(getDetails.TotalAmount, 2);
            }
            return PartialView("_ApproveRejectFundRequest", getDetails);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveFundRequestByAgent(AgentFundApproveRejectModel model)
        {
            if((model.IsCommissionRequested && model.IsTxnCashRequested) || (!model.IsCommissionRequested && !model.IsTxnCashRequested))
            {
                _notyfService.Error("Invalid Request. Either Receivalbe amount or Commission amount can be requested at a time!");
                return RedirectToAction("ReqtFundByAgent", "SuperAgent");
            }

            var mappedData = _mapper.Map<ApproveRejectFundTransferByAdmin>(model);
            var response = await _agentrReportServices.ApproveRejectAgentFundRequestAysnc(mappedData);
            if (response.StatusCode != 200)
            {
                _notyfService.Error(response.MsgText);
                return RedirectToAction("ReqtFundByAgent", "SuperAgent");
            }
            else
            {
                _notyfService.Success(response.MsgText);
                return RedirectToAction("ReqtFundByAgent", "SuperAgent");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ReviewFundRequestByAgent(string AgentCode, string FundRequestId)
        {
            AgentFundApproveRejectModel model = new AgentFundApproveRejectModel();
            model.AgentCode = AgentCode;
            model.RequestFundId = FundRequestId;
            var getDetails = await _agentrReportServices.GetDetailsRequestFundAsync(model);
            if (getDetails is not null)
            {
                getDetails.RequestFundId = FundRequestId;
                getDetails.AgentCode = AgentCode;
                getDetails.TotalAmount = Math.Round(getDetails.TotalAmount, 2);
            }
            return PartialView("_ReviewFundRequestByAgent", getDetails);
        }

        [HttpPost]
        public async Task<IActionResult> ReviewFundRequestByAgent(AgentFundApproveRejectModel model)
        {
            var mappedData = _mapper.Map<ApproveRejectFundTransferByAdmin>(model);
            var response = await _agentrReportServices.ReviewAgentFundRequestAysnc(mappedData);
            if (response.StatusCode != 200)
            {
                _notyfService.Error(response.MsgText);
                return RedirectToAction("ReqtFundByAgent", "SuperAgent");
            }
            else
            {
                _notyfService.Success(response.MsgText);
                return RedirectToAction("ReqtFundByAgent", "SuperAgent");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ApprovedFundRequestList()
        {
            return null;
        }

        #endregion agent fund request

        [HttpGet]
        public async Task<IActionResult> AgentUnsettledAmountSummary()
        {
            var result = await _agentrReportServices.GetAgentUnsettledAmountSummaryList();
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_AgentUnsettledAmountSummary", result));
            return await Task.FromResult(View(result));
        }

        [HttpGet]
        public async Task<IActionResult> UploadBulkAgent()
        {
            return await Task.FromResult(PartialView("_UploadBulkAgent"));
        }

        [HttpPost]
        public async Task<IActionResult> UploadBulkAgent(BulkAgent model)
        {
            var bulkAgent = new List<BulkAgentDetailModel>();

            if (model.UploadBulkAgentFile == null || model.UploadBulkAgentFile.Length == 0)
            {
                return BadRequest("Please select a file");
            }

            if (model.UploadBulkAgentFile.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) || model.UploadBulkAgentFile.FileName.EndsWith(".xls", StringComparison.OrdinalIgnoreCase))
            {
                using var stream = model.UploadBulkAgentFile.OpenReadStream();
                using var package = new ExcelPackage(stream);

                var workSheet = package.Workbook.Worksheets[0];
                var rowCount = workSheet.Dimension.Rows;

                var products = new List<BulkAgentDetailModel>();

                for (int row = 2; row <= rowCount; row++)
                {
                    var product = new BulkAgentDetailModel
                    {
                        SN = workSheet.Cells[row, 1].Value?.ToString(),
                        District = workSheet.Cells[row, 2].Value?.ToString(),
                        AgentName = workSheet.Cells[row, 3].Value?.ToString(),
                        Address = workSheet.Cells[row, 4].Value?.ToString()
                    };
                    products.Add(product);
                }
                bulkAgent = products;
            }
            return await Task.FromResult(PartialView("_UploadBulkAgent", bulkAgent));
        }
    }
}