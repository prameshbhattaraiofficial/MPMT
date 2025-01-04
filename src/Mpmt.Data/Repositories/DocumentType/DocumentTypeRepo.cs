using Dapper;
using Mpmt.Core.Dtos.DocumentType;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.DocumentType
{
    /// <summary>
    /// The document type repo.
    /// </summary>
    public class DocumentTypeRepo : IDocumentTypeRepo
    {
        /// <summary>
        /// Adds the document type async.
        /// </summary>
        /// <param name="addDocumentType">The add document type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddDocumentTypeAsync(IUDDocumentType addDocumentType)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@Event", "I");
                param.Add("Id", addDocumentType.Id);
                param.Add("@DocumentType", addDocumentType.DocumentType);
                param.Add("@DocumentTypeCode", addDocumentType.DocumentTypeCode);
                param.Add("@IsExpirable", addDocumentType.IsExpirable);
                param.Add("@IsActive", addDocumentType.IsActive);
                param.Add("@Remarks", addDocumentType.Remarks);
                param.Add("@LoggedInUser", 1);
                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_DocumentType]", param, commandType: CommandType.StoredProcedure);

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

        /// <summary>
        /// Gets the document type async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<DocumentTypeDetails>> GetDocumentTypeAsync()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            return await connection.QueryAsync<DocumentTypeDetails>("[dbo].[usp_get_DocumentType]", commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the document type by id async.
        /// </summary>
        /// <param name="documentTypeId">The document type id.</param>
        /// <returns>A Task.</returns>
        public async Task<DocumentTypeDetails> GetDocumentTypeByIdAsync(int documentTypeId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Id", documentTypeId);
            return await connection.QueryFirstOrDefaultAsync<DocumentTypeDetails>("[dbo].[usp_get_DocumentType_ById]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Removes the document type async.
        /// </summary>
        /// <param name="removeDocumentType">The remove document type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveDocumentTypeAsync(IUDDocumentType removeDocumentType)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();

            param.Add("@Event", "D");
            param.Add("Id", removeDocumentType.Id);
            param.Add("@DocumentType", removeDocumentType.DocumentType);
            param.Add("@DocumentTypeCode", removeDocumentType.DocumentTypeCode);
            param.Add("@IsExpirable", removeDocumentType.IsExpirable);
            param.Add("@IsActive", removeDocumentType.IsActive);
            param.Add("@Remarks", removeDocumentType.Remarks);
            param.Add("@LoggedInUser", 1);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_DocumentType]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        /// <summary>
        /// Updates the document type async.
        /// </summary>
        /// <param name="updateDocumentType">The update document type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateDocumentTypeAsync(IUDDocumentType updateDocumentType)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();

            param.Add("@Event", "U");
            param.Add("Id", updateDocumentType.Id);
            param.Add("@DocumentType", updateDocumentType.DocumentType);
            param.Add("@DocumentTypeCode", updateDocumentType.DocumentTypeCode);
            param.Add("@IsExpirable", updateDocumentType.IsExpirable);
            param.Add("@IsActive", updateDocumentType.IsActive);
            param.Add("@Remarks", updateDocumentType.Remarks);
            param.Add("@LoggedInUser", 1);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_DocumentType]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
    }
}
