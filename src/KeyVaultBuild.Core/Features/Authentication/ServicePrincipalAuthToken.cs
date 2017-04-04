using System;
using System.Threading;
using KeyVaultBuild.Features.Config;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace KeyVaultBuild.Features.Authentication
{
    public class ServicePrincipalAuthToken : IAuthToken
    {
        private readonly Configuration _config;
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);
        private DateTimeOffset _expiry;
        private string _token;
        
        public ServicePrincipalAuthToken(Configuration config)
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
                var result = authContext.AcquireToken(resource, new ClientCredential(_config.ServicePrincipal, _config.ServicePrincipalSecret));
                _token = result.AccessToken;
                _expiry = result.ExpiresOn;
            }
            catch (Exception ex)
            {
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