using MSLServer.Data;
using MSLServer.Logic;
using MSLServer.Services.EmailService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IServerRepository, ServerRepository>();
builder.Services.AddTransient<IServerThumbnailRepository, ServerThumbnailRepository>();
builder.Services.AddTransient<IServerLogoRepository, ServerLogoRepository>();
builder.Services.AddTransient<ServerListDBContext, ServerListDBContext>();
/* Services */
builder.Services.AddScoped<IEmailService, EmailService>();


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

// Adding background worker services
builder.Services.AddSingleton(new PeriodicTimer(TimeSpan.FromSeconds(60)));
builder.Services.AddHostedService<ServerStatusCheckerBackgroundWorker>();


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

app.UseRouting();
app.UseStaticFiles();
app.UseCors();

app.MapControllers();

app.Run();
