using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebAppMontGroup.Models
{
    public class Respuesta_Json
    {


        //public List<Dictionary<string, object>> data { get; set; }

        public List<Dictionary<string, object>> DatTableToDictionary(DataTable dt) { 
        
            List<Dictionary<string, object>> filas = new List<Dictionary<string, object>>();
            Dictionary<string, object> fila;
            foreach (DataRow row in dt.Rows)
            {
                fila = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns) 
                {
                    fila.Add(col.ColumnName, row[col]);
                }
                filas.Add(fila);
            }

            return filas;
        }
    }
}