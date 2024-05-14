using System.Text;
using customer_api.Models;
using customer_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(
//        policy =>
//        {
//            policy.WithOrigins("http://localhost:8100").AllowAnyHeader()
//                                                  .AllowAnyMethod(); ;
//        });
//});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Configurar la autenticación JWT en Swagger
    var jwtSecret = builder.Configuration.GetValue<string>("Jwt:Secret");
    var key = Encoding.ASCII.GetBytes(jwtSecret);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});


// Add services to the container.
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection(nameof(MongoDBSettings)));

builder.Services.AddSingleton<IMongoDBSettings>(sp =>
    sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);


// Register services
builder.Services.AddSingleton<CustomerService>();
//builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<UserService>();



// Configure JWT authentication
var jwtSecret = builder.Configuration.GetValue<string>("Jwt:Secret");
if (string.IsNullOrEmpty(jwtSecret))
{
    throw new ArgumentNullException("JWT secret key is not configured");
}

var key = Encoding.ASCII.GetBytes(jwtSecret);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

//builder.Services.AddAuthorization();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        // Habilitar la autenticación en Swagger UI
        c.OAuthClientId("swagger");
        c.OAuthAppName("Swagger UI");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();