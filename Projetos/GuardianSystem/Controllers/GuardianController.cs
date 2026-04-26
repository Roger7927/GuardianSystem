// (c) 2026 Guillermo Roger Hernandez Chandia - ADS
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GuardianSystem.Controllers;

public class GuardianLog
{
    public int Id { get; set; }
    public string Usuario { get; set; } = "Sistema_Anonimo";
    public string Acao { get; set; } = "Log_Vazio";
    public string Prioridade { get; set; } = "Baixa";
    public DateTime Timestamp { get; set; } = DateTime.Now;
}

public class GuardianContext : DbContext
{
    public GuardianContext(DbContextOptions<GuardianContext> options) : base(options) { }
    public DbSet<GuardianLog> Logs { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class GuardianController : ControllerBase
{
    private readonly GuardianContext _context;
    public GuardianController(GuardianContext context) { _context = context; }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GuardianLog>>> GetLogs([FromQuery] string? prioridade)
    {
        var consulta = _context.Logs.AsQueryable();
        if (!string.IsNullOrEmpty(prioridade))
            consulta = consulta.Where(l => l.Prioridade == prioridade);

        return await consulta.OrderByDescending(l => l.Timestamp).ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<GuardianLog>> PostLog(GuardianLog log)
    {
        // Validação de Segurança: Impede campos em branco
        if (string.IsNullOrWhiteSpace(log.Usuario) || string.IsNullOrWhiteSpace(log.Acao))
        {
            return BadRequest("Erro: Usuário e Ação não podem estar vazios!");
        }

        // Filtro de Regra de Negócio: Só aceita as prioridades que definimos
        var permitidas = new List<string> { "Baixa", "Media", "Critica" };
        if (!permitidas.Contains(log.Prioridade))
        {
            return BadRequest("Erro: Prioridade inválida! Use Baixa, Media ou Critica.");
        }

        _context.Logs.Add(log);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetLogs), new { id = log.Id }, log);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutLog(int id, GuardianLog log)
    {
        if (id != log.Id) return BadRequest();
        _context.Entry(log).State = EntityState.Modified;
        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Logs.Any(e => e.Id == id)) return NotFound();
            else throw;
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLog(int id)
    {
        var log = await _context.Logs.FindAsync(id);
        if (log == null) return NotFound();
        _context.Logs.Remove(log);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
