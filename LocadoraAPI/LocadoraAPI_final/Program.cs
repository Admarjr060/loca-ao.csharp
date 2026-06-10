using Microsoft.EntityFrameworkCore;
using LocadoraAPI.Data;
using LocadoraAPI.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=locadora.db"));

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}


app.MapGet("/carros", async (AppDbContext db) =>
    Results.Ok(await db.Carros.ToListAsync()));


app.MapGet("/carros/{id}", async (int id, AppDbContext db) =>
{
    var carro = await db.Carros.FindAsync(id);
    return carro is not null ? Results.Ok(carro) : Results.NotFound("Carro não encontrado.");
});

app.MapPost("/carros", async (Carro carro, AppDbContext db) =>
{
    
    if (string.IsNullOrWhiteSpace(carro.Modelo) || string.IsNullOrWhiteSpace(carro.Placa))
        return Results.BadRequest("Modelo e Placa são obrigatórios.");

    if (carro.ValorDiaria <= 0)
        return Results.BadRequest("Valor da diária deve ser maior que zero.");

    bool placaExiste = await db.Carros.AnyAsync(c => c.Placa == carro.Placa);
    if (placaExiste)
        return Results.BadRequest("Já existe um carro com essa placa.");

    carro.Disponivel = true;
    db.Carros.Add(carro);
    await db.SaveChangesAsync();

    return Results.Created($"/carros/{carro.Id}", carro);
});


app.MapPut("/carros/{id}", async (int id, Carro carroAtualizado, AppDbContext db) =>
{
    var carro = await db.Carros.FindAsync(id);
    if (carro is null) return Results.NotFound("Carro não encontrado.");

    if (string.IsNullOrWhiteSpace(carroAtualizado.Modelo) || string.IsNullOrWhiteSpace(carroAtualizado.Placa))
        return Results.BadRequest("Modelo e Placa são obrigatórios.");

    if (carroAtualizado.ValorDiaria <= 0)
        return Results.BadRequest("Valor da diária deve ser maior que zero.");

    carro.Modelo = carroAtualizado.Modelo;
    carro.Placa = carroAtualizado.Placa;
    carro.ValorDiaria = carroAtualizado.ValorDiaria;
    carro.Disponivel = carroAtualizado.Disponivel;

    await db.SaveChangesAsync();
    return Results.Ok(carro);
});


app.MapDelete("/carros/{id}", async (int id, AppDbContext db) =>
{
    var carro = await db.Carros.FindAsync(id);
    if (carro is null) return Results.NotFound("Carro não encontrado.");

    db.Carros.Remove(carro);
    await db.SaveChangesAsync();
    return Results.Ok("Carro removido com sucesso.");
});


app.MapGet("/locacoes", async (AppDbContext db) =>
    Results.Ok(await db.Locacoes.Include(l => l.Carro).ToListAsync()));

app.MapGet("/locacoes/{id}", async (int id, AppDbContext db) =>
{
    var locacao = await db.Locacoes.Include(l => l.Carro).FirstOrDefaultAsync(l => l.Id == id);
    return locacao is not null ? Results.Ok(locacao) : Results.NotFound("Locação não encontrada.");
});

app.MapPost("/locacoes", async (Locacao locacao, AppDbContext db) =>
{
    
    if (string.IsNullOrWhiteSpace(locacao.Cliente))
        return Results.BadRequest("Nome do cliente é obrigatório.");

    if (locacao.Dias <= 0)
        return Results.BadRequest("Quantidade de dias deve ser maior que zero.");

    var carro = await db.Carros.FindAsync(locacao.CarroId);
    if (carro is null)
        return Results.NotFound("Carro não encontrado.");

    if (!carro.Disponivel)
        return Results.BadRequest("Carro indisponível para locação.");

   
    locacao.Carro = carro;
    locacao.CalcularTotal();

    carro.Disponivel = false;

    db.Locacoes.Add(locacao);
    await db.SaveChangesAsync();

    return Results.Created($"/locacoes/{locacao.Id}", locacao);
});

app.MapPut("/locacoes/{id}", async (int id, Locacao locacaoAtualizada, AppDbContext db) =>
{
    var locacao = await db.Locacoes.Include(l => l.Carro).FirstOrDefaultAsync(l => l.Id == id);
    if (locacao is null) return Results.NotFound("Locação não encontrada.");

    if (string.IsNullOrWhiteSpace(locacaoAtualizada.Cliente))
        return Results.BadRequest("Nome do cliente é obrigatório.");

    if (locacaoAtualizada.Dias <= 0)
        return Results.BadRequest("Quantidade de dias deve ser maior que zero.");

    locacao.Cliente = locacaoAtualizada.Cliente;
    locacao.Dias = locacaoAtualizada.Dias;

    locacao.CalcularTotal();

    await db.SaveChangesAsync();
    return Results.Ok(locacao);
});

app.MapDelete("/locacoes/{id}", async (int id, AppDbContext db) =>
{
    var locacao = await db.Locacoes.Include(l => l.Carro).FirstOrDefaultAsync(l => l.Id == id);
    if (locacao is null) return Results.NotFound("Locação não encontrada.");

    
    if (locacao.Carro is not null)
        locacao.Carro.Disponivel = true;

    db.Locacoes.Remove(locacao);
    await db.SaveChangesAsync();
    return Results.Ok("Locação removida com sucesso.");
});

app.Run();
