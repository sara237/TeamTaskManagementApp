using Microsoft.EntityFrameworkCore;
using TeamTaskManagement.API.Hubs;
using TeamTaskManagement.Application.Interfaces;
using TeamTaskManagement.Application.Services;
using TeamTaskManagement.Infrastructure.Persistence;
using TeamTaskManagement.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
//builder.Services.AddDbContext<TeamTaskManagementDbContext>(options =>
//    options.UseInMemoryDatabase("TeamTaskManagementDB"));
//builder.Services.AddDbContext<TeamTaskManagementDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<TeamTaskManagementDbContext>(options =>
        options.UseInMemoryDatabase("TeamTaskManagementDB"));
}
else
{
    builder.Services.AddDbContext<TeamTaskManagementDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}
// Register Repositories
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IUserRepository, UserService>();

// Register Services
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<ChatService>();
var devCorsPolicy = "devCorsPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(devCorsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
// Add SignalR for real-time chat
builder.Services.AddSignalR();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseRouting();
app.UseAuthorization();
app.UseCors(devCorsPolicy);
app.UseHttpsRedirection();
app.MapControllers();

// Map SignalR Hub (to create later)
app.MapHub<ChatHub>("/chatHub");
app.Run();
