using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using pergisafar.Database;
using Microsoft.Extensions.Options;
using System.Text;
using RepositoryPattern.Services.AuthService;
using RepositoryPattern.Services.UserService;
using RepositoryPattern.Services.RoleService;
using RepositoryPattern.Services.SettingService;
using RepositoryPattern.Services.PaymentService;
using SendingEmail;
using RepositoryPattern.Services.TransactionsTypeService;
using RepositoryPattern.Services.BannerService;
using RepositoryPattern.Services.TransactionService;
using RepositoryPattern.Services.StatusService;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDatabaseSettings>(db => db.GetRequiredService<IOptions<DatabaseSettings>>().Value);
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ITransactionsTypeService, TransactionsTypeService>();
builder.Services.AddScoped<IStatusService, StatusService>();

builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddScoped<IBannerService, BannerService>();

builder.Services.AddScoped<ConvertJWT>();


builder.Services.AddHttpClient();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}
).AddJwtBearer(options =>
{
    var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    IConfigurationRoot configuration = builder.Build();
    string secretKey = configuration.GetSection("AppSettings")["JwtKey"];

    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ClockSkew = TimeSpan.Zero,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "pergisafar.com",
        ValidAudience = "pergisafar.com",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)) // NOTE: THIS SHOULD BE A SECRET KEY NOT TO BE SHARED; A GUID IS RECOMMENDED
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TravelBerkah", Version = "v1" });

    // Define the "Bearer" security scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer", // The scheme should be "bearer"
        BearerFormat = "JWT"
    });

    // Add the security requirement
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
            new string[] { }
        }
    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.Use(async (context, next) =>
    {
        await next();
     
        if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized) // 401
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(new ErrorDto()
            {
                code = 401,
                errorMessage = new List<ErrorMessageItem>
                {
                    new ErrorMessageItem { error = "UnAuthorized" }
                }
            }.ToString());
        }
    });
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
