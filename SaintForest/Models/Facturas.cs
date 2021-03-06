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
    
    public partial class Facturas
    {
        public Facturas()
        {
            this.Pedidos = new HashSet<Pedidos>();
        }
    
        public int Id { get; set; }
        public int Cliente { get; set; }
        public System.DateTime FechaFactura { get; set; }
        public decimal TotalFactura { get; set; }
        public int Estado { get; set; }
        public int Cerrada { get; set; }
    
        public virtual EstadosDeLaFactura EstadosDeLaFactura { get; set; }
        public virtual SiNo SiNo { get; set; }
        public virtual Usuarios Usuarios { get; set; }
        public virtual ICollection<Pedidos> Pedidos { get; set; }
    }
}
