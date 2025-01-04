using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Mpmt.Core.Domain.Partners.SendTransactions;
using Mpmt.Core.Dtos;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Dtos.Transaction;
using Mpmt.Services.Common;
using Mpmt.Services.Hubs;
using Mpmt.Services.Mvc.Filters;
using Mpmt.Services.Partner;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.Notification;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Areas.Partner.Models.SendTransactions;
using Mpmt.Web.Filter;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net;
using System.Security.Claims;
using CsvHelper;
using OfficeOpenXml;
using CsvHelper.Configuration;
using AllOverIt.Patterns.Specification.Extensions;
using System.Data;
using Mpmt.Core.Common;
using Mpmt.Core.Dtos.PartnerReport;
using Mpmt.Core.Extensions;



namespace Mpmt.Web.Areas.Partner.Controllers
{
    /// <summary>
    /// The send transactions controller.
    /// </summary>

    [Authorize]
    [PartnerAuthorization]
    [RolePremission]
    public class SendTransactionsController : BasePartnerController
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IWebHelper _webHelper;
        private readonly IFileProviderService _fileProviderService;
        private readonly ICommonddlServices _commonddlService;
        private readonly ITransactionServices _transactionServices;
        private readonly IPartnerSendTxnsService _partnerSendTxnsService;
        private readonly IPartnerSenderService _partnerSenderService;
        private readonly INotyfService _notify;
        private readonly IRMPService _rMPService;
        private readonly INotificationService _notificationService;
        private readonly IHubContext<UserHub> _hubcontext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendTransactionsController"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="webHelper">The web helper.</param>
        /// <param name="commonddlService">The commonddl service.</param>
        /// <param name="partnerSendTxnsService">The partner send txns service.</param>
        public SendTransactionsController(
            IConfiguration config,
            IMapper mapper,
            IWebHelper webHelper,
            IFileProviderService fileProviderService,
            ICommonddlServices commonddlService,
            ITransactionServices transactionServices,
            IPartnerSendTxnsService partnerSendTxnsService,
            IPartnerSenderService partnerSenderService,
            INotyfService notify,
            IRMPService rMPService
            )
        {

            _config = config;
            _mapper = mapper;
            _webHelper = webHelper;
            _fileProviderService = fileProviderService;
            _commonddlService = commonddlService;
            _transactionServices = transactionServices;
            _partnerSendTxnsService = partnerSendTxnsService;
            _partnerSenderService = partnerSenderService;
            _notify = notify;
            _rMPService = rMPService;
        }

