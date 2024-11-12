using Djini_OOP_Lab8;
using Djini_OOP_Lab8.BLL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer("Data Source=DESKTOP-9L7NQAF;Initial Catalog=DjiniDB;Integrated Security=True;Connect Timeout=30;TrustServerCertificate=True;"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var filePath = Path.Combine(System.AppContext.BaseDirectory, "Djini_OOP_Lab8.xml");
    c.IncludeXmlComments(filePath);
});
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IWorkerService, WorkerService>();
builder.Services.AddScoped<IVacancyService, VacancyService>();
builder.Services.AddScoped<IResumeService, ResumeService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
