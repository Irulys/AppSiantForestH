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
    
    public partial class EstadoDeLaReserva
    {
        public EstadoDeLaReserva()
        {
            this.Reservas = new HashSet<Reservas>();
        }
    
        public int Id { get; set; }
        public string Estado { get; set; }
    
        public virtual ICollection<Reservas> Reservas { get; set; }
    }
}
