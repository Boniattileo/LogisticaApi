using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using LogisticaApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LogisticaApi.Services
{
    public class AuthService
    {
        private readonly FirebaseAuth _firebaseAuth;
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
            string jsonCredentialsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configuration["Firebase:JsonCredentialsPath"]);

            if (FirebaseApp.GetInstance("LogisticaApi") == null)
            {
                var credential = GoogleCredential.FromFile(jsonCredentialsPath);
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = credential,
                    ProjectId = _configuration["Firebase:ProjectId"]
                }, "LogisticaApi");
            }

            _firebaseAuth = FirebaseAuth.GetAuth(FirebaseApp.GetInstance("LogisticaApi"));
        }

        public async Task<string> Register(RegisterModel model)
        {
            var userRecordArgs = new UserRecordArgs
            {
                Email = model.Email,
                Password = model.Password
            };

            var user = await _firebaseAuth.CreateUserAsync(userRecordArgs);
            var claims = new Dictionary<string, object>
            {
                { "role", model.Role }
            };
            await _firebaseAuth.SetCustomUserClaimsAsync(user.Uid, claims);

            return await GenerateJwtToken(user.Uid, model.Email, model.Role.ToString());
        }

        private async Task<string> GenerateJwtToken(string userId, string email, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(1);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }        

        public async Task<string> Login(LoginModel model)
        {
            try
            {
                
                FirebaseToken decodedToken = await _firebaseAuth.VerifyIdTokenAsync(model.IdToken);
                string userId = decodedToken.Uid;
                
                UserRecord user = await _firebaseAuth.GetUserAsync(userId);
                string email = user.Email;
                
                string role = user.CustomClaims.TryGetValue("role", out var roleValue) ? roleValue.ToString() : "User";

                return await GenerateJwtToken(userId, email, role);
            }
            catch (FirebaseAuthException ex)
            {
                throw new Exception("Falha na autenticação: " + ex.Message);
            }
        }
    }
}