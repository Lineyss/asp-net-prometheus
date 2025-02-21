namespace WindowsManager.Abstracts
{
    public class AHttpWorker
    {
        private static readonly HttpClientHandler handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };

        public static readonly HttpClient httpClient = new HttpClient(handler);
        
        public const string BaseUrl = "http://91.214.243.150:800/api/";
    }
}
