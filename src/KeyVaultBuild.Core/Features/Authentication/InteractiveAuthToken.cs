using System;
using System.Threading;
using KeyVaultBuild.Features.Config;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace KeyVaultBuild.Features.Authentication
{
    public class InteractiveAuthToken : IAuthToken
    {
        private readonly Configuration _config;
        private const string InteractiveAuthClientId = "1950a258-227b-4e31-a9cf-717495945fc2";
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);
        private DateTimeOffset _expiry;
        private string _token;

        //reference so interactive auth dll is copied
        private object reference = typeof(Microsoft.IdentityModel.Clients.ActiveDirectory.Internal.WebBrowserNavigateErrorEventArgs);

        public InteractiveAuthToken(Configuration config)
        {
            _config = config;
        }

        public string GetAuthToken(string resource)
        {
            if (_expiry < DateTime.UtcNow || string.IsNullOrEmpty(_token))
                RequestAuthToken(resource);
            return _token;
        }

        private void RequestAuthToken(string resource)
        {
            SemaphoreSlim.Wait();
            try
            {
                var authContext = new AuthenticationContext($"https://login.windows.net/{_config.Directory}/oauth2/authorize", false);
                var result = authContext.AcquireToken(resource, InteractiveAuthClientId, new Uri("urn:ietf:wg:oauth:2.0:oob"), _config.AlwaysPromptInteractiveAuth ? PromptBehavior.Always : PromptBehavior.Auto);
                _token = result.AccessToken;
                _expiry = result.ExpiresOn;
            }
            catch (Exception ex)
            {
                TokenCache.DefaultShared.Clear();
                Log.Error(ex, "Failed to get auth token");
                throw;
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }
    }
}