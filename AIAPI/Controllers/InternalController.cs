using AIAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AIAPI.DTOs;
using AIAPI.Services;
using AIAPI.Interfaces;

namespace AIAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternalController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IConfiguration _configuration;
        private readonly IJwtService _jwtService;
        private readonly InternalService _InternalService;       
        public InternalController(IConfiguration configuration, IJwtService jwtService, InternalService InternalService)
        {

            _config = configuration;
            _jwtService = jwtService;
            _InternalService = InternalService;
        }


        [Authorize]
        [HttpGet]
        [Route("TestGetAIProcess")]
        public async Task<IActionResult> TestGetAIProcess([FromBody] paramDTO model)
        {
            try
            {
                var jwtToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;

                var tenantIdClaim = jsonToken?.Claims.SingleOrDefault(claim => claim.Type == "cTenantID")?.Value;
                if (string.IsNullOrWhiteSpace(tenantIdClaim) || !int.TryParse(tenantIdClaim, out int cTenantID))
                {
                    return EncryptedError(401, "Invalid or missing cTenantID in token.");
                }

                var json = await _InternalService.GetAllProcessmetaAsync(model);
                var data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);

                var response = new APIResponse
                {
                    body = data?.Cast<object>().ToArray() ?? Array.Empty<object>(),
                    statusText = data == null || !data.Any() ? "No data found" : "Successful",
                    status = data == null || !data.Any() ? 204 : 200
                };

                string jsoner = JsonConvert.SerializeObject(response);
                var encrypted = AesEncryption.Encrypt(jsoner);
                return StatusCode(response.status, encrypted);
            }
            catch (Exception ex)
            {
                var apierrDtls = new APIResponse
                {
                    status = 500,
                    statusText = "Internal server Error",
                    error = ex.Message
                };

                string jsoner = JsonConvert.SerializeObject(apierrDtls);
                var encryptapierrDtls = AesEncryption.Encrypt(jsoner);
                return StatusCode(500, encryptapierrDtls);
            }
        }

        private ActionResult EncryptedError(int status, string message)
        {
            var response = new APIResponse { status = status, statusText = message };
            string json = JsonConvert.SerializeObject(response);
            string encrypted = AesEncryption.Encrypt(json);
            return Ok(encrypted);
        }

       





    }
}
