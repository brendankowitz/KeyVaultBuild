using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyVaultBuild.Core.Features.Authentication;
using KeyVaultBuild.Core.Features.Config;
using KeyVaultBuild.Core.Features.Operations;
using KeyVaultBuild.Core.Features.Transformation;

namespace KeyVaultBuild.Core
{
    public class SecretService
    {
        private readonly AuthedClient _client;
        private readonly IDictionary<string, ReadKey> _keyCache = new Dictionary<string, ReadKey>();

        public SecretService(Configuration config)
        {
            _client = new AuthedClient(new InteractiveAuthToken(config));
        }

        public ReadKey ResolveSingleKey(string keySyntax)
        {
            if(KeyTransform.IsKeySyntax(keySyntax) == false)
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