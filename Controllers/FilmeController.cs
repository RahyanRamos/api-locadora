using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiLocadora.Dtos;
using ApiLocadora.DataContexts;
using Microsoft.EntityFrameworkCore;

namespace ApiLocadora.Controllers
{
    [Route("filmes")]
    [ApiController]
    public class FilmeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FilmeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<IActionResult> Buscar()
        {
            var listaFilmes = await _context.Filmes.ToListAsync();
            return Ok(listaFilmes);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] FilmeDto item)
        {
            var data = item.AnoLancamento;
            var filme = new Filme
            {
                Nome = item.Nome,
                Genero = item.Genero,
                AnoLancamento = new DateOnly(data.Year, data.Month, data.Day)
            };

            await _context.Filmes.AddAsync(filme);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(Buscar), new { id = filme.Id }, filme);
            return Created("", filme);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] FilmeDto item)
        {
            try
            {
                var filme = await _context.Filmes.FindAsync(id);

                if(filme is null)
                {
                    return NotFound();
                }

                filme.Nome = item.Nome;
                filme.Genero = item.Genero;
                filme.AnoLancamento = new DateOnly(item.AnoLancamento.Year, item.AnoLancamento.Month, item.AnoLancamento.Day);

                _context.Filmes.Update(filme);
                await _context.SaveChangesAsync();

                return Ok(filme);
            }
            catch(Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                var filme = await _context.Filmes.FindAsync(id);

                if(filme is null)
                {
                    return NotFound();
                }

                _context.Filmes.Remove(filme);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch(Exception e)
            {
                return Problem(e.Message);
            }
        }
    }
}
