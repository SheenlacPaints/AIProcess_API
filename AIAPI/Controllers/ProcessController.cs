using AIAPI.DTOs;
using AIAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AIAPI.Interfaces; 
using AIAPI.Services;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AIAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IConfiguration _configuration; 
        private readonly IJwtService _jwtService;


        private readonly IInternalService _InternalService;


        public ProcessController(IConfiguration configuration, IJwtService jwtService, IInternalService InternalService)
        {
            _config = configuration;
            _jwtService = jwtService;
            _InternalService = InternalService;
        }

        [Authorize]
        [HttpPost]
        [AllowAnonymous]
        [Route("GetAIProcess_sample")]
        public async Task<IActionResult> GetAIProcess_sample([FromBody] paramDTO model)
        {
            try
            {                  
                var jwtToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrWhiteSpace(jwtToken))
                {
                    return StatusCode(401, "Authorization token is missing");
                }
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;             
                var usernameClaim = jsonToken?.Claims.SingleOrDefault(claim => claim.Type == "username")?.Value;
                string username = usernameClaim;

                if ( string.IsNullOrWhiteSpace(usernameClaim))
                {
                    var error = new APIResponse
                    {
                        status = 401,
                        statusText = "Invalid or missing cTenantID in token."
                    };
                    string errorJson = JsonConvert.SerializeObject(error);                
                    return StatusCode(401, errorJson);
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

                return StatusCode(response.status, jsoner);
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

                return StatusCode(500, jsoner);
            }
        }

           
        [HttpPost]
        [Route("createtoken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> createtoken([FromBody] UserDTO User)
        {
            
        

            APIResponse Objresponse = new APIResponse();

            if (User == null || string.IsNullOrEmpty(User.userName) || string.IsNullOrEmpty(User.password))
                return BadRequest("Username and password must be provided.");

            var connStr = _config.GetConnectionString("Database");
            string status = string.Empty;
            string email = "", phoneno = "", UserID = "",username = "";
            Console.WriteLine("DB Connection String: " + connStr);

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("sp_validate_Admin_login_new", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FilterValue1", User.userName);
                        cmd.Parameters.AddWithValue("@FilterValue2", User.password);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                status = reader["cstatus"]?.ToString() ?? "";
                                if (status == "valid user")
                                {
                                    username = reader["username"]?.ToString();                                  
                                    email = reader["email"]?.ToString();
                                    phoneno = reader["phoneno"]?.ToString();
                                }
                            }
                        }
                    }
                }

                if (status == "invalid password")
                {
                    Objresponse.statusText = "Incorrect Password";
                    Objresponse.status = 400;
                    return BadRequest(Objresponse);
                }

                if (status == "username not exist")
                {
                    Objresponse.statusText = "User does not exist.";
                    Objresponse.status = 404;
                    return NotFound(Objresponse);
                }

                if (status == "valid user")
                {
                    var accessToken = _jwtService.GenerateJwtToken(User.userName,out var tokenExpiry);

                    var loginDetails = new
                    {
                        username = username,
                        phoneno= phoneno,
                        email = email,
                        token = accessToken,                     
                    };
                    Objresponse.body = new object[] { loginDetails };
                    Objresponse.statusText = "Logged in Successfully";
                    Objresponse.status = 200;
                    var apiDtls = new APIResponse
                    {
                        status = 200,
                        statusText = "Logged in Successfully",
                        body = new[] { loginDetails }
                    };
                    string json = JsonConvert.SerializeObject(apiDtls);                 
                    return StatusCode(200, json);
                }

                var apierDtls = new APIResponse
                {
                    status = 500,
                    statusText = "Unexpected login status",
                };
                string jsone = JsonConvert.SerializeObject(apierDtls);             
                return StatusCode(500, jsone);

            }
            catch (Exception ex)
            {

                var apierrDtls = new APIResponse
                {
                    status = 500,
                    statusText = "An error occurred during login: " + ex.Message,
                };

                string jsoner = JsonConvert.SerializeObject(apierrDtls);
               
                return StatusCode(500, jsoner);
            }
        }



        [Authorize]
        [HttpPost]
        [Route("FetchCustomerProcessData")]
        public async Task<IActionResult> GetAIProcess([FromBody] LoginDTO model)
        {
            try
            {
                var username = User.FindFirst("username")?.Value;

                if (model == null || string.IsNullOrEmpty(model.EmpId) || string.IsNullOrEmpty(model.PhoneNumber))
                {
                    return StatusCode(400,new APIResponse
                    {
                        status = 400,
                        statusText = "Employee ID and Phone Number are required and cannot be null."
                    });
                }

                if (string.IsNullOrWhiteSpace(username))
                {
                    return StatusCode(401, new APIResponse
                    {
                        status = 401,
                        statusText = "Username claim missing in token"
                    });
                }

                var purchaseHistoryjson = await _InternalService.GetAllPurchasehistorymetaAsync(model);
                var schemeMasterjson = await _InternalService.GetAllSchemeProcessmetaAsync(model);
                var FrequentPurchasedMasterjson = await _InternalService.GetAllPurchaseProcessmetaAsync(model);

                return Ok(new
                {
                    PurchaseHistoryMaster = purchaseHistoryjson,
                    SchemeMaster = schemeMasterjson,
                    FrequentPurchasedMaster = FrequentPurchasedMasterjson
                });
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }




        [Authorize]
        [HttpPost]
        [Route("FetchCustomerDetails")]
        public async Task<IActionResult> FetchCustomerDetails()
        {
            try
            {
                var username = User.FindFirst("username")?.Value;

                if (string.IsNullOrWhiteSpace(username))
                {
                    return StatusCode(401, new APIResponse
                    {
                        status = 401,
                        statusText = "Username claim missing in token"
                    });
                }

                var result = await _InternalService.FetchCustomerDetailAsync(username);
                return Ok(result);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}