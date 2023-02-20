using System;
using Newtonsoft.Json;
using SpotOfTheDay.UI.Models;
using SpotOfTheDay.UI.Models.Response;
using SpotOfTheDay.UI.Models.Response.AdVisionService;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SpotOfTheDay.UI.Services;
using Microsoft.Extensions.Logging;
using SpotOfTheDay.UI.Controllers;
using System.Linq;
using SpotOfTheDay.UI.Business.CacheManager;

namespace SpotOfTheDay.UI.Business
{
	public class AdVisionManager
	{
        private readonly IConfiguration _configuration;
        private readonly ICacheManager _cacheService;

        public AdVisionManager(
            IConfiguration configuration,
            ICacheManager cacheService)
        {
            _configuration = configuration;
            _cacheService = cacheService;
        }

        /// <summary>
        /// This method takes data from Cache Cache takes data from AdVisionService
        /// </summary>
        /// <param name="page">
        /// It represents current page
        /// </param>
        /// <returns>
        /// If it has no error, it return BaseResponse with Listof AdVisionData and pagenation info
        /// on the other hand if there exist error , this method return BaseResponse with HasError=true and error Message
        /// </returns>
        public async Task<BaseResponse<GetAllResponse>> GetDataFromSpotOfTheDay(int page)
        {
            var data = new BaseResponse<GetAllResponse>();
            data.Data = new GetAllResponse();

            try
            {
                var result = await GetDataFromCache();
                List<AdVisionData> adVisionDataList = result.Data;
                if (adVisionDataList.Any())
                {
                    data = PagingSettings(data, adVisionDataList, page);
                }

                return data;
            }
            catch (Exception ex)
            {
                data.HasError = true;
                data.Message = ex.Message;

                return data;
            }

        }

        private BaseResponse<GetAllResponse> PagingSettings(BaseResponse<GetAllResponse> data, List<AdVisionData> adVisionDatas, int page)
        {
            int pageSize = 12;
            data.Data.Items = adVisionDatas
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
            data.Data.PageCount = (int)Math.Ceiling(adVisionDatas.Count / (double)pageSize);
            data.Data.CurrentPage = page;
            data.Data.PagingConstant = page;

            if (data.Data.PageCount - data.Data.PagingConstant < 3)
            {
                data.Data.PagingConstant = data.Data.PageCount - 2;
            }
            if (data.Data.PagingConstant < 3)
            {
                data.Data.PagingConstant = 3;
            }
            if (data.Data.PageCount < 5)
            {
                data.Data.PagingConstant = data.Data.PageCount;
            }

            return data;
        }

        private async Task<BaseResponse<List<AdVisionData>>> GetDataFromCache()
        {
            AdVisionService adVisionService = new AdVisionService(_configuration);

            if (!_cacheService.IsAdd("adVisionData"))
            {
                _cacheService.Add("adVisionData", await adVisionService.GetDataFromSpotOfTheDay(), 10);
            }
          
            BaseResponse<List<AdVisionData>> adVisionList = (BaseResponse<List<AdVisionData>>)_cacheService.Get("adVisionData");
            return adVisionList;

        }
    }
}

