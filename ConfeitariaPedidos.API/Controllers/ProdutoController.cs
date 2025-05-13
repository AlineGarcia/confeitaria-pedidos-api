using ConfeitariaPedidos.Application.DTOs.Produto;
using Microsoft.AspNetCore.Mvc;
using ConfeitariaPedidos.Domain.Entities;
using ConfeitariaPedidos.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ConfeitariaPedidos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("ListarTodos")]
        public async Task<ActionResult<IEnumerable<ProdutoReadDto>>> GetProdutos()
        {
            var produtos = await _context.Produtos.Select(p => new ProdutoReadDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Categoria = p.Categoria,
                Preco = p.Preco,
            })
            .ToListAsync();

            return Ok(produtos);
        }

        [HttpGet("ObterPorId")]
        public async Task<ActionResult<ProdutoReadDto>> GetProdutoById(Guid id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
                return NotFound();

            var dto = new ProdutoReadDto
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Categoria = produto.Categoria,
                Preco = produto.Preco
            };

            return Ok(dto);
        }

        [HttpPost("Adicionar")]
        public async Task<ActionResult> AddProduto([FromBody]ProdutoCreateDto produtoCreateDto)
        {
            var produto = new Produto
            {
                Nome = produtoCreateDto.Nome,
                Categoria = produtoCreateDto.Categoria,
                Preco = produtoCreateDto.Preco
            };

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProdutoById), new { id = produto.Id }, produto);
        }

        [HttpPut("Atualizar")]
        public async Task<ActionResult> Update(Guid id, [FromBody]ProdutoUpdateDto produtoUpdateDto)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return NotFound();

            produto.Nome = produtoUpdateDto.Nome;
            produto.Categoria = produtoUpdateDto.Categoria;
            produto.Preco = produtoUpdateDto.Preco;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("Excluir")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
                return NotFound();

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
