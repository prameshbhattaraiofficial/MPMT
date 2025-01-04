using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using DocumentFormat.OpenXml.Office2019.Drawing.Model3D;
using Mpmt.Core.Dtos.Paging;
using Org.BouncyCastle.Asn1.Tsp;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.InwardRemitanceReport
{
    public class InwardRemitanceDto
    {
        public string SN { get; set; }
        public string CountryCodeAlpha2 { get; set; }
        public string CountryCodeAlpha3 { get; set; }
        public string CountryName { get; set; }
        public string NoOfTransaction { get; set; }
        public string TotalAmountUsd { get; set; }
        public string TotalAmountNpr { get; set; }
    }

    public class InwardRemitanceCompanyWiseDto
    {
        public string SN { get; set; }
        public string PartnerCode { get; set; }
        public string PartnerName { get; set; }
        public string PartnerOrgName { get; set; }
        public string PrincipalOrBranch { get; set; }
        public string CountryCodeAlpha2 { get; set; }
        public string CountryCodeAlpha3 { get; set; }
        public string CountryName { get; set; }
        public string NoOfTransaction { get; set; }
        public string TotalAmountUsd { get; set; }
        public  string TotalAmountNpr { get; set; }
    }
    public class SumofTransaction
    {
        public string GrandTotalCountry { get; set; }
        public string GrandTotalTxn { get; set; }
        public string GrandTotalAmountUSD { get; set; }
        public string GrandTotalAmountNPR { get; set; }
    }
    public class InwardRemitanceFilterDto : PagedRequest
    {
        
        public string CountryCode { get; set; }
        public string DateFlag { get; set; }
        public string StartDate { get; set; }
        public string StartDateBS { get; set; }
        public string EndDate { get; set; }
        public string EndDateBS { get; set; }
        public string Frequency { get; set; }
        public string NprToUsdRate = null; //{ get; set; }
        public int Export {  get; set; }

    }

    public class InwardRemitanceAgentWiseReport
    {

        public string SN { get; set; }
        public string AgentCode { get; set; }
        public string Agent {  get; set; }
        public string Province {  get; set; }
        public string District {  get; set; }
        public string LocalLevel {  get; set; }
        public string Pan {  get; set; }
        public string TotalTxnCount {  get; set; }
        public string TotalAmountNPR { get; set; }
    }


    public class ActionTakenByRemitanceDto
    {
        public string SN { get; set; }
        public string AgentCode { get; set; }
        public string Agent { get; set; }
        public string Province { get; set;   }
        public string District { get; set; }
        public string LocalLevel { get; set; }
        public string RegisteredEngDate {  get; set; }
        public string ActionRequired {  get; set; }
        public string DateOfAction {  get; set; }
        public string ActionReason {  get; set; }
        public string SettlementAmount {  get; set; }
    }
}
