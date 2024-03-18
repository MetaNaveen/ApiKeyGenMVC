namespace ApiKeyGenMVC.Services {

   public interface ISharedDataService {
        public string SecretKey { get; set; }
    }

   public class SharedDataService : ISharedDataService {
      public string SecretKey { get; set; } = "";

      private readonly IConfiguration _configuration;

      public SharedDataService(IConfiguration configuration) {
         _configuration = configuration;
         SecretKey = configuration["AppSettings:SecretKey"] ?? "";
      }
   }
}
