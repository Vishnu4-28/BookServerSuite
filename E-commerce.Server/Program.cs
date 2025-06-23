using System.Text;
using E_commerce.Server.DAL.BASE;
using E_commerce.Server.data;
using E_commerce.Server.Model.Entities;
using E_commerce.Server.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "AllowLocalhost3000";

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", policy =>

//        policy.AllowAnyOrigin()
//              .AllowAnyMethod()
//              .AllowAnyHeader());
//});

var db_host = Environment.GetEnvironmentVariable("DB_HOST");
var db_Name = Environment.GetEnvironmentVariable("DB_NAME");
var db_sa_password = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
var ConnectionString = $"Server={db_host};Database={db_Name};User Id=sa; Password={db_sa_password};TrustServerCertificate=True;";



builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:53394")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                      });
});




// Add services to the container.
builder.Services.AddControllers(option =>
{
    option.RespectBrowserAcceptHeader = true;
    option.ReturnHttpNotAcceptable = true;

}).AddXmlSerializerFormatters();






builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "E-Commerce API", Version = "v1" });
    options.OperationFilter<FileUploadOperation>();

    // Add JWT Bearer support
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your JWT token.\nExample: Bearer eyJhbGciOiJIUzI1NiIs..."
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});








// Add DbContext before building the app
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
})


.AddJwtBearer("Bearer", options =>
{
    var jwtKey = builder.Configuration["Jwt:Key"];
    var jwtIssuer = builder.Configuration["Jwt:Issuer"];
    var jwtAudience = builder.Configuration["Jwt:Audience"];

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.Zero
    };
});


// Add authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy =>
        policy.RequireRole("Admin"));
});





builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuth, Auth>();

builder.Services.AddScoped<IRepository<Books>, Repository<Books>>();
builder.Services.AddScoped<IRepository<User>, Repository<User>>();

builder.Services.AddSignalR();
builder.Services.AddScoped<IRepository<BookImg>, Repository<BookImg>>();


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();


}


app.UseCors("AllowAll");
app.UseStaticFiles();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<NotificationHub>("/NotificationHub");
app.MapFallbackToFile("/index.html");


app.Run();
