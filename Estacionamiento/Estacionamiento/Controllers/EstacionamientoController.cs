using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Estacionamiento.Models;
using System.Configuration;
using System.DirectoryServices;

namespace Estacionamiento.Controllers
{
    public class EstacionamientoController : Controller
    {
        // GET: Estacionamiento
        public ActionResult AdmEstacionamientos()
        {
            return View();
        }

        [NonAction]
        private List<MLugares> ObtenerCatalogosBD()
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;
            SqlConnection conexionSQL = new SqlConnection(cadenaConexion);

            SqlCommand comando = new SqlCommand("Select * from catLugares", conexionSQL);

            SqlDataAdapter da = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<MLugares> listaLugares = new List<MLugares>();

            foreach (DataRow r in dt.Rows)
            {
                MLugares lugares = new MLugares(r);
                listaLugares.Add(lugares);
            }

            return listaLugares;
        }

        public JsonResult MostrarCatalogoLugares_AJAX()
        {
            try
            {
                List<MLugares> listaCalagos = ObtenerCatalogosBD();

                return Json(new { error = false, listaCalagos }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = true, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [NonAction]
        private List<MLugares> ObtenerCatalogosVisitantesBD()
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;
            SqlConnection conexionSQL = new SqlConnection(cadenaConexion);

            SqlCommand comando = new SqlCommand("Select * from catLugares where bVisita = 1", conexionSQL);

            SqlDataAdapter da = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<MLugares> listaLugares = new List<MLugares>();

            foreach (DataRow r in dt.Rows)
            {
                MLugares lugares = new MLugares(r);
                listaLugares.Add(lugares);
            }

            return listaLugares;
        }

        public JsonResult MostrarCatalogoLugaresVisitante_AJAX()
        {
            try
            {
                List<MLugares> listaCalagos = ObtenerCatalogosVisitantesBD();

                return Json(new { error = false, listaCalagos }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = true, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [NonAction]
        private List<MLugarDireccion> ObtenerCatalogosLugaresBD(int direccion)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;
            SqlConnection conexionSQL = new SqlConnection(cadenaConexion);

            SqlCommand comando = new SqlCommand(string.Format("Select * from catNumeroLugares where iLugar = {0} and bActivo = 1 and bVisitante = 0 and bPresidencia = 0", direccion), conexionSQL);

            SqlDataAdapter da = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<MLugarDireccion> listaLugares = new List<MLugarDireccion>();

            foreach (DataRow r in dt.Rows)
            {
                MLugarDireccion lugares = new MLugarDireccion(r);
                listaLugares.Add(lugares);
            }

            return listaLugares;
        }

        public JsonResult MostrarCatalogoLugaresPorDireccion_AJAX(int direccion)
        {
            try
            {
                List<MLugarDireccion> listaCalagos = ObtenerCatalogosLugaresBD(direccion);

                return Json(new { error = false, listaCalagos }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = true, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [NonAction]
        private List<MLugarDireccion> ObtenerCatalogosLugaresVisitantesBD(int direccion)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;
            SqlConnection conexionSQL = new SqlConnection(cadenaConexion);

            SqlCommand comando = new SqlCommand(string.Format("Select * from catNumeroLugares where iLugar = {0} and bActivo = 1 and bVisitante = 1", direccion), conexionSQL);

            SqlDataAdapter da = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<MLugarDireccion> listaLugares = new List<MLugarDireccion>();

            foreach (DataRow r in dt.Rows)
            {
                MLugarDireccion lugares = new MLugarDireccion(r);
                listaLugares.Add(lugares);
            }

            return listaLugares;
        }

        public JsonResult MostrarCatalogoLugaresPorDireccionVisitantes_AJAX(int direccion)
        {
            try
            {
                List<MLugarDireccion> listaCalagos = ObtenerCatalogosLugaresVisitantesBD(direccion);

                return Json(new { error = false, listaCalagos }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = true, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult BuscarUserAD_AJAX(string parametroBusqueda, string errorMensaje)
        {
            try
            {
                MUsuarios Usuario = new MUsuarios();

                string Cuenta = "";
                string noEmpleado = "";
                string nombreEmpleado = "";
                string correoUsuario = "";
                string areaUsuario = "";
                string cargoUsuario = "";
                string telefonoUusario = "";
                string ubicacionUsuario = "";
                string Company = "";

                string UAN = "LDAP://gf.grupofarmacos.net/DC=gf,DC=grupofarmacos,DC=net";

                DirectoryEntry oroot = new DirectoryEntry(UAN, "GFLSAEC1", "Sa3cgf19*", AuthenticationTypes.Secure);
                DirectorySearcher nombre = new DirectorySearcher(oroot);
                int NoBuscarEmpleado;

                if (int.TryParse(parametroBusqueda, out NoBuscarEmpleado))
                {
                    nombre.Filter = $"employeeNumber= {NoBuscarEmpleado}";
                }

                else
                {
                    nombre.Filter = $"sAMAccountName= {parametroBusqueda}";
                }

                nombre.PropertiesToLoad.Add("sAMAccountName"); /* Cuenta de Empleado*/
                nombre.PropertiesToLoad.Add("employeeNumber"); /* Número de Empleado*/
                nombre.PropertiesToLoad.Add("cn"); /* Nombre de Empleado*/
                nombre.PropertiesToLoad.Add("mail"); /* Correo de Empleado*/
                nombre.PropertiesToLoad.Add("description"); /* Área de Empleado*/
                nombre.PropertiesToLoad.Add("title"); /* Cargo de Empleado*/
                nombre.PropertiesToLoad.Add("telephoneNumber"); /* Teléfono de Empleado*/
                nombre.PropertiesToLoad.Add("physicalDeliveryOfficeName"); /* Ubicación de Empleado*/
                nombre.PropertiesToLoad.Add("Company"); /* UEN de Empleado*/

                if (nombre.FindAll().Count > 0)
                {
                    foreach (SearchResult resultNombre in nombre.FindAll())
                    {
                        try
                        {
                            noEmpleado = (string)resultNombre.Properties["employeeNumber"][0];
                            Usuario.uiNumeroEmpleado = Convert.ToInt32(noEmpleado);
                        }
                        catch
                        {
                            noEmpleado = "";
                        }

                        try
                        {
                            nombreEmpleado = (string)resultNombre.Properties["cn"][0];
                            Usuario.sNombre = nombreEmpleado;
                        }

                        catch
                        {

                        }

                        try
                        {
                            correoUsuario = (string)resultNombre.Properties["mail"][0];
                            Usuario.sCorreo = correoUsuario;
                        }

                        catch
                        {

                        }

                    }
                }

                else
                {
                    errorMensaje = "Error: El usuario no existe.";
                    return Json(new { error = true, mensaje = errorMensaje }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { Usuario, error = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = true, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [NonAction]

        private void ValidarUsuarioEmpleadoBD(int numeroEmpleado, string nombreUser)
        {
            bool filas;
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;

            using (SqlConnection conexionSQL = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexionSQL.Open();
                    using (SqlCommand comando = new SqlCommand("sp_ValidarUsuarioEmpelado", conexionSQL))
                    {
                        comando.CommandType = CommandType.StoredProcedure;

                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@uiNumeroEmpleado", Value = numeroEmpleado });

                        filas = Convert.ToBoolean(comando.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                finally
                {
                    conexionSQL.Close();
                    conexionSQL.Dispose();
                }

                if (filas == true)
                {
                    throw new ApplicationException(string.Format("El usuario: [{0}] ya cuenta con cajón de Estacionamiento", nombreUser));
                }
            }

        }

        [NonAction]
        private MUsuarios ValidarExistenciaUsuarioEmpleadoBD(int numeroEmpleado)
        {
            MUsuarios usuario = new MUsuarios();
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;

            using (SqlConnection conexionSQL = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexionSQL.Open();
                    using (SqlCommand comando = new SqlCommand(string.Format("Select * from usuarios where uiNumeroEmpleado = {0} and iRol = 1 and bActivo = 1", numeroEmpleado),conexionSQL))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(comando);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count == 0)
                        {
                            usuario = new MUsuarios();
                        }
                        else
                        {
                            DataRow r = dt.Rows[0];
                            usuario.IdUsuarios = Convert.ToInt32(r["IdUsuarios"]);
                            usuario.uiNumeroEmpleado = Convert.ToInt32(r["uiNumeroEmpleado"]);
                            usuario.sNombre = Convert.ToString(r["sNombre"]);
                        }

                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                finally
                {
                    conexionSQL.Close();
                    conexionSQL.Dispose();
                }

            }

            return usuario;
        }

        [NonAction]
        private MEstacionamiento ObtenerCajonEmpleadoBD(int registroEmpleado)
        {
            MEstacionamiento estacionamiento = new MEstacionamiento();
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;

            using (SqlConnection conexionSQL = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexionSQL.Open();
                    using (SqlCommand comando = new SqlCommand(string.Format("Select * from cajonesEstacionamientos where iUsuario = {0}", registroEmpleado), conexionSQL))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(comando);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        DataRow r = dt.Rows[0];
                        estacionamiento.uiRegistroCajones = Convert.ToInt32(r["uiRegistroCajones"]);
                        estacionamiento.iCajon = Convert.ToInt32(r["iCajon"]);
                        estacionamiento.iUsuario = Convert.ToInt32(r["iUsuario"]);
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                finally
                {
                    conexionSQL.Close();
                    conexionSQL.Dispose();
                }

            }

            return estacionamiento;
        }

        [NonAction]
        private int AgregarUsuariosEmpleadosBD(int numeroEmpleado, string nombreUser, string correo)
        {
            int registroUsuario = 0;
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;

            using (SqlConnection conexionSQL = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexionSQL.Open();
                    using (SqlCommand comando = new SqlCommand("sp_InsertUsuariosEmpleados", conexionSQL))
                    {
                        comando.CommandType = CommandType.StoredProcedure;

                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@uiNumeroEmpleado", Value = numeroEmpleado });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.VarChar, ParameterName = "@sNombre", Size = 200, Value = nombreUser });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.VarChar, ParameterName = "@sCorreo", Size = 100, Value = correo });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@IdUsuarios", Direction = ParameterDirection.Output });

                        int filas = 0;

                        filas = comando.ExecuteNonQuery();
                        registroUsuario = Convert.ToInt32(comando.Parameters["@IdUsuarios"].Value);

                        if (filas != 1)
                        {
                            throw new ApplicationException(string.Format("No se agregó el usuario:{0}", nombreUser));
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    conexionSQL.Close();
                    conexionSQL.Dispose();

                }
            }

            return registroUsuario;
        }

        [NonAction]
        private void AgregarAsignacionEstacionamientoEmpleadosBD(int cajon, int lugar, int numUsuario, string observaciones)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;

            using (SqlConnection conexionSQL = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexionSQL.Open();
                    using (SqlCommand comando = new SqlCommand("sp_InsertEstacionamiento", conexionSQL))
                    {
                        comando.CommandType = CommandType.StoredProcedure;

                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@iCajon", Value = cajon });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@iLugar", Value = lugar });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@iUsuario", Value = numUsuario });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.VarChar, ParameterName = "@sObservaciones", Value = observaciones });

                        int filas = 0;

                        filas = comando.ExecuteNonQuery();

                        if (filas != 1)
                        {
                            throw new ApplicationException(string.Format("No se asignó el lugar de estacionamient al usuario: {0}", numUsuario));
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    conexionSQL.Close();
                    conexionSQL.Dispose();
                }
            }

        }

        [NonAction]

        private MLugarDireccion ObtenerLugarDireccionBD(int numeroLugar, string lugar)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;
            SqlConnection conexionSQL = new SqlConnection(cadenaConexion);

            SqlCommand comando = new SqlCommand(string.Format("Select * from catNumeroLugares where iLugar = {0} and sLugar = '{1}'", numeroLugar, lugar), conexionSQL);
            SqlDataAdapter da = new SqlDataAdapter(comando);

            DataTable dt = new DataTable();
            da.Fill(dt);

            MLugarDireccion lugarDireccion = new MLugarDireccion();

            if (dt.Rows.Count == 0)
            {
                throw new ApplicationException("Este Cajón de estacionamiento ya no cuenta con Lugares disponibles");
            }
            else
            {
                DataRow r = dt.Rows[0];
                lugarDireccion = new MLugarDireccion(r);
            }

            return lugarDireccion;
        }

        [NonAction]

        private void CambiarEstatusLugaresBD(int lugar, string nombreLugar)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;

            using (SqlConnection conexionSQL = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexionSQL.Open();
                    using (SqlCommand comando = new SqlCommand("sp_CambiarStatusLugar", conexionSQL))
                    {
                        comando.CommandType = CommandType.StoredProcedure;

                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@iLugar", Value = lugar });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.VarChar, ParameterName = "@sLugar", Size = 50, Value = nombreLugar });

                        int filas = 0;
                        filas = comando.ExecuteNonQuery();

                        if (filas == 0)
                        {
                            throw new ApplicationException("No se editó el estatus del Lugar");
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                finally
                {
                    conexionSQL.Close();
                    conexionSQL.Dispose();
                }
            }

        }

        public JsonResult AsignarEstacionamientoEmpleado_AJAX(MEstacionamiento estacionamiento)
        {
            try
            {
                //ValidarUsuarioEmpleadoBD(estacionamiento.iUsuario, estacionamiento.nombreUsuario);
                bool validacion = false;
                MUsuarios user = ValidarExistenciaUsuarioEmpleadoBD(estacionamiento.iUsuario);

                if (user.IdUsuarios != 0)
                {
                    MEstacionamiento estaciona = ObtenerCajonEmpleadoBD(user.IdUsuarios);

                    if (estaciona.iCajon == estacionamiento.iCajon)
                    {
                        validacion = true;
                    }
                    else
                    {
                        int registroUsuario = AgregarUsuariosEmpleadosBD(estacionamiento.iUsuario, estacionamiento.nombreUsuario, estacionamiento.correoUsuario);

                        MLugarDireccion lugarDireccion = ObtenerLugarDireccionBD(estacionamiento.iCajon, estacionamiento.sLugar);
                        CambiarEstatusLugaresBD(lugarDireccion.iLugar, lugarDireccion.sLugar);
                        AgregarAsignacionEstacionamientoEmpleadosBD(estacionamiento.iCajon, lugarDireccion.uiRegistroLugar, registroUsuario, estacionamiento.sObservaciones);
                    }
                }

                else
                {
                    int registroUsuario = AgregarUsuariosEmpleadosBD(estacionamiento.iUsuario, estacionamiento.nombreUsuario, estacionamiento.correoUsuario);

                    MLugarDireccion lugarDireccion = ObtenerLugarDireccionBD(estacionamiento.iCajon, estacionamiento.sLugar);
                    CambiarEstatusLugaresBD(lugarDireccion.iLugar, lugarDireccion.sLugar);
                    AgregarAsignacionEstacionamientoEmpleadosBD(estacionamiento.iCajon, lugarDireccion.uiRegistroLugar, registroUsuario, estacionamiento.sObservaciones);
                }

                return Json(new { error = false, validacion }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = true, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [NonAction]
        private int AgregarUsuariosEmpleadosResponsablesBD(int numeroEmpleado, string nombreUser, string correo)
        {
            int registroUsuario = 0;
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;

            using (SqlConnection conexionSQL = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexionSQL.Open();
                    using (SqlCommand comando = new SqlCommand("sp_InsertUsuariosEmpleadosResponsables", conexionSQL))
                    {
                        comando.CommandType = CommandType.StoredProcedure;

                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@uiNumeroEmpleado", Value = numeroEmpleado });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.VarChar, ParameterName = "@sNombre", Size = 200, Value = nombreUser });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.VarChar, ParameterName = "@sCorreo", Size = 100, Value = correo });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@IdUsuarios", Direction = ParameterDirection.Output });

                        int filas = 0;

                        filas = comando.ExecuteNonQuery();
                        registroUsuario = Convert.ToInt32(comando.Parameters["@IdUsuarios"].Value);

                        if (filas != 1)
                        {
                            throw new ApplicationException(string.Format("No se agregó el usuario:{0}", nombreUser));
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    conexionSQL.Close();
                    conexionSQL.Dispose();

                }
            }

            return registroUsuario;
        }

        [NonAction]
        private int AgregarUsuariosVisitantesBD(string nombreUser, int registroUserResponsable)
        {
            int registroUsuario = 0;
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;

            using (SqlConnection conexionSQL = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexionSQL.Open();
                    using (SqlCommand comando = new SqlCommand("sp_InsertUsuariosVisitantes", conexionSQL))
                    {
                        comando.CommandType = CommandType.StoredProcedure;

                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.VarChar, ParameterName = "@sNombre", Size = 200, Value = nombreUser });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@iRegistroUserResponsable", Value = registroUserResponsable });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@IdUsuarios", Direction = ParameterDirection.Output });

                        int filas = 0;

                        filas = comando.ExecuteNonQuery();
                        registroUsuario = Convert.ToInt32(comando.Parameters["@IdUsuarios"].Value);

                        if (filas != 1)
                        {
                            throw new ApplicationException(string.Format("No se agregó el usuario:{0}", nombreUser));
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    conexionSQL.Close();
                    conexionSQL.Dispose();

                }
            }

            return registroUsuario;
        }

        [NonAction]
        private int AgregarAsignacionEstacionamientoVisitantesBD(int cajon, int lugar, int numUsuario, string observaciones, DateTime fechaVisita, string horaVisita, string horaSalida, string placa, string modelo, string color)
        {
            int registroCajon = 0;
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;

            using (SqlConnection conexionSQL = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexionSQL.Open();
                    using (SqlCommand comando = new SqlCommand("sp_InsertEstacionamientoVisitantes", conexionSQL))
                    {
                        comando.CommandType = CommandType.StoredProcedure;

                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@iCajon", Value = cajon });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@iLugar", Value = lugar });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@iUsuario", Value = numUsuario });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.VarChar, ParameterName = "@sObservaciones", Value = observaciones });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.DateTime, ParameterName = "@dtFechaVisita", Value = fechaVisita });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.VarChar, ParameterName = "@horaVisita", Size = 50, Value = horaVisita });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.VarChar, ParameterName = "@horaSalida", Size = 50, Value = horaSalida });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.VarChar, ParameterName = "@sPlaca", Size = 50, Value = placa });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.VarChar, ParameterName = "@sModelo", Size = 50, Value = modelo });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.VarChar, ParameterName = "@sColor", Size = 50, Value = color });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@uiRegistroCajones", Direction = ParameterDirection.Output });

                        int filas = 0;

                        filas = comando.ExecuteNonQuery();
                        registroCajon = Convert.ToInt32(comando.Parameters["@uiRegistroCajones"].Value);
                        return registroCajon;

                        if (filas != 1)
                        {
                            throw new ApplicationException(string.Format("No se asignó el lugar de estacionamient al usuario: {0}", numUsuario));
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    conexionSQL.Close();
                    conexionSQL.Dispose();
                }
            }

        }

        public JsonResult AsignarEstacionamientoVisitante_AJAX(MEstacionamiento estacionamiento)
        {
            try
            {
                MLugarDireccion lugarDireccion = ObtenerLugarDireccionBD(estacionamiento.iCajon, estacionamiento.sLugar);
                CambiarEstatusLugaresBD(lugarDireccion.iLugar, lugarDireccion.sLugar);

                int userResponsable = AgregarUsuariosEmpleadosResponsablesBD(estacionamiento.iUsuario, estacionamiento.nombreUsuario, estacionamiento.correoUsuario);

                int registroUsuario = AgregarUsuariosVisitantesBD(estacionamiento.nombreVisitante, userResponsable);
                AgregarAsignacionEstacionamientoVisitantesBD(estacionamiento.iCajon, lugarDireccion.uiRegistroLugar, registroUsuario, estacionamiento.sObservaciones, estacionamiento.dtFechaVisita, estacionamiento.horaVisita, estacionamiento.horaSalida, estacionamiento.sPlaca, estacionamiento.sModelo, estacionamiento.sColor);

                return Json(new { error = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = true, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [NonAction]
        private List<MEstacionamiento> ListaEstacionamientosAsignados()
        {
            List<MEstacionamiento> listaEstacionamientos = new List<MEstacionamiento>();
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;

            using (SqlConnection conexionSQL = new SqlConnection(cadenaConexion))
            {
                try
                {
                    using (SqlCommand comando = new SqlCommand("sp_MostrarAsignacionesEstacionamiento", conexionSQL))
                    {
                        comando.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter(comando);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        foreach (DataRow r in dt.Rows)
                        {
                            MEstacionamiento estacionamiento = new MEstacionamiento();
                            estacionamiento.uiRegistroCajones = Convert.ToInt32(r["uiRegistroCajones"]);
                            estacionamiento.dtFechaAsignacion = Convert.ToDateTime(r["dtFechaAsignacion"]);
                            estacionamiento.sObservaciones = Convert.ToString(r["sObservaciones"]);
                            estacionamiento.Lugares = new MLugares();
                            estacionamiento.Lugares.sLugar = Convert.ToString(r["sLugar"]);
                            estacionamiento.Usuarios = new MUsuarios();
                            estacionamiento.Usuarios.sNombre = Convert.ToString(r["sNombre"]);
                            estacionamiento.Usuarios.uiNumeroEmpleado = Convert.ToInt32(r["uiNumeroEmpleado"]);
                            estacionamiento.Usuarios.sRoleUsuarios = Convert.ToString(r["sRol"]);
                            estacionamiento.Usuarios.sCorreo = Convert.ToString(r["sCorreo"]);
                            estacionamiento.LugarDireccion = new MLugarDireccion();
                            estacionamiento.LugarDireccion.sLugar = Convert.ToString(r["LugarDireccion"]);
                            listaEstacionamientos.Add(estacionamiento);
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {

                }
            }

            return listaEstacionamientos;
        }

        public JsonResult MostrarAsignacionesEstacionamientos_AJAX()
        {
            try
            {
                List<MEstacionamiento> listaEstacionamientos = ListaEstacionamientosAsignados();

                return Json(new { error = false, listaEstacionamientos }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = true, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [NonAction]
        private List<MEstacionamiento> ListaEstacionamientosAsignadosVisitantes()
        {
            List<MEstacionamiento> listaEstacionamientos = new List<MEstacionamiento>();
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;

            using (SqlConnection conexionSQL = new SqlConnection(cadenaConexion))
            {
                try
                {
                    using (SqlCommand comando = new SqlCommand("sp_MostrarAsignacionesEstacionamientoVisitantes", conexionSQL))
                    {
                        comando.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter(comando);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        foreach (DataRow r in dt.Rows)
                        {
                            MEstacionamiento estacionamiento = new MEstacionamiento();
                            estacionamiento.uiRegistroCajones = Convert.ToInt32(r["uiRegistroCajones"]);
                            estacionamiento.horaVisita = Convert.ToString(r["horaVisita"]);
                            estacionamiento.horaSalida = Convert.ToString(r["horaSalida"]);
                            estacionamiento.dtFechaAsignacion = Convert.ToDateTime(r["dtFechaAsignacion"]);
                            estacionamiento.dtFechaVisita = Convert.ToDateTime(r["dtFechaVisita"]);
                            estacionamiento.sObservaciones = Convert.ToString(r["sObservaciones"]);
                            estacionamiento.Lugares = new MLugares();
                            estacionamiento.Lugares.sLugar = Convert.ToString(r["sLugar"]);
                            estacionamiento.Usuarios = new MUsuarios();
                            estacionamiento.Usuarios.sNombre = Convert.ToString(r["sNombre"]);
                            estacionamiento.Usuarios.sRoleUsuarios = Convert.ToString(r["sRol"]);
                            estacionamiento.Usuarios.nombreResponsable = Convert.ToString(r["NombreResponsable"]);
                            estacionamiento.LugarDireccion = new MLugarDireccion();
                            estacionamiento.LugarDireccion.sLugar = Convert.ToString(r["LugarDireccion"]);
                            estacionamiento.sPlaca = Convert.ToString(r["sPlaca"]);
                            estacionamiento.sModelo = Convert.ToString(r["sModelo"]);
                            estacionamiento.sColor = Convert.ToString(r["sColor"]);
                            listaEstacionamientos.Add(estacionamiento);
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {

                }
            }

            return listaEstacionamientos;
        }

        public JsonResult MostrarAsignacionesEstacionamientosVisitantes_AJAX()
        {
            try
            {
                List<MEstacionamiento> listaEstacionamientos = ListaEstacionamientosAsignadosVisitantes();

                return Json(new { error = false, listaEstacionamientos }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = true, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [NonAction]

        private MEstacionamiento ObtenerEstacionamientoLiberaBD(int registroCajon)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;
            SqlConnection conexionSQL = new SqlConnection(cadenaConexion);

            SqlCommand comando = new SqlCommand(string.Format("Select * from cajonesEstacionamientos where uiRegistroCajones = {0}", registroCajon), conexionSQL);

            SqlDataAdapter da = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            da.Fill(dt);

            DataRow r = dt.Rows[0];
            MEstacionamiento estacionamiento = new MEstacionamiento(r);

            return estacionamiento;
        }

        [NonAction]
        private void LiberarCajonEstacionamientoBD(int registroCajon)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;

            using (SqlConnection conexionSQL = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexionSQL.Open();
                    using (SqlCommand comando = new SqlCommand("sp_UpdateEstacionamiento", conexionSQL))
                    {
                        comando.CommandType = CommandType.StoredProcedure;

                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@uiRegistroCajones", Value = registroCajon });

                        int filas = 0;
                        filas = comando.ExecuteNonQuery();

                        if (filas == 0)
                        {
                            throw new ApplicationException("No se liberó cajón de estacionamiento");
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                finally
                {
                    conexionSQL.Close();
                    conexionSQL.Dispose();
                }
            }
        }

        [NonAction]
        private void LiberarCajonLugarEstacionamientoBD(int registroLugar)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;

            using (SqlConnection conexionSQL = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexionSQL.Open();
                    using (SqlCommand comando = new SqlCommand("sp_UpdateLugaresEstacionamiento", conexionSQL))
                    {
                        comando.CommandType = CommandType.StoredProcedure;

                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@uiRegistroLugar", Value = registroLugar });

                        int filas = 0;
                        filas = comando.ExecuteNonQuery();

                        if (filas == 0)
                        {
                            throw new ApplicationException("No se liberó lugar de estacionamiento");
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                finally
                {
                    conexionSQL.Close();
                    conexionSQL.Dispose();
                }
            }
        }

        [NonAction]
        private void LiberarUsuarioEstacionamiento(int idUser)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;

            using (SqlConnection conexionSQL = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexionSQL.Open();
                    using (SqlCommand comando = new SqlCommand("sp_UpdateUsuarios", conexionSQL))
                    {
                        comando.CommandType = CommandType.StoredProcedure;

                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@IdUsuarios", Value = idUser });

                        int filas = 0;
                        filas = comando.ExecuteNonQuery();

                        if (filas == 0)
                        {
                            throw new ApplicationException("No se desactivo el Usuario");
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                finally
                {
                    conexionSQL.Close();
                    conexionSQL.Dispose();
                }
            }
        }


        public JsonResult LiberarCajon_AJAX(int registroCajon)
        {
            try
            {
                MEstacionamiento estacionamiento = ObtenerEstacionamientoLiberaBD(registroCajon);

                LiberarCajonEstacionamientoBD(estacionamiento.uiRegistroCajones);
                LiberarCajonLugarEstacionamientoBD(estacionamiento.iLugar);
                LiberarUsuarioEstacionamiento(estacionamiento.iUsuario);

                return Json(new { error = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = true, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [NonAction]

        private void EditarHoraSalidaBD(string horaSalida, int registroCajon)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;

            using (SqlConnection conexionSQL = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexionSQL.Open();
                    using (SqlCommand comando = new SqlCommand("sp_UpdateHoraSalida", conexionSQL))
                    {
                        comando.CommandType = CommandType.StoredProcedure;

                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.VarChar, ParameterName = "@horaSalida", Size = 50, Value = horaSalida });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@uiRegistroCajones", Value = registroCajon });

                        int filas = 0;
                        filas = comando.ExecuteNonQuery();

                        if (filas == 0)
                        {
                            throw new ApplicationException("No se editó la hora de Salida");
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                finally
                {
                    conexionSQL.Close();
                    conexionSQL.Dispose();
                }
            }
        }

        public JsonResult EditarHoraSalida_AJAX(MEstacionamiento estacionamiento)
        {
            try
            {
                EditarHoraSalidaBD(estacionamiento.horaSalida, estacionamiento.uiRegistroCajones);

                return Json(new { error = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = true, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #region AltaCajonesVisitantes
        public ActionResult AltaCajonVisitantes()
        {
            return View();
        }


        [NonAction]

        private bool ValidarUsuarioVisitanteEstacionamiento(string nombre)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;
            SqlConnection conexionSQL = new SqlConnection(cadenaConexion);

            SqlCommand comando = new SqlCommand(string.Format("Select * from usuarios where sNombre = '{0}'",nombre), conexionSQL);

            SqlDataAdapter da = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            da.Fill(dt);

            bool validaUser = false;

            if (dt.Rows.Count == 0)
            {
                validaUser = false;
            }
            else
            {
                validaUser = true;
            }

            return validaUser;
        }

        [NonAction]

        private bool ValidaEstacionamientoVisitaBD(string fechaVisita, string horaEntrada, string horaSalida )
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;
            SqlConnection conexionSQL = new SqlConnection(cadenaConexion);

            SqlCommand comando = new SqlCommand(string.Format("Select * from cajonesEstacionamientos where dtFechaVisita = '{0}' and horaVisita = '{1}' and horaSalida = '{2}'", fechaVisita, horaEntrada, horaSalida),conexionSQL);

            SqlDataAdapter da = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            da.Fill(dt);

            bool validaEstaciona = false;

            if (dt.Rows.Count == 0)
            {
                validaEstaciona = false;
            }
            else
            {
                validaEstaciona = true;
            }

            return validaEstaciona;
        }

        [NonAction]

        private void ValidacionSolicitudesUusariosBD(int numEmpleado)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;
            SqlConnection conexionSQL = new SqlConnection(cadenaConexion);

            SqlCommand comando = new SqlCommand("sp_ValidarSolicitudesActivasUser", conexionSQL);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@uiNumeroEmpleado", Value = numEmpleado });

            SqlDataAdapter da = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count >=3)
            {
                throw new ApplicationException("Ha alcanzado el máximo de solicitudes (3), debe esperar a que se libere al menos una.");
            }
            else
            {

            }
        }

        public JsonResult AsignarEstacionamientoVisitanteUsuario_AJAX(MEstacionamiento estacionamiento)
        {
            try
            {

                ValidacionSolicitudesUusariosBD(estacionamiento.iUsuario);

                bool validaUser = ValidarUsuarioVisitanteEstacionamiento(estacionamiento.nombreVisitante);
                int registroCajon = 0;

                if (validaUser == true)
                {
                    string fechaVisita = estacionamiento.dtFechaVisita.ToString("yyyy-MM-dd");

                    bool validaEstacionamiento = ValidaEstacionamientoVisitaBD(fechaVisita, estacionamiento.horaVisita, estacionamiento.horaSalida);

                    if (validaEstacionamiento == true)
                    {
                        throw new ApplicationException("Ya existe una solicitud con estos datos.");
                    }
                    else
                    {
                        int userResponsable = AgregarUsuariosEmpleadosResponsablesBD(estacionamiento.iUsuario, estacionamiento.nombreUsuario, estacionamiento.correoUsuario);

                        int registroUsuario = AgregarUsuariosVisitantesBD(estacionamiento.nombreVisitante, userResponsable);
                        registroCajon = AgregarAsignacionEstacionamientoVisitantesBD(estacionamiento.iCajon, estacionamiento.iLugar, registroUsuario, estacionamiento.sObservaciones, estacionamiento.dtFechaVisita, estacionamiento.horaVisita, estacionamiento.horaSalida, estacionamiento.sPlaca, estacionamiento.sModelo, estacionamiento.sColor);
                    }
                }
                else
                {
                    int userResponsable = AgregarUsuariosEmpleadosResponsablesBD(estacionamiento.iUsuario, estacionamiento.nombreUsuario, estacionamiento.correoUsuario);

                    int registroUsuario = AgregarUsuariosVisitantesBD(estacionamiento.nombreVisitante, userResponsable);
                    registroCajon = AgregarAsignacionEstacionamientoVisitantesBD(estacionamiento.iCajon, estacionamiento.iLugar, registroUsuario, estacionamiento.sObservaciones, estacionamiento.dtFechaVisita, estacionamiento.horaVisita, estacionamiento.horaSalida, estacionamiento.sPlaca, estacionamiento.sModelo, estacionamiento.sColor);
                }

                return Json(new { error = false, registroCajon }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = true, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        [NonAction]
        private List<MEstacionamiento> ListaEstacionamientosAsignadosVisitantesSinLugar()
        {
            List<MEstacionamiento> listaEstacionamientos = new List<MEstacionamiento>();
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;

            using (SqlConnection conexionSQL = new SqlConnection(cadenaConexion))
            {
                try
                {
                    using (SqlCommand comando = new SqlCommand("sp_MostrarAsignacionesEstacionamientoVisitantesSinLugar", conexionSQL))
                    {
                        comando.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter(comando);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        foreach (DataRow r in dt.Rows)
                        {
                            MEstacionamiento estacionamiento = new MEstacionamiento();
                            estacionamiento.uiRegistroCajones = Convert.ToInt32(r["uiRegistroCajones"]);
                            estacionamiento.horaVisita = Convert.ToString(r["horaVisita"]);
                            estacionamiento.horaSalida = Convert.ToString(r["horaSalida"]);
                            estacionamiento.dtFechaAsignacion = Convert.ToDateTime(r["dtFechaAsignacion"]);
                            estacionamiento.dtFechaVisita = Convert.ToDateTime(r["dtFechaVisita"]);
                            estacionamiento.sObservaciones = Convert.ToString(r["sObservaciones"]);
                            estacionamiento.Lugares = new MLugares();
                            estacionamiento.Lugares.sLugar = Convert.ToString(r["sLugar"]);

                            if (estacionamiento.Lugares.sLugar == "")
                            {
                                estacionamiento.Lugares.sLugar = "S/A";
                            }
                            else
                            {
                                estacionamiento.Lugares.sLugar = Convert.ToString(r["sLugar"]);
                            }

                            estacionamiento.Usuarios = new MUsuarios();
                            estacionamiento.Usuarios.sNombre = Convert.ToString(r["sNombre"]);
                            estacionamiento.Usuarios.sRoleUsuarios = Convert.ToString(r["sRol"]);
                            estacionamiento.Usuarios.nombreResponsable = Convert.ToString(r["NombreResponsable"]);
                            estacionamiento.LugarDireccion = new MLugarDireccion();
                            estacionamiento.LugarDireccion.sLugar = Convert.ToString(r["LugarDireccion"]);

                            if (estacionamiento.LugarDireccion.sLugar == "")
                            {
                                estacionamiento.LugarDireccion.sLugar = "S/A";
                            }
                            else
                            {
                                estacionamiento.LugarDireccion.sLugar = Convert.ToString(r["LugarDireccion"]);
                            }
                           
                            estacionamiento.sPlaca = Convert.ToString(r["sPlaca"]);
                            estacionamiento.sModelo = Convert.ToString(r["sModelo"]);
                            estacionamiento.sColor = Convert.ToString(r["sColor"]);
                            listaEstacionamientos.Add(estacionamiento);
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {

                }
            }

            return listaEstacionamientos;
        }

        public JsonResult MostrarAsignacionesEstacionamientosVisitantesSinLugar_AJAX()
        {
            try
            {
                List<MEstacionamiento> listaEstacionamientos = ListaEstacionamientosAsignadosVisitantesSinLugar();

                return Json(new { error = false, listaEstacionamientos }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = true, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [NonAction]
        private void AsignarCajonVisitanteBD(int cajon,  int registroCajon)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;

            using (SqlConnection conexionSQL = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexionSQL.Open();
                    using (SqlCommand comando = new SqlCommand("sp_UpdateAsignarCajon", conexionSQL))
                    {
                        comando.CommandType = CommandType.StoredProcedure;

                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@iCajon", Value = cajon });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@uiRegistroCajones", Value = registroCajon });

                        int filas = 0;
                        filas = comando.ExecuteNonQuery();

                        if (filas == 0)
                        {
                            throw new ApplicationException("No se asignó el Cajón al Visitante");
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                finally
                {
                    conexionSQL.Close();
                    conexionSQL.Dispose();
                }
            }
        }

        [NonAction]
        private void AsignarCajonLugarVisitanteBD(int lugar, int registroCajon)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;

            using (SqlConnection conexionSQL = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexionSQL.Open();
                    using (SqlCommand comando = new SqlCommand("sp_UpdateAsignarCajonLugar", conexionSQL))
                    {
                        comando.CommandType = CommandType.StoredProcedure;

                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@iLugar", Value = lugar });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@uiRegistroCajones", Value = registroCajon });

                        int filas = 0;
                        filas = comando.ExecuteNonQuery();

                        if (filas == 0)
                        {
                            throw new ApplicationException("No se asignó el Lugar al Visitante");
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                finally
                {
                    conexionSQL.Close();
                    conexionSQL.Dispose();
                }
            }
        }

        public JsonResult AsignarCajonVisitante_AJAX(MEstacionamiento estacionamiento)
        {
            try
            {
                MLugarDireccion lugarDireccion = ObtenerLugarDireccionBD(estacionamiento.iCajon, estacionamiento.sLugar);
                CambiarEstatusLugaresBD(lugarDireccion.iLugar, lugarDireccion.sLugar);

                AsignarCajonVisitanteBD(estacionamiento.iCajon, estacionamiento.uiRegistroCajones);
                AsignarCajonLugarVisitanteBD(lugarDireccion.uiRegistroLugar, estacionamiento.uiRegistroCajones);
                return Json(new { error = false}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = true, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ValidarFechaHoy_AJAX(string fecha)
        {
            try
            {
                bool validarFecha = false;

                string fechaActual = DateTime.Now.ToString("dd/MM/yyyy");
                DateTime fechaTimer = Convert.ToDateTime(fecha);
                string fechaVisita = fechaTimer.ToString("dd/MM/yyyy");

                if (fechaActual == fechaVisita)
                {
                    validarFecha = true;
                }
                else
                {
                    validarFecha = false;
                }

                return Json(new { error = false, validarFecha }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = true, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Estatus Solicitud


        [NonAction]

        private MEstacionamiento ObtenerSolicitudEstacionamientoBD(int solicitud)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;
            SqlConnection conexionSQL = new SqlConnection(cadenaConexion);

            SqlCommand comando = new SqlCommand(string.Format("Select * from cajonesEstacionamientos where uiRegistroCajones = {0}", solicitud), conexionSQL);
            SqlDataAdapter da = new SqlDataAdapter(comando);

            DataTable dt = new DataTable();
            da.Fill(dt);
            MEstacionamiento estacionamiento = new MEstacionamiento();

            if (dt.Rows.Count == 0)
            {
                throw new ApplicationException(string.Format("La solicitud: {0} ya no se encuentra disponible.", solicitud));
            }
            else
            {
                DataRow r = dt.Rows[0];
                estacionamiento = new MEstacionamiento(r);
                estacionamiento.bActivo = Convert.ToBoolean(r["bActivo"]);
            }

            return estacionamiento;
        }

        [NonAction]
        private List<MEstacionamiento> ListaEstacionamientosAsignadosVisitantesUnico(int registroCajon)
        {
            List<MEstacionamiento> listaEstacionamientos = new List<MEstacionamiento>();
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;

            using (SqlConnection conexionSQL = new SqlConnection(cadenaConexion))
            {
                try
                {
                    using (SqlCommand comando = new SqlCommand("sp_MostrarAsignacionesEstacionamientoVisitantesUnico", conexionSQL))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName= "@uiRegistroCajones", Value = registroCajon });

                        SqlDataAdapter da = new SqlDataAdapter(comando);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        foreach (DataRow r in dt.Rows)
                        {
                            MEstacionamiento estacionamiento = new MEstacionamiento();
                            estacionamiento.uiRegistroCajones = Convert.ToInt32(r["uiRegistroCajones"]);
                            estacionamiento.horaVisita = Convert.ToString(r["horaVisita"]);
                            estacionamiento.horaSalida = Convert.ToString(r["horaSalida"]);
                            estacionamiento.dtFechaAsignacion = Convert.ToDateTime(r["dtFechaAsignacion"]);
                            estacionamiento.dtFechaVisita = Convert.ToDateTime(r["dtFechaVisita"]);
                            estacionamiento.sObservaciones = Convert.ToString(r["sObservaciones"]);
                            estacionamiento.Lugares = new MLugares();
                            estacionamiento.Lugares.sLugar = Convert.ToString(r["sLugar"]);
                            estacionamiento.Usuarios = new MUsuarios();
                            estacionamiento.Usuarios.sNombre = Convert.ToString(r["sNombre"]);
                            estacionamiento.Usuarios.sRoleUsuarios = Convert.ToString(r["sRol"]);
                            estacionamiento.Usuarios.nombreResponsable = Convert.ToString(r["NombreResponsable"]);
                            estacionamiento.LugarDireccion = new MLugarDireccion();
                            estacionamiento.LugarDireccion.sLugar = Convert.ToString(r["LugarDireccion"]);
                            estacionamiento.sPlaca = Convert.ToString(r["sPlaca"]);
                            estacionamiento.sModelo = Convert.ToString(r["sModelo"]);
                            estacionamiento.sColor = Convert.ToString(r["sColor"]);
                            listaEstacionamientos.Add(estacionamiento);
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {

                }
            }

            return listaEstacionamientos;
        }

        public JsonResult ComprobarSolicitud_AJAX(int solicitud)
        {
            try
            {
                string estatus = "";
                List<MEstacionamiento> listaEstacionamiento = new List<MEstacionamiento>();

                MEstacionamiento estacionamiento = ObtenerSolicitudEstacionamientoBD(solicitud);

                if (estacionamiento.iCajon == 0 && estacionamiento.iLugar == 0)
                {
                    estatus = "Tu solicitud está en proceso";
                }

                else if(estacionamiento.iCajon != 0 && estacionamiento.iLugar != 0 && estacionamiento.bActivo == true)
                {
                    estatus = "Solicitud Asignada, verifique su correo para más detalles";

                  listaEstacionamiento =  ListaEstacionamientosAsignadosVisitantesUnico(estacionamiento.uiRegistroCajones);
                }
                else
                {
                    estatus = string.Format("La solicitud: {0} ya no se encuentra activa",solicitud);
                }


                return Json(new {error = false, estatus, listaEstacionamiento }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
              return Json(new { mensaje = ex.Message ,error = true }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Eliminar Solicitud

        [NonAction]

        private MUsuarios ObtenerUsuarioEstacionamientoBD(int registroUser)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;
            SqlConnection conexionSQL = new SqlConnection(cadenaConexion);

            SqlCommand comando = new SqlCommand(string.Format("Select * from usuarios where IdUsuarios = {0}", registroUser), conexionSQL);
            SqlDataAdapter da = new SqlDataAdapter(comando);

            DataTable dt = new DataTable();
            da.Fill(dt);

            DataRow r = dt.Rows[0];
            MUsuarios user = new MUsuarios(r);

            if (r["idUserResponsables"] != DBNull.Value)
            {
                user.idUserResponsables = Convert.ToInt32(r["idUserResponsables"]);
            }
            return user;
        }

        [NonAction]

        private void EliminarUsuariosEstacionamientoBD(int idUser)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;
            
            using (SqlConnection conexionSQL = new SqlConnection(cadenaConexion))
            {
                conexionSQL.Open();
                using (SqlCommand comando = new SqlCommand(string.Format("delete from usuarios where IdUsuarios = {0}", idUser), conexionSQL))
                {
                    try
                    {
                        int filas = 0;
                        filas = comando.ExecuteNonQuery();

                        if (filas == 0)
                        {
                            throw new ApplicationException(string.Format("No se eliminó el usuario con Id: {0}", idUser));
                        }
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                    finally
                    {

                        conexionSQL.Close();
                        conexionSQL.Dispose();
                    }
                }
            }
        }

        [NonAction]

        private void EliminarSolicitudCajonEstacionamientoBD(int registroCajon)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;

            using (SqlConnection conexionSQL = new SqlConnection(cadenaConexion))
            {
                conexionSQL.Open();

                using (SqlCommand comando = new SqlCommand(string.Format("delete from cajonesEstacionamientos where uiRegistroCajones = {0}",registroCajon),conexionSQL))
                {
                    try
                    {
                        int filas = 0;
                        filas = comando.ExecuteNonQuery();

                        if (filas == 0)
                        {
                            throw new ApplicationException(string.Format("No se eliminó el registro de estacionamiento con el Id: ", registroCajon));
                        }
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }

                    finally
                    {
                        conexionSQL.Close();
                        conexionSQL.Dispose();
                    }
                }
            }
            
        }

        [NonAction]
        private void InsertLogEliminaBD(string sUser, string sPlaca, DateTime dtFechaSolicita, DateTime dtFechaVisita)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;

            using (SqlConnection conexionSQL = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexionSQL.Open();
                    using (SqlCommand comando = new SqlCommand("sp_InsertLogElimina", conexionSQL))
                    {
                        comando.CommandType = CommandType.StoredProcedure;

                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.VarChar, ParameterName = "@sUser", Value = sUser });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.VarChar, ParameterName = "@sPlaca", Value = sPlaca });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Date, ParameterName = "@dtFechaSolicita", Value = dtFechaSolicita });
                        comando.Parameters.Add(new SqlParameter() { SqlDbType = SqlDbType.Date, ParameterName = "@dtFechaVisita", Value = dtFechaVisita });

                        int filas = 0;
                        filas = comando.ExecuteNonQuery();

                        if (filas == 0)
                        {
                            throw new ApplicationException("No insertó el log para elimiar");
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                finally
                {
                    conexionSQL.Close();
                    conexionSQL.Dispose();
                }
            }
        }


        public JsonResult EliminarSolicitud_AJAX(int solicitud)
        {
            try
            {
                MEstacionamiento estacionamiento = ObtenerSolicitudEstacionamientoBD(solicitud);

                MUsuarios usuario = ObtenerUsuarioEstacionamientoBD(estacionamiento.iUsuario);
                EliminarUsuariosEstacionamientoBD(usuario.IdUsuarios);

                MUsuarios usuarioResponsable = ObtenerUsuarioEstacionamientoBD(usuario.idUserResponsables);
                EliminarUsuariosEstacionamientoBD(usuarioResponsable.IdUsuarios);

                InsertLogEliminaBD(usuario.sNombre, estacionamiento.sPlaca, estacionamiento.dtFechaAsignacion, estacionamiento.dtFechaVisita);

                EliminarSolicitudCajonEstacionamientoBD(estacionamiento.uiRegistroCajones);

                return Json(new {error = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {mensaje = ex.Message, error = true }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Vigilates

        public ActionResult CajonesVisitantes()
        {
            List<MEstacionamiento> listaEstacionamientos = ListaEstacionamientosAsignadosVisitantes();

            return View(listaEstacionamientos);
        }

        #endregion

        public ActionResult ReportesEstacionamientos()
        {
            return View();
        }

        #region Cambio de cajón

        [NonAction]

        private MLugarDireccion ObtenerCajonActualBD(int registroCajon)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;
            SqlConnection conexionSQL = new SqlConnection(cadenaConexion);

            SqlCommand comando = new SqlCommand(string.Format("Select * from catNumeroLugares where uiRegistroLugar = {0}", registroCajon),conexionSQL);
            SqlDataAdapter da = new SqlDataAdapter(comando);

            DataTable dt = new DataTable();
            da.Fill(dt);

            DataRow r = dt.Rows[0];
            MLugarDireccion lugar = new MLugarDireccion(r);

            return lugar;
        }

        [NonAction]

        private void InactivarLugarBD(int registroLugar)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["SQLESTACIONA"].ConnectionString;
            using (SqlConnection conexionSQL = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexionSQL.Open();

                    using (SqlCommand comando = new SqlCommand("sp_LiberarLugarActivo", conexionSQL))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter() {SqlDbType = SqlDbType.Int, ParameterName= "@uiRegistroLugar", Value = registroLugar });

                        int filas = 0;

                        filas = comando.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    conexionSQL.Close();
                    throw ex;
                }

                finally {
                    conexionSQL.Close();
                    conexionSQL.Dispose();
                }
            }
        }

        public JsonResult CambiarCajonVisitante_AJAX(MEstacionamiento estacionamiento)
        {
            try
            {

                #region Liberar Cajón

                MEstacionamiento cajonActual = ObtenerEstacionamientoLiberaBD(estacionamiento.uiRegistroCajones);

                MLugarDireccion lugarActual = ObtenerCajonActualBD(cajonActual.iLugar);

                InactivarLugarBD(lugarActual.uiRegistroLugar);

                #endregion

                #region Asignar cajón

                MLugarDireccion lugarDireccion = ObtenerLugarDireccionBD(estacionamiento.iCajon, estacionamiento.sLugar);

                CambiarEstatusLugaresBD(lugarDireccion.iLugar, lugarDireccion.sLugar);

                AsignarCajonVisitanteBD(estacionamiento.iCajon, estacionamiento.uiRegistroCajones);
                AsignarCajonLugarVisitanteBD(lugarDireccion.uiRegistroLugar, estacionamiento.uiRegistroCajones);

                #endregion

                return Json(new { error = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = true, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        public ActionResult AsignacionLugares()
        {
            return View();
        }

    }
}