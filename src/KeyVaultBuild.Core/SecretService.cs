using System;
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
        private readonly IDictionary<string, ReadKey> _keyCache = new Dictionary<string, ReadKey>();

        public SecretService(Configuration config)
        {
            _client = new AuthedClient(new InteractiveAuthToken(config));
        }

        public ReadKey ResolveSingleKey(string keySyntax)
        {
            if(TransformKey.IsKeySyntax(keySyntax) == false)
                throw new Exception("Invalid key syntax");

            var raw = keySyntax.Trim('#', '{', '}').Split(':').Skip(1).ToArray();
            var vault = raw.First();
            var key = raw.Last();

            var cacheKey = $"{vault}:{key}";
            if (_keyCache.ContainsKey(cacheKey) == false)
                _keyCache[cacheKey] = new ReadKey(_client, vault, key);

            return _keyCache[cacheKey];
        }
    }
}