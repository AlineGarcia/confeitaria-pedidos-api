using ConfeitariaPedidos.Application.DTOs.Pedido;
using ConfeitariaPedidos.Application.DTOs.PedidoItem;
using ConfeitariaPedidos.Domain.Entities;
using ConfeitariaPedidos.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace ConfeitariaPedidos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PedidoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("Listar")]
        public async Task<ActionResult<IEnumerable<PedidoReadDto>>> ListarPedidos()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
                .Select(p => new PedidoReadDto
                {
                    Id = p.Id,
                    ClienteId = p.ClienteId,
                    DataEntrega = p.DataEntrega,
                    Observacoes = p.Observacoes,
                    Status = p.Status,
                    Itens = p.Itens.Select(i => new PedidoItemReadDto
                    {
                        Id = i.Id,
                        ProdutoId = i.ProdutoId,
                        Quantidade = i.Quantidade,
                        ProdutoNome = i.Produto.Nome
                    }).ToList()
                })
                .ToListAsync();

            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoReadDto>> GetById(Guid id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound();

            var dto = new PedidoReadDto
            {
                Id = pedido.Id,
                ClienteId = pedido.ClienteId,
                DataEntrega = pedido.DataEntrega,
                Observacoes = pedido.Observacoes,
                Status = pedido.Status,
                Itens = pedido.Itens.Select(i => new PedidoItemReadDto
                {
                    Id = i.Id,
                    ProdutoId = i.ProdutoId,
                    Quantidade = i.Quantidade,
                    ProdutoNome = i.Produto.Nome
                }).ToList() 
            };

            return Ok(dto);
        }

        [HttpPost("Adicionar")]
        public async Task<ActionResult> Create([FromBody] PedidoCreateDto dto)
        {
            var pedido = new Pedido
            {
                ClienteId = dto.ClienteId,
                DataEntrega = dto.DataEntrega,
                Observacoes = dto.Observacoes,
                Status = dto.Status,
                Itens = dto.Itens.Select(i => new PedidoItem
                {
                    ProdutoId = i.ProdutoId,
                    Quantidade = i.Quantidade,
                }).ToList()
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            var pedidoComProduto = await _context.Pedidos
                .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(p => p.Id == pedido.Id);

            var resultDto = new PedidoReadDto
            {
                Id = pedidoComProduto.Id,
                ClienteId = pedidoComProduto.ClienteId,
                DataEntrega = pedidoComProduto.DataEntrega,
                Observacoes = pedidoComProduto.Observacoes,
                Status = pedidoComProduto.Status,
                Itens = pedidoComProduto.Itens.Select(i => new PedidoItemReadDto
                {
                    ProdutoId = i.ProdutoId,
                    ProdutoNome = i.Produto?.Nome ?? string.Empty,
                    Quantidade = i.Quantidade,
                }).ToList()
            };

            return CreatedAtAction(nameof(GetById), new { id = pedido.Id }, pedido);
        }

        [HttpPut("{id}/Atualizar")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PedidoUpdateDto dto)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
                return NotFound();

            pedido.DataEntrega = dto.DataEntrega;
            pedido.Observacoes = dto.Observacoes;
            pedido.Status = dto.Status;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}/AtualizarItens")]
        public async Task<IActionResult> UpdateItens(Guid id, [FromBody] List<PedidoItemCreateDto> novosItens)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Itens)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound();

            _context.PedidoItens.RemoveRange(pedido.Itens);

            pedido.Itens = novosItens.Select(i => new PedidoItem
            {
                ProdutoId = i.ProdutoId,
                Quantidade = i.Quantidade,
                PedidoId = id
            }).ToList();

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}/Deletar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Itens)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound();

            _context.PedidoItens.RemoveRange(pedido.Itens);
            _context.Pedidos.Remove(pedido);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
