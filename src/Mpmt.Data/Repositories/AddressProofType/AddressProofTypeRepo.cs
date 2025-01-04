using Dapper;
using Mpmt.Core.Dtos.AddressProofType;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.AddressProofType;

public class AddressProofTypeRepo : IAddressProofTypeRepo
{
    public async Task<AddressProofTypeDetails> GetAddressProofTypeByIdAsync(int id)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@Id", id);
        return await connection.QueryFirstOrDefaultAsync<AddressProofTypeDetails>("[dbo].[usp_get_addressproof_type_byid]", param, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<AddressProofTypeDetails>> GetAddressProofTypesAsync()
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        return await connection.QueryAsync<AddressProofTypeDetails>("[dbo].[usp_get_addressproof_type]", commandType: CommandType.StoredProcedure);
    }

    public async Task<SprocMessage> IUDAddressProofTypeAsync(IUDAddressProofType addressProofType)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Event", addressProofType.Event);
            param.Add("@Id", addressProofType.Id);
            param.Add("@AddressProofName", addressProofType.AddressProofName);
            param.Add("@AddressProofCode", addressProofType.AddressProofCode);
            param.Add("@Description", addressProofType.Description);
            param.Add("@IsActive", addressProofType.IsActive);
            param.Add("@LoggedInUser", addressProofType.LoggedInUser);
            param.Add("@UserType", addressProofType.UserType);

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_addressproof_type]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
        catch (Exception)
        {
            throw;
        }
    }
}
