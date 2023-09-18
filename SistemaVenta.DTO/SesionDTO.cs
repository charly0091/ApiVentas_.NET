using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DTO
{
    public class SesionDTO
    {
        public int? IdUsuario { get; set; }
        public String? NombreCompleto { get; set; }
        public String? Correo { get; set; }
        public String? RolDescripcion { get; set; }
    }
}
