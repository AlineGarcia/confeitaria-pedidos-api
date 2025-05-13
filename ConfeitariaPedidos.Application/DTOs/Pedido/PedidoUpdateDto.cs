using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfeitariaPedidos.Application.DTOs.Pedido
{
    public class PedidoUpdateDto
    {
        public DateTime DataEntrega { get; set; }
        public string Observacoes { get; set; } = string.Empty;
        public string Status { get; set; } = "Pendente";
    }
}
