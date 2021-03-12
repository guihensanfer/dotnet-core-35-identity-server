using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Quickstart
{
    public class AppSettingsUtil
    {
        public AppSettingsUtil(string bomDevRegisterURL)
        {
            this.bomDevRegisterURL = bomDevRegisterURL;
        }
        public string bomDevRegisterURL { get; private set; }
    }
}
