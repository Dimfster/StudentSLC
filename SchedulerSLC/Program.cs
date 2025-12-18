using Microsoft.EntityFrameworkCore;
using StudentSLC.Data;
using StudentSLC.Security;
using StudentSLC.Services;
using StudentSLC.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("StudentSLCContext")));

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

// Регистрация пользователей    
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<GroupService>();
builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<ScheduleService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtProvider>();
builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddScoped<CodeGenerator>();

builder.Services.AddApiAuthentication(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();


var app = builder.Build();

// --- Секция инициализации данных --- 
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<PasswordHasher>();
    await SeedData.Initialize(db, passwordHasher);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}

app.UseCors(policy =>
{
    policy
        .WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod();
});


app.UseHttpsRedirection();

app.UseAuthentication(); 

app.UseAuthorization();

app.MapControllers();

app.Run();
