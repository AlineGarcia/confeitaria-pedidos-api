namespace ConfeitariaPedidos.Domain.Entities
{
    public class PedidoItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PedidoId { get; set; }
        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }

        public Pedido Pedido { get; set; }
        public Produto Produto { get; set; }
    }
}
