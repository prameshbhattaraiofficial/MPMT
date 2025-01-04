using AutoMapper;
using Dapper;
using Mpmt.Core.Dtos.ConversionRate;
using Mpmt.Core.Dtos.WebCrawler;
using Mpmt.Data.Common;
using System.Data;

namespace Mpmt.Data.Repositories.ExchangeRateRepo;

public class ExchangeRateRepository : IExchangeRateRepository
{
    private readonly IMapper _mapper;

    public ExchangeRateRepository(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<ExchangeRateChangedListPartner>> FedanExchangeRates(List<FedanExchangeRate> rates)
    {
        var dataTable = FedanRateTable();
        if (rates != null)
        {
            foreach (var rate in rates)
            {
                var row = dataTable.NewRow();
                row["Symbol"] = rate.Symbol;
                row["Currency"] = rate.CurrencyCode;
                row["UnitValue"] = rate.Unit;
                row["Rate10"] = rate.Rate10;
                row["Rate14"] = rate.Rate14;
                dataTable.Rows.Add(row);
            }
        }
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();

        param.Add("@ExchangeRateList", dataTable.AsTableValuedParameter("[dbo].[FedanExchangeRateListType]"));

        try
        {
            var result = await connection.QueryAsync("[dbo].[usp_update_fedan_conversion_rate]", param: param, commandType: CommandType.StoredProcedure);
            var data = _mapper.Map<IEnumerable<ExchangeRateChangedListPartner>>(result);
            return data;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task UpdateExchangeRates(List<ExchangeRate> rates)
    {
        var dataTableRates = ExchangeRateTable();
        if (rates != null)
        {
            foreach (var rate in rates)
            {
                var row = dataTableRates.NewRow();
                row["SourceCurrency"] = rate.SourceCurrency;
                row["UnitValue"] = rate.UnitValue;
                row["BuyingRate"] = rate.BuyingRate;
                row["SellingRate"] = rate.SellingRate;
                row["Rate10"] = rate.Rate10;
                row["Rate14"] = rate.Rate14;
                dataTableRates.Rows.Add(row);
            }
        }
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();

        param.Add("@ExchangeRateList", dataTableRates.AsTableValuedParameter("[dbo].[ExchangeRateListType]"));

        try
        {
            _ = await connection.ExecuteAsync("[dbo].[usp_update_conversion_rate]", param: param, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private DataTable ExchangeRateTable()
    {
        var dataTable = new DataTable();
        dataTable.Columns.Add("SourceCurrency", typeof(string));
        dataTable.Columns.Add("UnitValue", typeof(string));
        dataTable.Columns.Add("BuyingRate", typeof(string));
        dataTable.Columns.Add("SellingRate", typeof(string));
        dataTable.Columns.Add("Rate10", typeof(string));
        dataTable.Columns.Add("Rate14", typeof(string));
        return dataTable;
    }

    private DataTable FedanRateTable()
    {
        var dataTable = new DataTable();
        dataTable.Columns.Add("Symbol", typeof(string));
        dataTable.Columns.Add("Currency", typeof(string));
        dataTable.Columns.Add("UnitValue", typeof(string));
        dataTable.Columns.Add("Rate10", typeof(string));
        dataTable.Columns.Add("Rate14", typeof(string));
        return dataTable;
    }
}
