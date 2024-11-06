
namespace Infrastructure
{
    public class AppSettingsWeb : IAppSettingsWeb
    {
        private readonly IConfiguration _configuration;

        public AppSettingsWeb(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string ConnectionString
        {
            get
            {
                return _configuration.GetConnectionString("DefaultConnection");

            }
        }
    }
}
