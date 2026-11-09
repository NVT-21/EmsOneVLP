using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrderImportWorker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderImportWorker.Services
{
    public class AddressApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSyncOptions _options;

        public AddressApiService(IOptions<ApiSyncOptions> options)
        {
            _options = options.Value;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_options.BaseUrl)
            };
        }

        public async Task<AddressParseResult> ParseAddressAsync(AddressParseRequest req)
        {
            try
            {
                var result = new AddressParseResult();
                var senderTask = Task.FromResult<VMapSearchResponse?>(null);

                var receiverTask = SearchAsync(req.ReceiverStreet);
                if( req.ShopType == "2" )
                {
                    senderTask = SearchAsync(req.SenderStreet);
                }


                await Task.WhenAll(receiverTask, senderTask);

                var receiverRes = await receiverTask;
                var senderRes = await senderTask;

                if( receiverRes == null || receiverRes.Code != "SUCCESS" || receiverRes.Data == null )
                {
                    return new AddressParseResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "Phân tích địa chỉ người nhận thất bại"
                    };
                }

                if( senderRes == null || senderRes.Code != "SUCCESS" || senderRes.Data == null )
                {
                    return new AddressParseResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "Phân tích địa chỉ người gửi thất bại"
                    };
                }

                result.IsSuccess = true;
                result.ReceiverProvinceID = receiverRes.Data.Province_Code ?? "";
                result.ReceiverWardID = receiverRes.Data.Ward_Code ?? "";
                result.SenderProvinceID = senderRes.Data.Province_Code ?? "";
                result.SenderWardID = senderRes.Data.Ward_Code ?? "";

                return result;
            }
            catch( Exception ex )
            {
                return new AddressParseResult
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        private async Task<VMapSearchResponse?> SearchAsync(string fullAddress)
        {
            if( string.IsNullOrWhiteSpace(fullAddress) )
                return null;

            var encodedText = WebUtility.UrlEncode(fullAddress);
            var url = $"api/Map/VMap/SearchV2?text={encodedText}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseText = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<VMapSearchResponse>(responseText);
        }

        private string BuildAddress(string street, string wardName, string provinceName)
        {
            var parts = new List<string>();

            if( !string.IsNullOrWhiteSpace(street) )
                parts.Add(street.Trim());

            if( !string.IsNullOrWhiteSpace(wardName) )
                parts.Add(wardName.Trim());

            if( !string.IsNullOrWhiteSpace(provinceName) )
                parts.Add(provinceName.Trim());

            parts.Add("Vietnam");

            return string.Join(", ", parts);
        }
    }

    public class AddressParseRequest
    {
        public string ReceiverStreet { get; set; }
        public string ReceiverProvinceName { get; set; }
        public string ReceiverWardName { get; set; }

        public string SenderStreet { get; set; }
        public string SenderProvinceName { get; set; }
        public string SenderWardName { get; set; }
        public string ShopType { get; set; } = string.Empty;
    }

    public class AddressParseResult
    {
        public bool IsSuccess { get; set; }
        public string ReceiverProvinceID { get; set; }
        public string ReceiverWardID { get; set; }
        public string SenderProvinceID { get; set; }
        public string SenderWardID { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class VMapSearchResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public VMapSearchData Data { get; set; }
    }

    public class VMapSearchData
    {
        public string Province_Code { get; set; }
        public string ProvincePostalCode { get; set; }
        public string Province_Code_2017 { get; set; }
        public string Province_Name { get; set; }

        public string District_Code { get; set; }
        public string DistrictPostalCode { get; set; }
        public string District_Code_2017 { get; set; }
        public string District_Name { get; set; }

        public string Ward_Code { get; set; }
        public string WardPostalCode { get; set; }
        public string Ward_Code_2017 { get; set; }
        public string Ward_Name { get; set; }

        public string Label { get; set; }
    }
}
