using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Interfaces;
using OrderManager.Persistence.Contexts;
using OrderManager.Persistence.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace OrderManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AuthController(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           ITokenService tokenService,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _dbContext = dbContext;
        }
        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// </summary>
        /// <param name="request">The login request containing username and password.</param>
        /// <returns>A JWT access token if authentication is successful.</returns>
        /// <response code="200">Returns the generated access token.</response>
        /// <response code="401">Returns if the credentials are invalid.</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username) ??
                       await _userManager.FindByEmailAsync(request.Username);

            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid credentials." });
            }
            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                return Unauthorized(new { Message = "Invalid credentials." });
            }
            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || roles.Count == 0)
            {
                return Unauthorized(new { Message = "User has no assigned roles." });
            }
            var token = _tokenService.GenerateToken(user.Id, roles);

            //await _signInManager.SignInAsync(user, false);
            return Ok(new
            {
                AccessToken = token,
                UserId = user.Id,
                Role = roles[0]
            });
        }

        /// <summary>
        /// Logout the current user by blacklisting their token.
        /// </summary>
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // Extract the token from the Authorization header
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { Message = "Invalid token." });
            }

            // Decode the token to get its expiration
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                return BadRequest(new { Message = "Invalid token format." });
            }

            // Check if the token is already expired
            var expirationDate = jwtToken.ValidTo;
            if (expirationDate < DateTime.UtcNow)
            {
                return BadRequest(new { Message = "Token is already expired." });
            }

            // Blacklist the token by saving it in the database
            _dbContext.RevokedTokens.Add(new RevokedToken
            {
                Token = token,
                Expiration = expirationDate,
                RevokedAt = DateTime.UtcNow
            });

            await _dbContext.SaveChangesAsync();

            return Ok(new { Message = "Logged out successfully." });
        }


        /// <summary>
        /// Represents a login request with a username and password.
        /// </summary>
        public class LoginRequest
        {
            /// <summary>
            /// The username or email of the user.
            /// </summary>
            public string Username { get; set; }

            /// <summary>
            /// The password of the user.
            /// </summary>
            public string Password { get; set; }
        }
    }
}