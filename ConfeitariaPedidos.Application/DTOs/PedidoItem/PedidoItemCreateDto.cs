using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfeitariaPedidos.Application.DTOs.PedidoItem
{
    public class PedidoItemCreateDto
    {
        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}
