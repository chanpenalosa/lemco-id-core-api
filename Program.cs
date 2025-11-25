using lemco_id_core_api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<EmployeeMySqlDbContext>();
builder.Services.AddControllers();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll",
            builder => builder.AllowAnyOrigin() // Replace with your client application's origin
                              .AllowAnyHeader()
                              .AllowAnyMethod());
});

var app = builder.Build();
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
