using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotOfTheDay.UI.Models;
using SpotOfTheDay.UI.Models.Response;
using SpotOfTheDay.UI.Models.Response.AdVisionService;

namespace SpotOfTheDay.UI.Services
{
    public class AdVisionService
    {
        private readonly IConfiguration _configuration;

        public AdVisionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// This method collects data from "https://datasink-openapi.advision-digital.de/v_spotoftheday"
        /// </summary>
        /// <returns>
        /// If it has no error, it return BaseResponse with Listof AdVisionData
        /// on the other hand if there exist error , this method return BaseResponse with HasError=true and error Message
        /// </returns>
        public async Task<BaseResponse<List<AdVisionData>>> GetDataFromSpotOfTheDay()
        {
            var data = new BaseResponse<List<AdVisionData>>();
            data.Data = new List<AdVisionData>();
            HttpClient client = new HttpClient();

            string adress = _configuration["AdVisionApiSettings:adress"].ToString();
            string token = GetToken();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            try
            {
                var response = await client.GetStringAsync(adress);
                List<AdVisionData> adVisionDataList = JsonConvert.DeserializeObject<List<AdVisionData>>(response);
                data.Data = adVisionDataList;

                return data;
            }
            catch (Exception ex)
            {
                data.HasError = true;
                data.Message = ex.Message;
                return data;
            }

        }

        private string GetToken()
        {
            string address = "https://sts.advision-digital.de/connect/token";
            string clientId = _configuration["AdVisionApiSettings:clientId"].ToString();
            string clientSecret = _configuration["AdVisionApiSettings:clientSecret"].ToString();
            string scope = _configuration["AdVisionApiSettings:scope"].ToString();
            var client = new HttpClient();
            var tokenResponse = client.RequestClientCredentialsTokenAsync(new
                                ClientCredentialsTokenRequest
            {
                Address = address,
                ClientId = clientId,
                ClientSecret = clientSecret,
                Scope = scope
            }).GetAwaiter().GetResult();
            tokenResponse.HttpResponse.EnsureSuccessStatusCode();

            return tokenResponse.AccessToken;
        }
    }

}

