using Microsoft.AspNetCore.SignalR;
using MSLServer.Data;
using MSLServer.Logic;
using MSLServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IServerRepository, ServerRepository>();
builder.Services.AddTransient<IServerThumbnailRepository, ServerThumbnailRepository>(); 
builder.Services.AddTransient<ServerListDBContext, ServerListDBContext>();

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
app.UseCors();
app.UseRouting();
app.UseStaticFiles();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<SignalRHub>("/hub");
});

app.MapControllers();

app.Run();