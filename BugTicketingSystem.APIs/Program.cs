using BugTicketingSystem.APIs.HandleFiles;
using BugTicketingSystem.APIs.Middleware;
using BugTicketingSystem.BL;
using BugTicketingSystem.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddBusinessServices();
builder.Services.AddDataAccessServices(builder.Configuration);


builder.Services.AddExceptionHandler<ExceptionHandlingMiddleware>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<IFileService, FileService>();




#region Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secretKey = builder.Configuration.GetValue<string>("JWT:SecretKey");

        var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
        var key = new SymmetricSecurityKey(secretKeyBytes);

        options.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            //ValidateLifetime = true,
            //ValidateIssuerSigningKey = true,
            IssuerSigningKey = key
        };

    });
#endregion


var app = builder.Build();





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();



#region HandleFiles

// handle image upload
//if folder doesn't exist create it
var imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Upload");
Directory.CreateDirectory(imageFolderPath);


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imageFolderPath),
    RequestPath = "/api/static-files"
});
#endregion

app.UseExceptionHandler();

app.MapControllers();

app.Run();
