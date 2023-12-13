using System;
using ITCron.API.Models.InternetProtocol;

namespace ITCron.API.Interfaces
{
	public interface IIPInformationService
	{
        Task<IPInformation> GetIPInfo(string address);
    }
}

