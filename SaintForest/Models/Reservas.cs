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
    
    public partial class Reservas
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdHabitacion { get; set; }
        public System.DateTime FechaInical { get; set; }
        public System.DateTime FechaFinal { get; set; }
        public Nullable<int> Estado { get; set; }
    
        public virtual EstadoDeLaReserva EstadoDeLaReserva { get; set; }
        public virtual Habitaciones Habitaciones { get; set; }
        public virtual Usuarios Usuarios { get; set; }
    }
}