using System;
using ITCron.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ITCron.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class IPInformationController : Controller
	{
		private IIPInformationService IpInfoService { get; set; }

		public IPInformationController(IIPInformationService iPInfoService)
		{
			IpInfoService = iPInfoService;
        }

        [Route("GetInfo")]
        [HttpGet]
        public async Task<IActionResult> GetInfoByIPAddress(string address)
        {
            var result = await IpInfoService.GetIPInfo(address);

            return Ok(result);
        }
    }
}

