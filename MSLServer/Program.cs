using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using MSLServer.Data;
using MSLServer.Logic;
using MSLServer.Middlewares;
using MSLServer.Models;
using MSLServer.Models.Policy;
using MSLServer.Services.EmailService;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IServerRepository, ServerRepository>();
builder.Services.AddTransient<IServerThumbnailRepository, ServerThumbnailRepository>();
builder.Services.AddTransient<IServerLogoRepository, ServerLogoRepository>();
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();
builder.Services.AddTransient<ServerListDBContext, ServerListDBContext>();
/* Services */
builder.Services.AddScoped<IEmailService, EmailService>();

// Adding background worker services
builder.Services.AddSingleton(new PeriodicTimer(TimeSpan.FromSeconds(60)));
builder.Services.AddHostedService<ServerStatusCheckerBackgroundWorker>();


builder.Services.AddSignalR();
builder.Services.AddCors(options => options.AddDefaultPolicy(
    builder =>
    {

        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    }
));


builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Resource.Criptkey)),
        ValidateIssuer = false,
        ValidateAudience = false

    };

});

builder.Services.AddAuthorization(configure =>
{
    configure.AddPolicy("DeletePolicy", policyBuilder =>
    {
        policyBuilder.AddRequirements(new IdRequirement(""));
    });
});




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseRouting();
app.UseStaticFiles();
app.UseCors();

app.MapControllers();

app.Run();
