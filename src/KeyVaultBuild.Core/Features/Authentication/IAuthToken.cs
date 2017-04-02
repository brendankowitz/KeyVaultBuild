namespace KeyVaultBuild.Core.Features.Authentication
{
    public interface IAuthToken
    {
        string GetAuthToken(string resource);
    }
}