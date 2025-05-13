using ConfeitariaPedidos.Application.DTOs.PedidoItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfeitariaPedidos.Application.DTOs.Pedido
{
    public class PedidoCreateDto
    {
        public Guid ClienteId { get; set; }
        public DateTime DataEntrega { get; set; }
        public string Observacoes { get; set; } = string.Empty;
        public string Status { get; set; } = "Pendente";
        public List<PedidoItemCreateDto> Itens { get; set; } = new();
    }
}
