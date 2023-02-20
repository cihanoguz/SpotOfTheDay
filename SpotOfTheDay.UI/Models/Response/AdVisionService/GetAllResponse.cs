using System;
using System.Collections.Generic;

namespace SpotOfTheDay.UI.Models.Response.AdVisionService
{
	public class GetAllResponse
	{
        public GetAllResponse()
        {
            Items = new List<AdVisionData>();
        }

        public List<AdVisionData> Items { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public int PagingConstant { get; set; }
    }
}

