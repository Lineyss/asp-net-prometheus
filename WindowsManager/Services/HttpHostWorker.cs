using Newtonsoft.Json;
using System.Net.Http.Json;
using WindowsManager.Models;
using WindowsManager.Abstracts;

namespace WindowsManager.Services
{
    public class HttpHostWorker(ILogger<HttpHostWorker> logger) : AHttpWorker, IHttpHostWorker
    {
        public readonly string CurrentUrl = BaseUrl + "Host";
        private readonly ILogger<HttpHostWorker> logger = logger;

        public async Task<HttpResult<HostModel>> GetByHostnameAsync(string Hostname, CancellationToken token)
        {
            try
            {
                string url = CurrentUrl + $"/Hostname/{Hostname}";
                logger.LogInformation($"{DateTime.Now}: GetByHostnameAsync - отправка запроса");
                var responce = await httpClient.GetAsync(url, token);
                int statusCode = (int)responce.StatusCode;

                if(statusCode == 200)
                {
                    string json = await responce.Content.ReadAsStringAsync();
                    HostModel hostname = JsonConvert.DeserializeObject<HostModel>(json);
                    return new HttpResult<HostModel>(hostname, statusCode);
                }

                return new HttpResult<HostModel>(null, 500);
            }
            catch (Exception e)
            {
                logger.LogError($"{DateTime.Now}: GetByHostnameAsync - {e.Message}");
                return new HttpResult<HostModel>(null, 500);
            }
        }

        public async Task<HttpResult<HostModel>> CreateHostnameAsync(string Hostname, CancellationToken token)
        {
            try
            {
                logger.LogInformation($"{DateTime.Now}: CreateHostnameAsync - отправка запроса");
                var responce = await httpClient.PostAsJsonAsync(CurrentUrl, Hostname, token);
                int status = (int)responce.StatusCode;

                if(status == 200)
                {
                    string json = await responce.Content.ReadAsStringAsync();
                    HostModel host = JsonConvert.DeserializeObject<HostModel>(json);
                    return new HttpResult<HostModel>(host, status);
                }

                return new HttpResult<HostModel>(null, status);
            }
            catch (Exception e)
            {
                logger.LogError($"{DateTime.Now}: CreateHostnameAsync - {e.Message}");
                return new HttpResult<HostModel>(null, 500);
            }
        }

        public async Task<HttpResult<HashSet<HttpSoftware>>> AddSoftwares(string Hostname, HashSet<HttpSoftware> softwares, CancellationToken token)
        {
            try
            {
                string CurrentUrl = this.CurrentUrl + $"/{Hostname}/software";
                logger.LogInformation($"{DateTime.Now}: AddSoftwares - отправка запроса");
                var responce = await httpClient.PostAsJsonAsync(CurrentUrl, softwares, token);
                int statusCode = (int)responce.StatusCode;

                if (statusCode != 200)
                {
                    string jsonResponce = await responce.Content.ReadAsStringAsync();
                    logger.LogInformation($"{DateTime.Now}: AddSoftwares - ответ {jsonResponce}");
                    HashSet<HttpSoftware> deleteSoftwares = JsonConvert.DeserializeObject<HashSet<HttpSoftware>>(jsonResponce) ?? [];
                    return new HttpResult<HashSet<HttpSoftware>>(null, statusCode);
                }

                return new HttpResult<HashSet<HttpSoftware>>(null, statusCode);
            }
            catch (Exception e)
            {
                logger.LogError($"{DateTime.Now}: AddSoftwares - {e.Message}");
                return new HttpResult<HashSet<HttpSoftware>>(500);
            }
        }
        public async Task<HttpResult<HashSet<HttpSoftware>>> CheckSoftware(string Hostname, HashSet<HttpSoftware> softwares, CancellationToken token)
        {
            try
            {
                string CurrentUrl = this.CurrentUrl + $"/{Hostname}/software";

                logger.LogInformation($"{DateTime.Now}: CheckSoftware - отправка запроса");
                var responce = await httpClient.PutAsJsonAsync(CurrentUrl, softwares, token);
                logger.LogInformation($"{DateTime.Now}: CheckSoftware - ответ {(int)responce.StatusCode} статус");

                int statusCode = (int)responce.StatusCode;

                string str = await responce.Content.ReadAsStringAsync();
                logger.LogInformation($"{DateTime.Now}: CheckSoftware - Данные в ответе {str}");

                if (statusCode == 200)
                {
                    HashSet<HttpSoftware> _software = JsonConvert.DeserializeObject<HashSet<HttpSoftware>>(str) ?? [];

                    return new HttpResult<HashSet<HttpSoftware>>(_software, statusCode);
                }

                return new HttpResult<HashSet<HttpSoftware>>(null, statusCode); ;
            }
            catch (Exception e)
            {
                logger.LogError($"{DateTime.Now}: CheckSoftware - {e.Message}");
                return new HttpResult<HashSet<HttpSoftware>>(null, 500);
            }
        }
    }
}
