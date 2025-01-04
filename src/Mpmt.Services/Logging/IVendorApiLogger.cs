using Mpmt.Core.Dtos.Logging;

namespace Mpmt.Services.Logging
{
    public interface IVendorApiLogger
    {
        /// <summary>
        /// API request/response log insert in database.
        /// </summary>
        /// <param name="transactionId">Transaction ID</param>
        /// <param name="trackerId">Tracker ID</param>
        /// <param name="requestInput">Request input payload/body.</param>
        /// <param name="responseOutput">Response output body.</param>
        /// <param name="responseHttpStatus">Response HTTP status code.</param>
        /// <param name="requestUrl">Request input URL.</param>
        /// <param name="requestHeaders">Incoming request headers in dictionary json format.</param>
        /// <param name="vendorRequestInput">Vendor API request input or payload</param>
        /// <param name="vendorResponseOutput">Vendor API response body.</param>
        /// <param name="vendorRequestURL">Vendor API request URL</param>
        /// <param name="vendorResponseHttpStatus">Vendor API response HTTP status</param>
        /// <param name="vendorResponseStatus">Whether Vendor API has response</param>
        /// <param name="vendorResponseState">Whether Vendor API response is 'Success' or 'Failure'</param>
        /// <param name="vendorResponseMessage">Vendor API response message</param>
        /// <param name="vendorTransactionId">Vendor API transaction ID</param>
        /// <param name="vendorTrackerId">Vendor API transaction tracker ID</param>
        /// <param name="vendorException">Vendor API Exception message</param>
        /// <param name="vendorExceptionStackTrace">Vendor API Exception stack trace</param>
        /// <param name="vendorId">Vendor API service ID</param>
        /// <param name="vendorType">Vendor API service type</param>
        /// <param name="vendorRequestInput2">Vendor API 2 request input or payload</param>
        /// <param name="vendorResponseOutput2">Vendor API 2 response body.</param>
        /// <param name="vendorRequestURL2">Vendor API 2 request URL</param>
        /// <param name="vendorResponseHttpStatus2">Vendor API 2 response HTTP status</param>
        /// <param name="vendorResponseStatus2">Whether Vendor API 2 has response</param>
        /// <param name="vendorResponseState2">Whether Vendor API 2 response is 'Success' or 'Failure'</param>
        /// <param name="vendorResponseMessage2">Vendor API 2 response message</param>
        /// <param name="vendorTransactionId2">Vendor API 2 transaction ID</param>
        /// <param name="vendorTrackerId2">Vendor API 2 transaction tracker ID</param>
        /// <param name="vendorException2">Vendor API 2 Exception message</param>
        /// <param name="vendorExceptionStackTrace2">Vendor API 2 Exception stack trace</param>
        /// <param name="vendorId2">Vendor API 2 service ID</param>
        /// <param name="vendorType2">Vendor API 2 service type</param>
        /// <param name="vendorRequestInput3">Vendor API 3 request input or payload</param>
        /// <param name="vendorResponseOutput3">Vendor API 3 response body.</param>
        /// <param name="vendorRequestURL3">Vendor API 3 request URL</param>
        /// <param name="vendorResponseHttpStatus3">Vendor API 3 response HTTP status</param>
        /// <param name="vendorResponseStatus3">Whether Vendor API 3 has response</param>
        /// <param name="vendorResponseState3">Whether Vendor API 3 response is 'Success' or 'Failure'</param>
        /// <param name="vendorResponseMessage3">Vendor API 3 response message</param>
        /// <param name="vendorTransactionId3">Vendor API 3 transaction ID</param>
        /// <param name="vendorTrackerId3">Vendor API 3 transaction tracker ID</param>
        /// <param name="vendorException3">Vendor API 3 Exception message</param>
        /// <param name="vendorExceptionStackTrace3">Vendor API 3 Exception stack trace</param>
        /// <param name="vendorId3">Vendor API 3 service ID</param>
        /// <param name="vendorType3">Vendor API 3 service type</param>
        /// <param name="clientCode">Client unique code</param>
        /// <param name="partnerCode">Partner unique code</param>
        /// <param name="agentCode">Agent unique code</param>
        /// <param name="memberId">User/Customer/Client Id</param>
        /// <param name="memberUserName">User/Customer/Client username</param>
        /// <param name="memberName">User/Customer/Client fullname</param>
        /// <param name="deviceCode">Device ID/Code of the request initiater.</param>
        /// <param name="ipAddress">IP address of the request initiater</param>
        /// <param name="platform">Device platform of the request initiater</param>
        /// <param name="machineName">Machine name on which this application is running.</param>
        /// <param name="environment">Current Environment on which application is running.</param>
        /// <returns></returns>
        Task LogInsertAsync(string transactionId = null, string trackerId = null, string requestInput = null, string responseOutput = null, int? responseHttpStatus = null,
            string requestUrl = null, string requestHeaders = null, string vendorRequestInput = null, string vendorResponseOutput = null, string vendorRequestURL = null,
            string vendorRequestHeaders = null, int? vendorResponseHttpStatus = null, bool? vendorResponseStatus = null, string vendorResponseState = null, string vendorResponseMessage = null,
            string vendorTransactionId = null, string vendorTrackerId = null, string vendorException = null, string vendorExceptionStackTrace = null, string vendorId = null, string vendorType = null,
            string vendorRequestInput2 = null, string vendorResponseOutput2 = null, string vendorRequestURL2 = null, string vendorRequestHeaders2 = null, int? vendorResponseHttpStatus2 = null,
            bool? vendorResponseStatus2 = null, string vendorResponseState2 = null, string vendorResponseMessage2 = null, string vendorTransactionId2 = null, string vendorTrackerId2 = null,
            string vendorException2 = null, string vendorExceptionStackTrace2 = null, string vendorId2 = null, string vendorType2 = null,
            string vendorRequestInput3 = null, string vendorResponseOutput3 = null, string vendorRequestURL3 = null, string vendorRequestHeaders3 = null, int? vendorResponseHttpStatus3 = null,
            bool? vendorResponseStatus3 = null, string vendorResponseState3 = null, string vendorResponseMessage3 = null, string vendorTransactionId3 = null, string vendorTrackerId3 = null,
            string vendorException3 = null, string vendorExceptionStackTrace3 = null, string vendorId3 = null, string vendorType3 = null,
            string clientCode = null, string partnerCode = null, string agentCode = null, string memberId = null, string memberUserName = null,
            string memberName = null, string deviceCode = null, string ipAddress = null, string platform = null, string machineName = null, string environment = null);

