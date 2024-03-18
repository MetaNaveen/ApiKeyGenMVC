using ApiKeyGenMVC.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;

namespace ApiKeyGenMVC.Controllers;

[ApiController]
[Route ("api/[controller]")]
public class ApiKeyController : ControllerBase {
   #region APIKEY
   // /generate?email=a&timestamp=a
   //private const string SecretKey = "YourSecretKeyHere"; // Secret key for HMAC
   //private const double ApiKeyValidityHours = 24; // Validity period of the API key in hours
   private readonly IConfiguration _configuration;
   private readonly ISharedDataService _sharedDataService;
   static string sTimeStampFormat = "yyyy-MM-dd"; // "yyyy-MM-ddTHH:mm:ssZ", "yyyy-MM-dd", "yyyy-MM"

   public ApiKeyController(IConfiguration configuration, ISharedDataService sharedDataService) {
      _configuration = configuration;
      _sharedDataService = sharedDataService;
   }

   [HttpGet]
   [Route ("/validate")]
   public IActionResult Validate ([FromQuery] string email, [FromQuery] string apiKey) {
      string activeApiKey = GenerateApiKey (email, _sharedDataService.SecretKey);
      if (apiKey == activeApiKey) {
         return Ok ("API key is valid.");
      } else {
         return BadRequest ("Invalid API key.");
      }
   }

   [HttpGet]
   [Route ("/generate")]
   public IActionResult Generate ([FromQuery] string email) {
      var apiKey = GenerateApiKey (email, _sharedDataService.SecretKey);
      return Ok (apiKey);
   }

   [HttpGet]
   [Route ("/generateApiKey")]
   public IActionResult GenerateApiKey ([FromHeader] string email) {
      var apiKey = GenerateApiKey (email, _sharedDataService.SecretKey);
      return Ok (apiKey);
   }

   [HttpGet]
   [Route ("/validateApiKey")]
   public IActionResult ValidateApiKey ([FromHeader] string email, [FromHeader] string apiKey) {
      string activeApiKey = GenerateApiKey (email, _sharedDataService.SecretKey);
      if (apiKey == activeApiKey) {
         return Ok ("API key is valid.");
      } else {
         return BadRequest ("Invalid API key.");
      }
   }

   private static string GenerateApiKey (string email, string salt) {
      string data = email + DateTime.UtcNow.ToString (sTimeStampFormat);
      byte[] keyBytes = Encoding.UTF8.GetBytes (salt);
      byte[] dataBytes = Encoding.UTF8.GetBytes (data);
      using var hmac = new HMACSHA256 (keyBytes);
      byte[] hashBytes = hmac.ComputeHash (dataBytes);
      string apiKey = Convert.ToBase64String (hashBytes);
      return apiKey;
   }
   #endregion
}