        /// <summary>
        /// Indices the.
        /// </summary>
        /// <returns>An IActionResult.</returns>
        /// 

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] RemitTransactionFilter remitTransactionFilter)
        {
            var actions = await _rMPService.GetPartnerActionPermissionList("SendTransactions");
            ViewBag.actions = actions;

            var currencyddl = await _commonddlService.GetCurrencyddl();
            ViewBag.Currency = new SelectList(currencyddl, "value", "Text");

            var documenttypeddl = await _commonddlService.GetDocumentTypeddl();
            ViewBag.documentTypeDdl = new SelectList(documenttypeddl, "Text","Value");


            var transectiondetail = await _transactionServices.GetRemitTxnAsync(remitTransactionFilter);
            if (Common.WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_SendTransactionTable", transectiondetail));

            return View(transectiondetail);

        }

        [HttpGet]
        public async Task<IActionResult> SenderDetail(string TransactionId)
        {

            var senderdetail = await _transactionServices.GetSenderByTxnId(TransactionId);


            return PartialView("_SenderDetail", senderdetail);

        }

        [HttpGet]
        public async Task<IActionResult> ReciverDetail(string TransactionId)
        {

            var recipientsdetail = await _transactionServices.GetRecipientByTxnId(TransactionId);

            return PartialView("_ReceiverDetail", recipientsdetail);

        }

        [HttpGet]
        public async Task<IActionResult> PaymentDetail(string TransactionId)
        {
            var transectiondetail = await _transactionServices.PaymentDetailsByTxnIdAsync(TransactionId);


            return PartialView("_PaymentType", transectiondetail);

        }

        [HttpGet]
        public async Task<IActionResult> ViewDetail(string TransactionId)
        {
            var transectiondetail = await _transactionServices.GetTxnById(TransactionId);
            return PartialView("_ViewTransactionDetail", transectiondetail);
        }


        /// <summary>
        /// Gets the recipient district list.
        /// </summary>
        /// <param name="provinceCode">The province code.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<JsonResult> GetRecipientDistrictList([FromQuery][Required] string provinceCode)
        {
            var provinceDdl = await _commonddlService.GetDistrictddl(provinceCode);

            var data = new SelectList(provinceDdl, "value", "Text");

            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetRecipientLocalLevelList([FromQuery][Required] string districtCode)
        {
            var districtDdl = await _commonddlService.Getlocallevelddl(districtCode);

            var data = new SelectList(districtDdl, "value", "Text");

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> GetProcessId([FromBody] GetProcessId request)
        {
            var (result, processId) = await _partnerSendTxnsService.GetProcessIdAsync(request);

            if (string.IsNullOrWhiteSpace(processId?.ProcessId) || !result.Success)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var response = new ResponseDto
                {
                    Success = false,
                    Message = "Bad request!",
                    Errors = new() { result.Errors.First() },
                };

                return new JsonResult(response);
            }

            return new JsonResult(new ResponseDto { Success = true, Message = "Ok", Data = processId.ProcessId });
        }

        /// <summary>
        /// Adds the transaction.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> AddTransaction()
        {

            var documentTypeddl = await _commonddlService.GetDocumentTypeddl();
            ViewBag.documentTypeDdl = new SelectList(documentTypeddl, "Lookup", "Text");

            await GetAndSetAddTransactionDdlsAsync();

            if (_webHelper.IsAjaxRequest(Request))
                return PartialView();

            return View();
        }

        private async Task GetAndSetAddTransactionDdlsAsync()
        {
            var partnerCode = User.FindFirstValue("PartnerCode");



            IEnumerable<Commonddl> sourceCurrencyDdl = await _commonddlService.GetPartnerSourceCurrencyddl(partnerCode);
            IEnumerable<Commonddl> destCurrencyDdl = await _commonddlService.GetPartnerDestinationCurrencyddl(partnerCode);
            IEnumerable<Commonddl> countryDdl = await _commonddlService.GetCountryddl();
            IEnumerable<Commonddl> recipientCountryDdl = null;
            IEnumerable<Commonddl> senderCountryDdl = null;
            IEnumerable<Commonddl> recipientProvinceDdl = await _commonddlService.Getprovinceddl("NP");
            IEnumerable<Commonddl> relationshipDdl = await _commonddlService.GetRelationShipddl();
            IEnumerable<Commonddl> transferPurposeDdl = await _commonddlService.Gettransferpurposeddl();
            IEnumerable<Commondropdown> recipientTypeDdl = await _commonddlService.Getrecipienttypeddl();
            IEnumerable<Commondropdown> payoutTypeDdl = await _commonddlService.GetPaymentTypeddl();
            IEnumerable<Commonddl> walletsDdl = await _commonddlService.GetWalletddl();
            IEnumerable<Commondropdown> bankListDdl = await _commonddlService.GetBankddl();
            IEnumerable<DocumentTypeddl> documentTypeDdl = await _commonddlService.GetDocumentTypeddl();
            IEnumerable<Commonddl> senderOccupation = await _commonddlService.GetoccupationAsyncddl();
            IEnumerable<Commonddl> sourceOfIncome = await _commonddlService.GetIncomeSourceAsyncddl();

            recipientCountryDdl = countryDdl.Where(c => c.value.Equals("NP", StringComparison.OrdinalIgnoreCase));
            if (!recipientCountryDdl.Any()) recipientCountryDdl = new List<Commonddl> { new() { Text = "Nepal", value = "NP" } };

            senderCountryDdl = countryDdl.Where(c => !c.value.Equals("NP", StringComparison.OrdinalIgnoreCase));

            ViewBag.EmptyWalletCurrency = (sourceCurrencyDdl.Count() == 0) ? "Please create wallet to proceed." : null;
            ViewBag.SourceCurrencyDdl = new SelectList(sourceCurrencyDdl, "value", "Text");
            ViewBag.DestCurrencyDdl = new SelectList(destCurrencyDdl, "value", "Text");
            ViewBag.CountryDdl = new SelectList(countryDdl, "value", "Text");
            ViewBag.SenderCountryDdl = new SelectList(senderCountryDdl, "value", "Text");
            ViewBag.RecipientCountryDdl = new SelectList(recipientCountryDdl, "value", "Text");
            ViewBag.RecipientProvinceDdl = new SelectList(recipientProvinceDdl, "value", "Text");
            ViewBag.RelationshipDdl = new SelectList(relationshipDdl, "Text", "Text");
            ViewBag.TransferPurposeDdl = new SelectList(transferPurposeDdl, "Text", "Text");
            ViewBag.senderOccupationDdl = new SelectList(senderOccupation, "Text", "Text");
            ViewBag.DocumentTypeDdl = new SelectList(documentTypeDdl, "lookup", "Text");
            ViewBag.sourceOfIncomeDdl = new SelectList(sourceOfIncome, "Text", "Text");
            ViewBag.RecipientTypeDdl = new SelectList(recipientTypeDdl, "lookup", "Text");
            ViewBag.PayoutTypeDdl = new SelectList(payoutTypeDdl, "lookup", "Text");
            ViewBag.WalletsDdl = new SelectList(walletsDdl, "lookup", "Text");
            ViewBag.BankListDdl = new SelectList(bankListDdl, "lookup", "Text");
        }

        /// <summary>
        /// Adds the transaction.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogRequest]
        [LogResponse]
        public async Task<IActionResult> AddTransaction([FromForm] AddTransactionModel model)
        {
            if (model.NoRecipientFirstName == true)
            {
                model.AccountHolderName = model.RecipientLastName;
                model.WalletHolderName = model.RecipientLastName;
            }
            else
            {
                model.AccountHolderName = model.RecipientFirstName + " " + model.RecipientLastName;
                model.WalletHolderName = model.RecipientFirstName + " " + model.RecipientLastName;
            }
            if (string.IsNullOrEmpty(model.SenderFirstName))
                model.NoSenderFirstName = true;

            var txnReq = _mapper.Map<AddTransactionDto>(model);

            var txnAddResult = await _partnerSendTxnsService.AddTransactionAsync(txnReq);

            if (!txnAddResult.Success)
            {
                await GetAndSetAddTransactionDdlsAsync();

                Response.StatusCode = txnAddResult.ResultCode;
                ViewBag.Error = string.Join('\n', txnAddResult.Errors);

                return PartialView(model);
            }

            _notify.Success("Transaction has been successfully sent!");
            return Ok();
        }

        /// <summary>
        /// Gets the conversion amt.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        public async Task<IActionResult> GetConversionAmt([FromBody] GetSendTxnChargeAmountDetailsModel model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;

                var response = new ResponseDto
                {
                    Success = false,
                    Message = "Bad request!",
                    Errors = new() { "Invalid destination currency." },
                };

                return new JsonResult(response);
            }

            var request = _mapper.Map<GetSendTxnChargeAmountDetailsRequest>(model);

            var (status, txnChargeDetails) = await _partnerSendTxnsService.GetSendTxnChargeAmountDetailsAsync(request);

            if (txnChargeDetails != null)
            {
                switch (model.PaymentType)
                {
                    case "BANK":
                        if (decimal.Parse(string.IsNullOrEmpty(txnChargeDetails.BankSendTxnLimitNPR) ? "0" : txnChargeDetails.BankSendTxnLimitNPR) < decimal.Parse(string.IsNullOrEmpty(txnChargeDetails.ReceivingAmountNPR) ? "0" : txnChargeDetails.ReceivingAmountNPR))
                        {
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            return new JsonResult(new ResponseDto { Success = false, Message = "Ok", Errors = new() { "Net Recieving Amount exceeded Total Bank Transfer Amount allowed for the Partner!" } });
                        }
                        break;
                    case "WALLET":
                        if (decimal.Parse(string.IsNullOrEmpty(txnChargeDetails.WalletSendTxnLimitNPR) ? "0" : txnChargeDetails.BankSendTxnLimitNPR) < decimal.Parse(string.IsNullOrEmpty(txnChargeDetails.ReceivingAmountNPR) ? "0" : txnChargeDetails.ReceivingAmountNPR))
                        {
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            return new JsonResult(new ResponseDto { Success = false, Message = "Ok", Errors = new() { "Net Recieving Amount exceeded Total Wallet Transfer Amount allowed for the Partner!" } });
                        }
                        break;
                    case "CASH":
                        if (decimal.Parse(string.IsNullOrEmpty(txnChargeDetails.CashPayoutSendTxnLimitNPR) ? "0" : txnChargeDetails.BankSendTxnLimitNPR) < decimal.Parse(string.IsNullOrEmpty(txnChargeDetails.ReceivingAmountNPR) ? "0" : txnChargeDetails.ReceivingAmountNPR))
                        {
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            return new JsonResult(new ResponseDto { Success = false, Message = "Ok", Errors = new() { "Net Recieving Amount exceeded Total Cash Transfer Amount allowed for the Partner!" } });
                        }
                        break;
                    default:
                        break;
                }
            }

            if (status.StatusCode != 200)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new JsonResult(new ResponseDto { Success = false, Message = "Ok", Errors = new() { status.MsgText } });
            }

            return new JsonResult(new ResponseDto { Success = true, Message = "Ok", Data = txnChargeDetails });
        }
        [HttpPost]
        public async Task<IActionResult> GetConversionRateForNepali([FromBody] GetSendTxnChargeAmountDetailsModel model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;

                var response = new ResponseDto
                {
                    Success = false,
                    Message = "Bad request!",
                    Errors = new() { "Invalid destination currency." },
                };

                return new JsonResult(response);
            }
            var request = _mapper.Map<GetSendTxnChargeAmountDetailsRequest>(model);
            var (status, txnChargeDetails) = await _partnerSendTxnsService.GetSendNepaliToOtherChargeAmountDetailsAsync(request);
            if (txnChargeDetails != null)
            {
                switch (model.PaymentType)
                {
                    case "BANK":
                        if (decimal.Parse(string.IsNullOrEmpty(txnChargeDetails.BankSendTxnLimitNPR) ? "0" : txnChargeDetails.BankSendTxnLimitNPR) < decimal.Parse(string.IsNullOrEmpty(txnChargeDetails.ReceivingAmountNPR) ? "0" : txnChargeDetails.ReceivingAmountNPR))
                        {
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            return new JsonResult(new ResponseDto { Success = false, Message = "Ok", Errors = new() { "Net Recieving Amount exceeded Total Bank Transfer Amount allowed for the Partner!" } });
                        }
                        break;
                    case "WALLET":
                        if (decimal.Parse(string.IsNullOrEmpty(txnChargeDetails.WalletSendTxnLimitNPR) ? "0" : txnChargeDetails.BankSendTxnLimitNPR) < decimal.Parse(string.IsNullOrEmpty(txnChargeDetails.ReceivingAmountNPR) ? "0" : txnChargeDetails.ReceivingAmountNPR))
                        {
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            return new JsonResult(new ResponseDto { Success = false, Message = "Ok", Errors = new() { "Net Recieving Amount exceeded Total Wallet Transfer Amount allowed for the Partner!" } });
                        }
                        break;
                    case "CASH":
                        if (decimal.Parse(string.IsNullOrEmpty(txnChargeDetails.CashPayoutSendTxnLimitNPR) ? "0" : txnChargeDetails.BankSendTxnLimitNPR) < decimal.Parse(string.IsNullOrEmpty(txnChargeDetails.ReceivingAmountNPR) ? "0" : txnChargeDetails.ReceivingAmountNPR))
                        {
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            return new JsonResult(new ResponseDto { Success = false, Message = "Ok", Errors = new() { "Net Recieving Amount exceeded Total Cash Transfer Amount allowed for the Partner!" } });
                        }
                        break;
                    default:
                        break;
                }
            }

            if (status.StatusCode != 200)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new JsonResult(new ResponseDto { Success = false, Message = "Ok", Errors = new() { status.MsgText } });
            }

            return new JsonResult(new ResponseDto { Success = true, Message = "Ok", Data = txnChargeDetails });
        }

        [HttpPost]
        public async Task<IActionResult> GetExistingSenderList([FromBody] GetExistingSenderListModel model)
        {
            if (!_webHelper.IsAjaxRequest(Request))
                return RedirectToAction("Index");

            var partnerCode = User.FindFirstValue("PartnerCode");
            var senderList = await _partnerSenderService
                .GetExistingSendersByPartnercode(partnerCode, MemberId: model.MemberId, FullName: model.FullName);

            // TODO: change later
            senderList = senderList.Take(5);

            return PartialView("_ExistingSenderList", senderList);
        }
        [HttpPost]
        public async Task<IActionResult> GetExistingReciverList([FromBody] GetExistingSenderListModel model)
        {
            if (!_webHelper.IsAjaxRequest(Request))
                return RedirectToAction("Index");

            //var partnerCode = User.FindFirstValue("PartnerCode");
            var ReciverList = await _partnerSenderService
                .GetExistingRecipientsByPartnercode(model.MemberId);

            // TODO: change later
            //ReciverList = ReciverList;

            return PartialView("_ExistingReciverList", ReciverList);
        }
        #region Bulk Tansaction
        [HttpGet]
        public async Task<IActionResult> SendBulkTransaction()
        {
            return await Task.FromResult(PartialView("_AddBulkTransaction"));
        }

        [HttpPost]
        [LogRequest]
        [LogResponse]
        public async Task<IActionResult> SendBulkTransaction(BulkTransactionModel model)
        {
            if (model.uploadFile == null || model.uploadFile.Length == 0)
                return BadRequest("Please select a file.");
            var listResponse = new List<BulkTransactionResponse>();
            try
            {
                if (model.uploadFile.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    using var reader = new StreamReader(model.uploadFile.OpenReadStream());
                    var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = false
                    };
                    using var csv = new CsvReader(reader, csvConfig);
                    var index = 0;
                    foreach (var BTDM in csv.GetRecords<BulkTransactionDetailsModel>())
                    {
                        index++;
                        if (index == 1)
                        {
                            continue;
                        }
                        var bulkTransactionDetailsModel = new BulkTransactionDetailsModel
                        {
                            PaymentType = BTDM.PaymentType,
                            SendingAmount = BTDM.SendingAmount,
                            SourceCurrency = BTDM.SourceCurrency,
                            DestinationCurrency = BTDM.DestinationCurrency,
                            ReceivingAmount = BTDM.ReceivingAmount,
                            SenderName = BTDM.SenderName,
                            SendeContactNumber = BTDM.SendeContactNumber,
                            SenderEmail = BTDM.SenderEmail,
                            SenderCountry = BTDM.SenderCountry,
                            SenderCity = BTDM.SenderCity,
                            SenderAddress = BTDM.SenderAddress,
                            SenderDocType = BTDM.SenderDocType,
                            SenderDocNumber = BTDM.SenderDocNumber,
                            RelationshipWithBeneficiary = BTDM.RelationshipWithBeneficiary,
                            SenderOccupation = BTDM.SenderOccupation,
                            SenderSourceOfIncome = BTDM.SenderSourceOfIncome,
                            PurposeOfRemittance = BTDM.PurposeOfRemittance,
                            BeneficiaryName = BTDM.BeneficiaryName,
                            BeneficiaryContactNumber = BTDM.BeneficiaryContactNumber,
                            BeneficiaryDOB = BTDM.BeneficiaryDOB,
                            BeneficiaryCountry = BTDM.BeneficiaryCountry,
                            BeneficiaryCity = BTDM.BeneficiaryCity,
                            BeneficiaryAddress = BTDM.BeneficiaryAddress,
                            BeneficiaryRelationwithSender = BTDM.BeneficiaryRelationwithSender,
                            BeneficiaryBankCode = BTDM.BeneficiaryBankCode,
                            BeneficiaryBankName = BTDM.BeneficiaryBankName,
                            BeneficiaryBankBranch = BTDM.BeneficiaryBankBranch,
                            BankAccountNo = BTDM.BankAccountNo,
                            WalletCode = BTDM.WalletCode,
                            WalletID = BTDM.WalletID,
                        };
                        var (responsem, validateEachCells) = await _partnerSendTxnsService.ValidateBulkTransactionCells(bulkTransactionDetailsModel);
                        if (responsem.Errors.Count > 0)
                        {
                            var errorMsg = "";

                            if (responsem.Errors.Count > 0)
                            {
                                errorMsg = string.Join(",", responsem.Errors.Select(msg => msg.Message));
                                validateEachCells.Status = "Falied";
                                validateEachCells.Remarks = errorMsg;
                                listResponse.Add(validateEachCells);
                                continue;
                            }
                        }
                        if (bulkTransactionDetailsModel.PaymentType.ToUpper() == "WALLET" || bulkTransactionDetailsModel.PaymentType.ToUpper() == "BANK")
                        {
                            var resp = await _partnerSendTxnsService.ValidateBulkTxnApiAsync(bulkTransactionDetailsModel);
                            if (resp.Success == false)
                            {
                                var errMessage = string.Join(",", resp.Errors.FirstOrDefault());
                                validateEachCells.Status = "Falied";
                                validateEachCells.Remarks = errMessage;
                                listResponse.Add(validateEachCells);
                                continue;
                            }
                            var (response, data) = await _partnerSendTxnsService.PushBulkTransactionServiceAsync(bulkTransactionDetailsModel);
                            if (response.StatusCode != 200)
                            {
                                var errMessage = string.Join(",", responsem.Errors.Select(msg => msg.Message));
                                validateEachCells.Status = "Falied";
                                validateEachCells.Remarks = errMessage;
                                listResponse.Add(validateEachCells);
                                continue;
                            }
                        }
                    }

                    List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<BulkTransactionResponse>(listResponse, 500000);
                    var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "BulkTransaction", true);

                    BulkTxnBaseModel modelRequest = new BulkTxnBaseModel();
                    modelRequest.SendExcelFile = excelFileByteArr;
                    modelRequest.FileName = fileName;

                    _ = _partnerSendTxnsService.SendBulkTransactionEmailAsync(modelRequest);

                    return await Task.FromResult(View("BulkTransactionDetails", listResponse));

                }
                else if (model.uploadFile.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) || model.uploadFile.FileName.EndsWith(".xls", StringComparison.OrdinalIgnoreCase))
                {
                    using (var stream = model.uploadFile.OpenReadStream())
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        var products = new List<BulkTransactionDetailsModel>().ToList();

                        for (int row = 2; row <= rowCount; row++)
                        {

                            var product = new BulkTransactionDetailsModel
                            {
                                PaymentType = worksheet.Cells[row, 1].Value?.ToString(),
                                SourceCurrency = worksheet.Cells[row, 2].Value?.ToString(),
                                DestinationCurrency = worksheet.Cells[row, 3].Value?.ToString(),
                                SendingAmount = worksheet.Cells[row, 4].Value?.ToString(),
                                ReceivingAmount = worksheet.Cells[row, 5].Value?.ToString(),
                                SenderName = worksheet.Cells[row, 6].Value?.ToString(),
                                SendeContactNumber = worksheet.Cells[row, 7].Value?.ToString(),
                                SenderEmail = worksheet.Cells[row, 8].Value?.ToString(),
                                SenderCountry = worksheet.Cells[row, 9].Value?.ToString(),
                                SenderCity = worksheet.Cells[row, 10].Value?.ToString(),
                                SenderAddress = worksheet.Cells[row, 11].Value?.ToString(),
                                SenderDocType = worksheet.Cells[row, 12].Value?.ToString(),
                                SenderDocNumber= worksheet.Cells[row, 13].Value?.ToString(),
                                RelationshipWithBeneficiary = worksheet.Cells[row, 14].Value?.ToString(),
                                SenderOccupation = worksheet.Cells[row, 15].Value?.ToString(),
                                SenderSourceOfIncome = worksheet.Cells[row, 16].Value?.ToString(),
                                PurposeOfRemittance = worksheet.Cells[row, 17].Value?.ToString(),
                                BeneficiaryName = worksheet.Cells[row, 18].Value?.ToString(),
                                BeneficiaryContactNumber = worksheet.Cells[row, 19].Value?.ToString(),
                                BeneficiaryDOB = worksheet.Cells[row, 20].Value?.ToString(),
                                BeneficiaryCountry = worksheet.Cells[row, 21].Value?.ToString(),
                                BeneficiaryCity = worksheet.Cells[row, 22].Value?.ToString(),
                                BeneficiaryAddress = worksheet.Cells[row, 23].Value?.ToString(),
                                BeneficiaryRelationwithSender = worksheet.Cells[row, 24].Value?.ToString(),
                                BeneficiaryBankCode = worksheet.Cells[row, 25].Value?.ToString(),
                                BeneficiaryBankName = worksheet.Cells[row, 26].Value?.ToString(),
                                BeneficiaryBankBranch = worksheet.Cells[row, 27].Value?.ToString(),
                                BankAccountNo = worksheet.Cells[row, 28].Value?.ToString(),
                                WalletCode = worksheet.Cells[row, 29].Value?.ToString(),
                                WalletID = worksheet.Cells[row, 30].Value?.ToString()
                            };

                            var (responseModel, validateCells) = await _partnerSendTxnsService.ValidateBulkTransactionCells(product);
                            if (responseModel.Errors.Count > 0)
                            {
                                var errorMsg = "";

                                if (responseModel.Errors.Count > 0)
                                {
                                    errorMsg = string.Join(",", responseModel.Errors.Select(msg => msg.Message));
                                    validateCells.Status = "Falied";
                                    validateCells.Remarks = errorMsg;
                                    listResponse.Add(validateCells);
                                    continue;
                                }
                            }
                            if (product.PaymentType.ToUpper() == "WALLET" || product.PaymentType.ToUpper() == "BANK")
                            {
                                var resp = await _partnerSendTxnsService.ValidateBulkTxnApiAsync(product);
                                if (resp.Success == false)
                                {
                                    var errMessage = string.Join(",", resp.Errors.FirstOrDefault());
                                    validateCells.Status = "Falied";
                                    validateCells.Remarks = errMessage;
                                    listResponse.Add(validateCells);
                                    continue;
                                }
                                var (response, data) = await _partnerSendTxnsService.PushBulkTransactionServiceAsync(product);
                                if (response.StatusCode != 200)
                                {
                                    var errMessage = string.Join(",", responseModel.Errors.Select(msg => msg.Message));
                                    validateCells.Status = "Falied";
                                    validateCells.Remarks = errMessage;
                                    listResponse.Add(validateCells);
                                    continue;
                                }
                            }
                        }

                        List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<BulkTransactionResponse>(listResponse, 500000);
                        var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "BulkTransaction", true);

                        BulkTxnBaseModel modelRequest = new BulkTxnBaseModel();
                        modelRequest.SendExcelFile = excelFileByteArr;
                        modelRequest.FileName = fileName;
                        _ = _partnerSendTxnsService.SendBulkTransactionEmailAsync(modelRequest);
                        return await Task.FromResult(View("BulkTransactionDetails", listResponse));
                    }
                }
                else
                {
                    return BadRequest("Unsupported file format.");
                }
            }
            catch (Exception ex)
            {

            }
            return await Task.FromResult(View("BulkTransactionDetails", listResponse));
        }

        #endregion
    }
}
