using ConfeitariaPedidos.Domain.Entities;
using ConfeitariaPedidos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConfeitariaPedidos.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoItem> PedidoItens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração de relacionamentos (opcional, EF já infere bem)
            modelBuilder.Entity<PedidoItem>()
                .HasOne(pi => pi.Pedido)
                .WithMany(p => p.Itens)
                .HasForeignKey(pi => pi.PedidoId);

            modelBuilder.Entity<PedidoItem>()
                .HasOne(pi => pi.Produto)
                .WithMany(p => p.Itens)
                .HasForeignKey(pi => pi.ProdutoId);
        }
    }
}
