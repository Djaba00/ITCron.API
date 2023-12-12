
using System.Net.Http.Headers;
using ITCron.API.DataAccess;
using ITCron.API.Interfaces;
using ITCron.API.Models.InternetProtocol;
using Microsoft.EntityFrameworkCore;

namespace ITCron.API.Services
{
	public class IPInformationService : IIPInformationService
    {
        HttpClient Client = new HttpClient();
        string Token;

        ApplicationContext db; 

        public IPInformationService(ApplicationContext db, IConfiguration configuration)
		{
            this.db = db;
            Token = configuration["ip-infoToken:DefaultToken"];

            Client.BaseAddress = new Uri("http://ipinfo.io/curl");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
            var result = await db.IPInformation.FirstOrDefaultAsync(c => c.IP == address);

            return result;
        }

        private async Task<int> AddIPInformationToDB(IPInformation info)
        {
            db.IPInformation.Add(info);

            var result = await db.SaveChangesAsync();

            return result;
        }
    }
}

