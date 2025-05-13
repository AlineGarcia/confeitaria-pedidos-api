using ConfeitariaPedidos.Domain.Entities;
using ConfeitariaPedidos.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConfeitariaPedidos.Application.DTOs.Cliente;

namespace ConfeitariaPedidos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClienteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("Listar")]
        public async Task<ActionResult<IEnumerable<ClienteReadDto>>> GetClientes()
        {
            var clientes = await _context.Clientes.Select(c => new ClienteReadDto
            {
                Id = c.Id,
                Nome = c.Nome,
                Telefone = c.Telefone,
                Email = c.Email,
                Endereco = c.Endereco,
            }).ToListAsync();

            return Ok(clientes);
        }

        [HttpGet("ObterPorId")]
        public async Task<ActionResult<ClienteReadDto>> GetCliente(Guid id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound();

            var dto = new ClienteReadDto
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Telefone = cliente.Telefone,
                Email = cliente.Email,
                Endereco = cliente.Endereco
            };

            return Ok(dto);
        }

        [HttpPost("Adicionar")]
        public async Task<ActionResult> Post([FromBody] ClienteCreateDto dto)
        {
            var cliente = new Cliente()
            {
                Nome = dto.Nome,
                Telefone = dto.Telefone,
                Email = dto.Email,
                Endereco = dto.Endereco
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id =  cliente.Id }, cliente);
        }

        [HttpPut("Atualizar")]
        public async Task<IActionResult> PutCliente(Guid id, [FromBody] ClienteUpdateDto dto)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound();

            cliente.Nome = dto.Nome;
            cliente.Telefone = dto.Telefone;
            cliente.Email = dto.Email;
            cliente.Endereco = dto.Endereco;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("Excluir")]
        public async Task<IActionResult> DeleteCliente(Guid id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) 
                return NotFound();

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
