using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using To_Do_List;
using To_Do_List.Models;

const string corsPolicy = "AllowAll";

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddDbContext<TaskContext>(e => e.UseSqlServer(builder.Configuration.GetConnectionString("SQL-Server")));
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddAutoMapper(typeof(MapperConfig));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "Todo List",
        Version = "v1",
        Description = "RESTFul Api For Todo List (Taks)",
        TermsOfService = new Uri("https://github.com/Y-Baker"),
        Contact = new OpenApiContact()
        {
            Email = "yuossefbakier@gmail.com",
            Name = "Yousef Bakier"
        }
    });

    option.EnableAnnotations();
});

builder.Services.AddCors(e => e.AddPolicy(corsPolicy, p =>
{
    p.AllowAnyOrigin();
    p.AllowAnyMethod();
    p.AllowAnyHeader();
}));

WebApplication? app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsPolicy);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
