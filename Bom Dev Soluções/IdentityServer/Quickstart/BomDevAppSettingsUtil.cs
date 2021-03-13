namespace IdentityServer.Quickstart
{
    public class BomDevAppSettingsUtil
    {
        public BomDevAppSettingsUtil(string bomDevRegisterURL, string bomDevForgotPassword, string homeURL)
        {
            this.registerURL = bomDevRegisterURL;
            this.forgotPasswordURL = bomDevForgotPassword;
            this.homeURL = homeURL;
        }

        public string homeURL { get; set; }
        public string registerURL { get; private set; }
        public string forgotPasswordURL { get; private set; }
    }
}
