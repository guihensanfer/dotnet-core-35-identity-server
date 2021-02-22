using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Bom_Dev.Shared.Projects
{
    public class Hosts
    {
        public const string HostsJSONFile = "hosts.json";

        public string IdentityServerBaseURL { get; private set; }
        public string BomDevBaseURL { get; private set; }
        public string APIBaseURL { get; private set; }

        /// <summary>
        /// Get config access URLs projects.
        /// </summary>
        public static Task<Hosts> GetHosts
        {
            get
            {                                
                var content = File.ReadAllTextAsync(HostsJSONFile);

                return Task.FromResult(JsonConvert.DeserializeObject<Hosts>(content.Result));
            }
        }
    }
}
