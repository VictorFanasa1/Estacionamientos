using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Estacionamiento.Models
{
    public class MUsuarios
    {
        public MUsuarios()
        {

        }

        public MUsuarios(DataRow r)
        {
            IdUsuarios = Convert.ToInt32(r["IdUsuarios"]);

            if (r["uiNumeroEmpleado"] != DBNull.Value)
            {
                uiNumeroEmpleado = Convert.ToInt32(r["uiNumeroEmpleado"]);
            }
            
            sNombre = Convert.ToString(r["sNombre"]);

            if (r["sCorreo"] != DBNull.Value)
            {
                sCorreo = Convert.ToString(r["sCorreo"]);
            }

        }

        public int IdUsuarios { get; set; }

        public int uiNumeroEmpleado { get; set; }

        public string sNombre { get; set; }

        public string sCorreo { get; set; }

        public int iRol { get; set; }

        public bool bActivo { get; set; }

        public string sRoleUsuarios { get; set; }

        public string nombreResponsable { get; set; }

        public int idUserResponsables { get; set; }

    }
}