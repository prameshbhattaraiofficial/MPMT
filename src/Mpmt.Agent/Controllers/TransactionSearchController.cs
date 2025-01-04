using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Agent.Common;
using Mpmt.Agent.Models.TransactionSearch;
using Mpmt.Core.Common;
using Mpmt.Core.Dtos.Agent;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Services.Services.AgentTxn;
using Mpmt.Services.Services.Common;
using System.Text;

namespace Mpmt.Agent.Controllers
{
    public class TransactionSearchController : AgentBaseController
    {
        public readonly ITransactionService _transactionService;
        private readonly INotyfService _notify;
        private readonly ICommonddlServices _commonddlService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TransactionSearchController(ITransactionService transactionService, INotyfService notify, ICommonddlServices commonddlServices, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _transactionService = transactionService;
            _notify = notify;
            _commonddlService = commonddlServices;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult SearchTransaction()
        {
            return View("SearchTransaction");
        }   

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchTransaction(TransactionDetailsSearchByMTCN model)
        {
            IEnumerable<DocumentTypeddl> documentTypeDdl = await _commonddlService.GetDocumentTypeddl();
            ViewBag.DocumentTypeDdl = new SelectList(documentTypeDdl, "lookup", "Text");

            IEnumerable<Commonddl> relationshipDdl = await _commonddlService.GetRelationShipddl();
            ViewBag.RelationshipDdl = new SelectList(relationshipDdl, "Text", "Text");

            IEnumerable<Commonddl> transferPurposeDdl = await _commonddlService.Gettransferpurposeddl();
            ViewBag.TransferPurposeDdl = new SelectList(transferPurposeDdl, "Text", "Text");

            if (string.IsNullOrWhiteSpace(model.ControlNumber))
            {
                _notify.Error("Control number is required!");
                return RedirectToAction("SearchTransaction", "TransactionSearch");
            }
            var (response, statusDetail) = await _transactionService.checkControlNumberAsynce(model.ControlNumber);

            if (statusDetail.StatusCode != 200)
            {
                _notify.Error(statusDetail.MsgText);
                return RedirectToAction("SearchTransaction", "TransactionSearch");
            }
            else
            {
                decimal b = decimal.Parse(response.PaymentAmountNPR);
                response.PaymentAmountNPR = Math.Round(b, 2).ToString();
                return View("_TransactionSearch", response);
            }
        }
        [HttpPost]
        public async Task<IActionResult> SearchTxn(AgentTxnModel model)
        {
            IEnumerable<DocumentTypeddl> documentTypeDdl = await _commonddlService.GetDocumentTypeddl();
            ViewBag.DocumentTypeDdl = new SelectList(documentTypeDdl, "lookup", "Text");

            IEnumerable<Commonddl> relationshipDdl = await _commonddlService.GetRelationShipddl();
            ViewBag.RelationshipDdl = new SelectList(relationshipDdl, "Text", "Text");

            IEnumerable<Commonddl> transferPurposeDdl = await _commonddlService.Gettransferpurposeddl();
            ViewBag.TransferPurposeDdl = new SelectList(transferPurposeDdl, "Text", "Text");

            var (response, statusDetail) = await _transactionService.checkControlNumberAsynce(model.ControlNumber);
            var (respTransaction, status) = await _transactionService.payoutTransactionByAgentAysnc(model);
            var mappedData = _mapper.Map<AgentPayOutReceiptModel>(respTransaction);
            if (status.StatusCode == 200)
            {
                ViewBag.Words = AmountToWords.ConvertAmount(double.Parse(mappedData.AmountNPR));
                mappedData.TransactionDate = Convert.ToDateTime(respTransaction.TransactionDate).ToString("dd-MMM-yyyy hh:mm:ss tt");
                mappedData.AgentAddress = string.Join(",", new string[] { mappedData.AgentCity, mappedData.AgentDistrict, mappedData.AgentCountry }.Where(c => !string.IsNullOrEmpty(c)));
                mappedData.SenderAddress = string.Join(",", new string[] { mappedData.SenderAddress, mappedData.SenderCountry }.Where(c => !string.IsNullOrEmpty(c)));
                decimal b = decimal.Parse(mappedData.AmountNPR);
                mappedData.AmountNPR = Math.Round(b, 2).ToString();
                _notify.Success(status.MsgText);
                return View("_PayoutReceipt", mappedData);
            }
            else
            {
                _notify.Error(status.MsgText);
                return View("_TransactionSearch", response);
            }
        }
        public Task<string> GetProcessId(string AgentCode, string ReferenceId)
        {
            var processId = _transactionService.GetProcessIdAsync(AgentCode, ReferenceId);
            return processId;
        }
        [HttpGet]
        public IActionResult ViewDetails()
        {
            _notify.Success("Successfully payout transaction!");
            return PartialView();
        }
        public string EncryptControlNumber(string MCTNNo, string publicKey)
        {
            try
            {
                var mtcnBytes = Encoding.UTF8.GetBytes(MCTNNo);

                var publKey = RsaCryptoUtils.ImportPublicKeyPem(publicKey);

                var mtcnPlainBytes = RsaCryptoUtils.EncryptData(mtcnBytes, publKey);

                var mtcnNumber = Convert.ToBase64String(mtcnPlainBytes);

                return mtcnNumber;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public string DecryptControlNumber(string MCTNNo, string privateKeyString)
        {
            try
            {
                var mtcnBytes = Convert.FromBase64String(MCTNNo);

                var pvtKey = RsaCryptoUtils.ImportPrivateKeyPem(privateKeyString);

                var mtcnPlainBytes = RsaCryptoUtils.DecryptData(mtcnBytes, pvtKey);

                var mtcnNumber2 = Encoding.UTF8.GetString(mtcnPlainBytes);

                return mtcnNumber2;
            }
            catch (Exception e)
            {
                // log exception
                return null;
            }
        }
    }
}