        /// <summary>
        /// Application response log update in database
        /// </summary>
        /// <param name="transactionId">Transaction ID</param>
        /// <param name="trackerId">Tracker ID</param>
        /// <param name="responseOutput">Response output body.</param>
        /// <param name="responseHttpStatus">Response HTTP status code.</param>
        /// <param name="vendorTransactionId">Vendor API transaction ID</param>
        /// <param name="vendorTrackerId">Vendor API transaction tracker ID</param>
        /// <param name="vendorId">Vendor ID</param>
        /// <param name="vendorType">Vendor type</param>
        /// <param name="vendorTransactionId2">Vendor API 2 transaction ID 2</param>
        /// <param name="vendorTrackerId2">Vendor API 2 transaction tracker ID 2</param>
        /// <param name="vendorId2">Vendor 2 ID</param>
        /// <param name="vendorType2">Vendor 2 type</param>
        /// <param name="vendorTransactionId3">Vendor API 2 transaction ID 3</param>
        /// <param name="vendorTrackerId3">Vendor API 2 transaction tracker ID 3</param>
        /// <param name="vendorId3">Vendor 2 ID</param>
        /// <param name="vendorType3">Vendor 2 type</param>
        /// <returns></returns>
        Task LogUpdateResponseAsync(string transactionId = null, string trackerId = null, string responseOutput = null, int? responseHttpStatus = null,
            string vendorTransactionId = null, string vendorTrackerId = null, string vendorId = null, string vendorType = null,
            string vendorTransactionId2 = null, string vendorTrackerId2 = null, string vendorId2 = null, string vendorType2 = null,
            string vendorTransactionId3 = null, string vendorTrackerId3 = null, string vendorId3 = null, string vendorType3 = null);

        /// <summary>
        /// Vendor API exception update in database
        /// </summary>
        /// <param name="vendorException">Vendor exception message.</param>
        /// <param name="vendorExceptionStackTrace">Vendor exception stack trace.</param>
        /// <returns></returns>
        Task LogVendorApiExceptionAsync(string vendorException, string vendorExceptionStackTrace);

        /// <summary>
        /// Vendor API 2 exception update in database
        /// </summary>
        /// <param name="vendorException2">Vendor API 2 exception message.</param>
        /// <param name="vendorExceptionStackTrace2">Vendor API 2 exception stack trace.</param>
        /// <returns></returns>
        Task LogVendorApiException2Async(string vendorException2, string vendorExceptionStackTrace2);

        /// <summary>
        /// Vendor API 3 exception update in database
        /// </summary>
        /// <param name="vendorException3">Vendor API 3 exception message.</param>
        /// <param name="vendorExceptionStackTrace3">Vendor API 3 exception stack trace.</param>
        /// <returns></returns>
        Task LogVendorApiException3Async(string vendorException3, string vendorExceptionStackTrace3);

