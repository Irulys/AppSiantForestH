using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SaintForest.Models
{
    public class UserModel
    {
        
        [Required]//Requerido
        [EmailAddress]//formato Email
        [StringLength(50)]//Maxima Longitud
        [Display(Name = "Email")]//Label        
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]//tipo contraseña
        [StringLength(50, MinimumLength = 6)]//entre 50 y 6 Caracteres
        [Display(Name = "Contrasena")]//Label
        public string Contrasena { get; set; }

        public UserModel()
        {
        
        }

        public bool IsCliente(int Rol)
        {
            bool C =false;
                if(Rol==1)
            {
                C = true;
            }
                return C;
        }
        public bool IsMucama(int Rol)
        {
            bool C = false;
            if (Rol == 2)
            {
                C = true;
            }
            return C;
        }
    }
    }
