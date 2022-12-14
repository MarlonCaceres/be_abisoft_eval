using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Text.Json;
using Newtonsoft.Json;
using be_abisoft.Models;

namespace be_abisoft.Utils
{
    public static class Utils
    {
        public static XmlDocument crearParametrosXml(string header, Dictionary<string, string> parameters)
        {
            XmlDocument xmlParametros = new XmlDocument();
            xmlParametros.LoadXml("<" + header + " />");

            foreach (KeyValuePair<string, string> kvp in parameters)
            {
                xmlParametros.DocumentElement.SetAttribute(kvp.Key, kvp.Value);
            }

            return xmlParametros;
        }

        public static FormResponseModel crearFormResponseModel(DataSet ds_respuesta)
        {
            FormResponseModel response = new FormResponseModel();
            if (ds_respuesta != null && ds_respuesta.Tables.Count > 0)
            {
                response.codError = ds_respuesta.Tables[0].Rows[0]["Error"].ToString();
                response.msgError = ds_respuesta.Tables[0].Rows[0]["Mensaje"].ToString();

                if (response.codError == "0")
                {
                    response.success = true;
                    foreach (DataTable table in ds_respuesta.Tables)
                        if (ds_respuesta.Tables.IndexOf(table) != 0)
                            response.root.Add(System.Text.Json.JsonSerializer.Deserialize<List<Object>>(JsonConvert.SerializeObject(table)));
                }
                else
                {
                    response.success = false;
                }
            }
            else
            {
                response.codError = "-1";
                response.msgError = "No hubo respuesta con el servidor";
                response.success = false;
            }

            return response;
        }

        public static Dictionary<string, string> creaDiccionarioLlamada(string sp_Procedimiento, XmlDocument parametros)
        {
            Dictionary<string, string> ld_parametrosP = new Dictionary<string, string>();
            ld_parametrosP.Add("sp_procedimiento", sp_Procedimiento);
            ld_parametrosP.Add("in_parametro", Convert.ToBase64String(Encoding.Unicode.GetBytes(parametros.OuterXml)));

            return ld_parametrosP;
        }
    }
}

