using Microsoft.EntityFrameworkCore;
using TT.API.Extensions;
using TT.API.Middleware;
using TT.BLL;
using TT.BLL.Services;
using TT.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<TTDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITimeEntryService, TimeEntryService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCustomExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
