
using System.Net.Http.Headers;
using ITCron.API.DataAccess;
using ITCron.API.Interfaces;
using ITCron.API.Models.InternetProtocol;
using Microsoft.EntityFrameworkCore;

namespace ITCron.API.Services
{
	public class IPInformationService : IIPInformationService
    {
        readonly HttpClient Client;
        readonly string Token;
        ApplicationContext Db; 

        public IPInformationService(HttpClient client, ApplicationContext db, IConfiguration configuration)
		{
            Client = client;
            Db = db;
            Token = configuration["ip-infoToken:DefaultToken"];
        }

        public async Task<IPInformation> GetIPInfo(string address)
		{
            var info = await GetIPInfoFromDB(address);

            if (info is null)
            {
                info = await GetIPInfoFromNet(address);

                await AddIPInformationToDB(info);
            }

            return info;
        }

        private async Task<IPInformation> GetIPInfoFromNet(string address)
        {
            var path = $"{address}?{Token}";

            var response = await Client.GetAsync(path);

            var result = await response.Content.ReadFromJsonAsync<IPInformation>();

            return result;
        }

        private async Task<IPInformation?> GetIPInfoFromDB(string address)
        {
            var result = await Db.IPInformation.FirstOrDefaultAsync(c => c.IP == address);

            return result;
        }

        private async Task<int> AddIPInformationToDB(IPInformation info)
        {
            Db.IPInformation.Add(info);

            var result = await Db.SaveChangesAsync();

            return result;
        }
    }
}