        /// <summary>
        /// Vendor API response log update in database.
        /// </summary>
        /// <param name="vendorRequestInput">Vendor API Request body/payload</param>
        /// <param name="vendorResponseOutput">Vendor API response body</param>
        /// <param name="vendorRequestURL">Vendor API Request URL</param>
        /// <param name="vendorRequestHeaders">Vendor API Request headers in dictionary json format</param>
        /// <param name="vendorResponseHttpStatus">Vendor API response HTTP status</param>
        /// <param name="vendorResponseStatus">Whether Vendor API has response</param>
        /// <param name="vendorResponseState">Whether Vendor API response is 'Success' or 'Failure'</param>
        /// <param name="vendorResponseMessage">Vendor API response message</param>
        /// <param name="vendorTransactionId">Vendor API transaction ID</param>
        /// <param name="vendorTrackerId">Vendor API transaction tracker ID</param>
        /// <param name="vendorException">Vendor API Exception message</param>
        /// <param name="vendorExceptionStackTrace">Vendor API Exception stack trace</param>
        /// <param name="vendorId">Vendor ID</param>
        /// <param name="vendorType">Vendor type</param>
        /// <returns></returns>
        Task LogVendorApiResponseAsync(string vendorRequestInput = null, string vendorResponseOutput = null, string vendorRequestURL = null, string vendorRequestHeaders = null,
            int? vendorResponseHttpStatus = null, bool? vendorResponseStatus = null, string vendorResponseState = null, string vendorResponseMessage = null, string vendorTransactionId = null,
            string vendorTrackerId = null, string vendorException = null, string vendorExceptionStackTrace = null, string vendorId = null, string vendorType = null);

        /// <summary>
        /// Vendor API 2 response log update in database.
        /// </summary>
        /// <param name="vendorRequestInput2">Vendor API 2 Request body/payload</param>
        /// <param name="vendorResponseOutput2">Vendor API 2 response body</param>
        /// <param name="vendorRequestURL2">Vendor API 2 Request URL</param>
        /// <param name="vendorRequestHeaders2">Vendor API 2 Request headers in dictionary json format</param>
        /// <param name="vendorResponseHttpStatus2">Vendor API 2 response HTTP status</param>
        /// <param name="vendorResponseStatus2">Whether Vendor API 2 has response</param>
        /// <param name="vendorResponseState2">Whether Vendor API 2 response is 'Success' or 'Failure'</param>
        /// <param name="vendorResponseMessage2">Vendor API 2 response message</param>
        /// <param name="vendorTransactionId2">Vendor API 2 transaction ID</param>
        /// <param name="vendorTrackerId2">Vendor API 2 transaction tracker ID</param>
        /// <param name="vendorException2">Vendor API 2 Exception message</param>
        /// <param name="vendorExceptionStackTrace2">Vendor API 2 Exception stack trace</param>
        /// <param name="vendorId2">Vendor 2 ID</param>
        /// <param name="vendorType2">Vendor 2 type</param>
        /// <returns></returns>
        Task LogVendorApiResponse2Async(string vendorRequestInput2 = null, string vendorResponseOutput2 = null, string vendorRequestURL2 = null, string vendorRequestHeaders2 = null,
            int? vendorResponseHttpStatus2 = null, bool? vendorResponseStatus2 = null, string vendorResponseState2 = null, string vendorResponseMessage2 = null, string vendorTransactionId2 = null,
            string vendorTrackerId2 = null, string vendorException2 = null, string vendorExceptionStackTrace2 = null, string vendorId2 = null, string vendorType2 = null);

        /// <summary>
        /// Vendor API 3 response log update in database.
        /// </summary>
        /// <param name="vendorRequestInput3">Vendor API 3 Request body/payload</param>
        /// <param name="vendorResponseOutput3">Vendor API 3 response body</param>
        /// <param name="vendorRequestURL3">Vendor API 3 Request URL</param>
        /// <param name="vendorRequestHeaders3">Vendor API 3 Request headers in dictionary json format</param>
        /// <param name="vendorResponseHttpStatus3">Vendor API 3 response HTTP status</param>
        /// <param name="vendorResponseStatus3">Whether Vendor API 3 has response</param>
        /// <param name="vendorResponseState3">Whether Vendor API 3 response is 'Success' or 'Failure'</param>
        /// <param name="vendorResponseMessage3">Vendor API 3 response message</param>
        /// <param name="vendorTransactionId3">Vendor API 3 transaction ID</param>
        /// <param name="vendorTrackerId3">Vendor API 3 transaction tracker ID</param>
        /// <param name="vendorException3">Vendor API 3 Exception message</param>
        /// <param name="vendorExceptionStackTrace3">Vendor API 3 Exception stack trace</param>
        /// <param name="vendorId3">Vendor 3 ID</param>
        /// <param name="vendorType3">Vendor 3 type</param>
        /// <returns></returns>
        Task LogVendorApiResponse3Async(string vendorRequestInput3 = null, string vendorResponseOutput3 = null, string vendorRequestURL3 = null, string vendorRequestHeaders3 = null,
            int? vendorResponseHttpStatus3 = null, bool? vendorResponseStatus3 = null, string vendorResponseState3 = null, string vendorResponseMessage3 = null, string vendorTransactionId3 = null,
            string vendorTrackerId3 = null, string vendorException3 = null, string vendorExceptionStackTrace3 = null, string vendorId3 = null, string vendorType3 = null);

        void SetLogContext(string transactionId = null, string trackerId = null, string vendorTransactionId = null, string vendorTrackerId = null,
            string vendorId = null, string vendorType = null, string vendorTransactionId2 = null, string vendorTrackerId2 = null,
            string vendorId2 = null, string vendorType2 = null, string vendorTransactionId3 = null, string vendorTrackerId3 = null,
            string vendorId3 = null, string vendorType3 = null);

        AppVendorApiLog GetLogInstance();
    }
}
