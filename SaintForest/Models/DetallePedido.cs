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
    
    public partial class DetallePedido
    {
        public int IdPedido { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public Nullable<decimal> Iva { get; set; }
        public Nullable<decimal> Ico { get; set; }
        public Nullable<decimal> TotalIva { get; set; }
        public Nullable<decimal> TotalIco { get; set; }
        public Nullable<decimal> ValorTotal { get; set; }
        public Nullable<decimal> Descuento { get; set; }
        public int Id { get; set; }
        public Nullable<decimal> ValorUnitario { get; set; }
    
        public virtual Pedidos Pedidos { get; set; }
        public virtual Productos Productos { get; set; }
    }
}
