using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebAppMontGroup.Models
{
    public class ConeccionMysql
    {
        private string _strCon;
        private MySqlConnection _con;

        public void conectar()
        {
            try
            {
                _con = new MySqlConnection(_strCon);
                _con.Open();
            }
            catch (Exception ex)
            {
                //Funciones.escribirMensaje(ex.Message);
            }
        }

        public void desconectar()
        {
            try
            {
                _con.Close();
                _con.Dispose();
            }
            catch (Exception ex)
            {
                //Funciones.escribirMensaje(ex.Message);
            }
        }

        public ConeccionMysql(string keyAppSettingBd)
        {

            _strCon = ConfigurationManager.ConnectionStrings[keyAppSettingBd].ConnectionString;
            //_strCon = "Data Source='" + SERVIDOR + "';Port='" + PUERTO + "'; User Id='" + USUARIO + "';password='" + PASSWORD + "';Database='" + DATABASE + "';Convert Zero Datetime=True";
        }

        public MySqlConnection retConeccion()
        {
            return _con;
        }

    }
}