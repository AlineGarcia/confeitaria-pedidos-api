namespace ConfeitariaPedidos.Domain.Entities
{
    public class Pedido
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ClienteId { get; set; }
        public DateTime DataEntrega { get; set; }
        public string Observacoes { get; set; } = string.Empty;
        public string Status { get; set; } = "Pendente";

        public Cliente Cliente { get; set; }
        public ICollection<PedidoItem> Itens { get; set; } = new List<PedidoItem>();
    }
}
