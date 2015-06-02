//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SaintForest.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pedidos
    {
        public Pedidos()
        {
            this.DetallePedido = new HashSet<DetallePedido>();
        }
    
        public int Id { get; set; }
        public Nullable<int> IdCliente { get; set; }
        public Nullable<System.DateTime> FechaPedido { get; set; }
        public Nullable<System.DateTime> FechaEntrega { get; set; }
        public decimal ValorTotal { get; set; }
        public int Estado { get; set; }
        public int Cerrado { get; set; }
        public Nullable<int> IdFactura { get; set; }
    
        public virtual ICollection<DetallePedido> DetallePedido { get; set; }
        public virtual EstadosDelPedido EstadosDelPedido { get; set; }
        public virtual Facturas Facturas { get; set; }
        public virtual SiNo SiNo { get; set; }
        public virtual Usuarios Usuarios { get; set; }
    }
}
