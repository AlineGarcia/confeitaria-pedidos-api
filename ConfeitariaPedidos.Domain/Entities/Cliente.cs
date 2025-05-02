namespace ConfeitariaPedidos.Domain.Entities
{
    public class Cliente
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;

        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
