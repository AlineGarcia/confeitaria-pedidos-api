namespace ConfeitariaPedidos.Domain.Entities
{
    public class Produto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public decimal Preco { get; set; }

        public ICollection<PedidoItem> Itens { get; set; } = new List<PedidoItem>();
    }
}
