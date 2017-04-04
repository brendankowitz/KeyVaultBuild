namespace KeyVaultBuild.Features.Authentication
{
    public interface IAuthToken
    {
        string GetAuthToken(string resource);
    }
}