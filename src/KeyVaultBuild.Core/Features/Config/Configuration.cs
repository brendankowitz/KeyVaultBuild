namespace KeyVaultBuild.Features.Config
{
    public class Configuration
    {
        public string Directory { get; set; }
        public string Resource { get; set; }

        public bool Interactive { get; set; } = true;
    }
}