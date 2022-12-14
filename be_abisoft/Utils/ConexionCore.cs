using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Data;

namespace be_abisoft.Utils
{
    public static class ConexionCore
    {
        public static string CadenaBDD(IConfiguration _config)
        {
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder();
            builder.Host = _config.GetSection("Conexion:Host").Value.ToString();
            builder.Database = _config.GetSection("Conexion:DataSource").Value.ToString();
            try
            {
                builder.Port = Convert.ToInt32(_config.GetSection("Conexion:Puerto").Value.ToString());
            }
            catch (Exception)
            {
                builder.Port = 5432;
            }

            builder.Username = _config.GetSection("Conexion:Usuario").Value.ToString();
            builder.PersistSecurityInfo = true;
            builder.Password = _config.GetSection("Conexion:Clave").Value.ToString();

            int li_timeOut = 250;
            try
            {
                li_timeOut = Convert.ToInt32(_config.GetSection("Conexion:TimeOut").Value.ToString());
            }
            catch
            {
                li_timeOut = 250;
            }

            builder.Timeout = li_timeOut;
            //Console.WriteLine(builder.ConnectionString);

            return builder.ConnectionString;
        }
    }
}

