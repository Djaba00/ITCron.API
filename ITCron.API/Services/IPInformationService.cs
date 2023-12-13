
using System.Net.Http.Headers;
using ITCron.API.DataAccess;
using ITCron.API.Interfaces;
using ITCron.API.Models.InternetProtocol;
using Microsoft.EntityFrameworkCore;

namespace ITCron.API.Services
{
	public class IPInformationService : IIPInformationService
    {
        string Token;
        ApplicationContext db; 

        public IPInformationService(ApplicationContext db, IConfiguration configuration)
		{
            this.db = db;
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
            HttpClient сlient = new HttpClient();

            сlient.BaseAddress = new Uri("http://ipinfo.io/curl");
            сlient.DefaultRequestHeaders.Accept.Clear();
            сlient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var path = $"{address}?{Token}";

            var response = await сlient.GetAsync(path);

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

