using KeyVaultBuild.Features.Operations;

namespace KeyVaultBuild
{
    public interface ISecretService
    {
        ReadKey ResolveSingleKey(string keySyntax);
        WriteKey GetWriter(string vault);
    }
}