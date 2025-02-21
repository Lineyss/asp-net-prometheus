using WindowsManager.Models;
using WindowsManager.Abstracts;
using WindowsManager.Services;

namespace WindowsManager.Contexts
{
    public class SoftwareWorkerContext(IEnumerable<ISoftwareFinder> softwareFinders, IEnumerable<ISoftwareDelete> softwareDeletes)
    {
        private readonly IEnumerable<ISoftwareFinder> softwareFinders = softwareFinders;
        private readonly IEnumerable<ISoftwareDelete> softwareDeletes = softwareDeletes.OrderBy(deleteModel => deleteModel is DeleteSoftware)
                                                                                       .ThenBy(deleteModel => deleteModel is DeleteSoftwareFolder);

        public async Task<HashSet<HttpSoftware>> FindSoftware(CancellationToken token)
        {
            var tasks = softwareFinders.Select(finder => finder.Find(token));

            var result = await Task.WhenAll(tasks);

            var array = new HashSet<RegistryModel>(result.SelectMany(list => list));

            var httpSoftwares = array.Select(element => new HttpSoftware(element.DisplayName, element.DisplayVersion, element.Publisher)).ToList();

            return new HashSet<HttpSoftware>(httpSoftwares);
        }

        public async Task DeleteSoftware(HttpSoftware software)
        {
            foreach(ISoftwareDelete delete in softwareDeletes)
            {
                await delete.Delete(software);
            }
        }
    }
}
