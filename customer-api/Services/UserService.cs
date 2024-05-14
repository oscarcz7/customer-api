using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using customer_api.Models;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;


namespace customer_api.Services
{
    public class UserService: IUserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly string? _jwtSecret;

        public UserService(IMongoDBSettings settings, IConfiguration config)
        {
            var mongoClient = new MongoClient(
                settings.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(settings.DatabaseName);
            _users = mongoDatabase.GetCollection<User>("Users");
            _jwtSecret = config.GetValue<string>("Jwt:Secret");

            if (string.IsNullOrEmpty(_jwtSecret))
            {
                throw new ArgumentNullException(nameof(_jwtSecret), "JWT Secret is not configured.");
            }
        }

        public async Task<User> Register(UserInsertDto userDto)
        {
            var existingUser = await _users.Find(u => u.Username == userDto.Username).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                throw new Exception("User already exists");
            }

            var userId = ObjectId.GenerateNewId().ToString();

            var user = new User
            {
                Id = userId,
                Username = userDto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };
            await _users.InsertOneAsync(user);

            return user;
        }

        public async Task<string> Login(UserInsertDto user)
        {
            var existingUser = await _users.Find(u => u.Username == user.Username).FirstOrDefaultAsync();
            if (existingUser == null || !BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            return GenerateJwtToken(existingUser);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

