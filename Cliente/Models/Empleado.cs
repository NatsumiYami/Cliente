using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Cliente.Models
{
    public class Empleado 
    {
        public int Id_empleado { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public DateTime Fecha_nacimiento { get; set; }
        public bool banActivo { get; set; } 


        
    }

    public class IngresoEmpleado: DbContext
    {

    }
}