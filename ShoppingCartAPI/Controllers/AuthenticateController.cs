using ShoppingCartAPI.Data;
using ShoppingCartAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MimeKit;
using MailKit.Net.Smtp;
using ShoppingCartAPI.DTO;
// using Newtonsoft.Json.Linq;
using System.Data;
using ShoppingCartAPI.Model;
using ShoppingCartAPI.Repository;

namespace ShoppingCartAPI.Controllers
{
    // Specifies that this class is an API controller and sets the route to "api/authenticate"
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        // Dependency injection for UserManager, RoleManager, and IConfiguration
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        private readonly ICartRepository _cartRepository;


        // Constructor to initialize dependencies
        public AuthenticateController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,ICartRepository cartRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _cartRepository = cartRepository;
        }

        // POST api/authenticate/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            // Find the user by their email address
            var user = await _userManager.FindByNameAsync(request.Email);

            // Check if the user exists and the password is correct
            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {
                // Get the role assigned to the user
                var userRoles = await _userManager.GetRolesAsync(user);

                // Create a list of claims for the JWT token
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email), // Add email claim
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Add unique identifier claim
                    new Claim(ClaimTypes.NameIdentifier, user.Id) // Add user ID claim
                };

                // Add role claims to the list of claims
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                // Generate the JWT token using the claims
                var token = GetToken(authClaims);

                var response = new LoginResponseDto()
                {
                    Email = request.Email,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    UserId = user.Id,

                };

                // Return the token and its expiration time to the client
                return Ok(response);
            }
            // Return Unauthorized status if login failed
            return Unauthorized();
        }


        // POST api/authenticate/register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            // Check if a user with the given email already exists
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            // Create a new user with the provided details
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(), // Generate a unique security stamp
                UserName = model.Email
            };
            // Create the user with the specified password
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            // Ensure that the default user role exists
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            // Assign the default user role to the new user
            if (await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }

            //Sending the mail

            var subject = "Welcome to Our Ecommerce website";
            var body = $"<h1>Welcome, {user.UserName}!</h1><p>Thank you for registering with EShoppingCart.</p>";
            await SendEmailAsync(user.Email, subject, body);


            // Create an empty cart for the new user
            var cart = new Cart
            {
                UserId = user.Id,
                CartItems = new List<CartItem>() // Empty cart items
            };

            await _cartRepository.CreateCart(cart);


            // Return success response
            return Ok(new Response { Status = "Success", Message = "User created successfully! and acknowledgement mail has been sent" });
        }

        private async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_configuration["EmailSettings:Name"], _configuration["EmailSettings:From"]));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                client.Connect(_configuration["EmailSettings:Server"], int.Parse(_configuration["EmailSettings:Port"]), false);
                client.Authenticate(_configuration["EmailSettings:SmtpUser"], _configuration["EmailSettings:SmtpPass"]);
                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }


        // Helper method to generate JWT token
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            // Get the secret key from configuration
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            // Create the token with specified issuer, audience, expiration time, claims, and signing credentials
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(1), // Token expiration time
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            // Return the generated token
            return token;
        }
    }
}
