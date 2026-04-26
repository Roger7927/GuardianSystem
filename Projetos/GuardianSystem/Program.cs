// (c) 2026 Guillermo Roger Hernandez Chandia - ADS
using Microsoft.EntityFrameworkCore;
using GuardianSystem.Controllers;

var builder = WebApplication.CreateBuilder(args);

// 1. SERVICES (Configura as ferramentas - As Bolinhas)
builder.Services.AddDbContext<GuardianContext>(opt =>
    opt.UseSqlite("Data Source=GuardianData.db"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. MIDDLEWARE (Configura a Vitrine - Swagger)
// Importante: Tem de estar DEPOIS do builder.Build()
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 3. DATABASE (Inicialização do Banco - A Caixa)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GuardianContext>();
    db.Database.EnsureCreated();
}

// 4. EXECUÇÃO (Dá partida no motor)
app.MapControllers();
app.Run();
