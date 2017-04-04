using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using KeyVaultBuild.Features.Authentication;
using KeyVaultBuild.Features.Config;
using KeyVaultBuild.Features.Operations;
using KeyVaultBuild.Features.Transformation;

namespace KeyVaultBuild
{
    public class SecretService : ISecretService
    {
        private readonly AuthedClient _client;
        private readonly IDictionary<string, ReadKey> _keyCache = new ConcurrentDictionary<string, ReadKey>();

        public SecretService(Configuration config)
        {
            IAuthToken tokenProvider;

            if (!string.IsNullOrEmpty(config.ServicePrincipal) && !string.IsNullOrEmpty(config.ServicePrincipalSecret))
                tokenProvider = new ServicePrincipalAuthToken(config);
            else
                tokenProvider = new InteractiveAuthToken(config);

            _client = new AuthedClient(tokenProvider);
        }

        public ReadKey ResolveSingleKey(string keySyntax)
        {
            if (TransformKeys.IsKeySyntax(keySyntax) == false)
                throw new Exception("Invalid key syntax");

            var raw = keySyntax.Trim('#', '{', '}').Split(':').ToArray();

            if (raw.Length != 2 && raw.Length != 3)
                throw new Exception("Invalid number of key parts");

            if (raw.Length == 3)
                raw = raw.Skip(1).ToArray();

            var vault = raw.First();
            var key = raw.Last();

            var cacheKey = $"{vault}:{key}";
            if (_keyCache.ContainsKey(cacheKey) == false)
                _keyCache[cacheKey] = new ReadKey(_client, vault, key);

            return _keyCache[cacheKey];
        }
    }
}