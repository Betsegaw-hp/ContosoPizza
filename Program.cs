using Microsoft.EntityFrameworkCore;
using ContosoPizza.Models;
using ContosoPizza.Models.Configurations;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using ContosoPizza.Middlewares;
using Microsoft.AspNetCore.Authorization;
using ContosoPizza.Policies;
using ContosoPizza.Constantes;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<PizzaService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddDbContext<PizzaDbContext>(opts =>
    {
        opts.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
        opts.EnableSensitiveDataLogging();
    }
);
builder.Services.Configure<JwtConfigs>(builder.Configuration.GetSection("JwtConfigs"));

builder.Services.AddSingleton<IAuthorizationHandler, OrderOwnerOrAdminHandler>();
// jwt config

var jwtConfigs = builder.Configuration.GetSection("JwtConfigs").Get<JwtConfigs>();
string secretKey = jwtConfigs?.SecretKey ?? throw new ArgumentNullException("JWT secret key is not Foud!");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfigs?.ValidIssuer,
            ValidAudience = jwtConfigs?.ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(Constants.OrderOwnerOrAdminPolicy.ToString(), policy =>
        policy.Requirements.Add(new OrderOwnerOrAdminRequirement()));
    });

builder.Services.AddHttpContextAccessor();

// Swagger and configure authentication
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CantosoPizza", Version = "v1" });

    // Enable JWT Authentication in Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer <TOKEN>'",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
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
            Array.Empty<string>()
        }
    });
});


builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
    options.HttpsPort = 7294;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseMiddleware<UserContextMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
