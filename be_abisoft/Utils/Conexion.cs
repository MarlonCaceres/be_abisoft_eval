using System;
using System.Data;
using Npgsql;

namespace be_abisoft.Utils
{
    public class Conexion
    {
        #region "variables privadas"
        private string _cadenaconexion = "";
        private Exception _Exception;
        private NpgsqlCommand command;
        private NpgsqlConnection conexion = null;

        #endregion

        #region "Propiedades"

        public Exception Exception
        {
            get { return _Exception; }
            set { _Exception = value; }
        }
        public string cadenaconexion
        {
            get { return _cadenaconexion; }
            set { _cadenaconexion = value; }
        }

        #endregion

        #region "metodos"

        private void CloseConexion()
        {
            try
            {
                if (conexion != null && conexion.State != ConnectionState.Closed)
                {
                    conexion.Close();
                    conexion.Dispose();
                }
            }
            catch (Exception ex)
            {
                Exception = ex;

            }
        }

        public DataSet GetDs(string commandtext, CommandType commandtype, string[] paramtername,
                                              string[] parametervalue)
        {
            NpgsqlTransaction transaction = null;
            _Exception = null;
            conexion = null;
            command = new NpgsqlCommand();
            int band_excepcion = 0;
            DataSet resultado = new DataSet();
            try
            {
                conexion = new NpgsqlConnection(_cadenaconexion);
                conexion.Open();
                transaction = conexion.BeginTransaction();
                command = new NpgsqlCommand();
                command.Connection = conexion;
                command.CommandType = commandtype;
                command.CommandText = commandtext;
                command.Transaction = transaction;
                if (paramtername != null && paramtername.Length > 0)
                {
                    for (int i = 0; i < paramtername.Length; i++)
                    {
                        command.Parameters.AddWithValue(paramtername[i], NpgsqlTypes.NpgsqlDbType.Xml, parametervalue[i].ToString());
                    }
                }
                DataSet ds = new DataSet();
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
                adapter.SelectCommand = command;
                adapter.Fill(ds);
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    DataTable dt = new DataTable();
                    command = new NpgsqlCommand("FETCH ALL IN " + "\"" + item[0].ToString() + "\"");
                    command.Connection = conexion;
                    NpgsqlDataAdapter datmp = new NpgsqlDataAdapter(command);
                    datmp.Fill(dt);
                    resultado.Tables.Add(dt);
                }

                transaction.Commit();

                adapter.Dispose();
                command.Dispose();
                CloseConexion();

            }
            catch (Exception ex)
            {
                band_excepcion = 1;
                Exception = ex;
                try
                {
                    transaction.Rollback();
                }
                catch (Exception)
                {
                }
                CloseConexion();
                try
                {
                    if (command != null) { command.Dispose(); }
                }
                catch (Exception)
                { }
            }
            finally
            {
                if (band_excepcion == 1)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Error", typeof(string));
                    dt.Columns.Add("Mensaje", typeof(string));
                    DataRow row = dt.NewRow();
                    row["Error"] = "9999";
                    row["Mensaje"] = Exception.Message;
                    dt.Rows.Add(row);
                    resultado.Tables.Add(dt);
                }

            }

            return resultado;
        }
        public DataTable GetDaTable(string commandtext, CommandType commandtype, string[] paramtername,
                                            object[] parametervalue)
        {
            NpgsqlTransaction transaction = null;
            _Exception = null;
            conexion = null;
            command = new NpgsqlCommand();
            DataTable resultado = new DataTable();
            try
            {
                conexion = new NpgsqlConnection(_cadenaconexion);
                conexion.Open();
                transaction = conexion.BeginTransaction();
                command = new NpgsqlCommand();
                command.Connection = conexion;
                command.CommandType = commandtype;
                command.CommandText = commandtext;
                command.Transaction = transaction;
                if (paramtername != null && paramtername.Length > 0)
                {
                    for (int i = 0; i < paramtername.Length; i++)
                    {
                        command.Parameters.AddWithValue(paramtername[i], NpgsqlTypes.NpgsqlDbType.Xml, parametervalue[i].ToString());
                    }
                }
                DataSet ds = new DataSet();
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
                adapter.SelectCommand = command;
                adapter.Fill(ds);
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    DataTable dt = new DataTable();
                    command = new NpgsqlCommand("FETCH ALL IN " + "\"" + item[0].ToString() + "\"");
                    command.Connection = conexion;
                    NpgsqlDataAdapter datmp = new NpgsqlDataAdapter(command);
                    datmp.Fill(dt);
                    resultado = dt;
                }

                transaction.Commit();

                adapter.Dispose();
                command.Dispose();
                CloseConexion();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Exception = ex;
                CloseConexion();
                try
                {
                    if (command != null) { command.Dispose(); }
                }
                catch (Exception)
                { }
            }
            return resultado;
        }

        public void Ejecuccion(string commandtext, CommandType commandtype, string[] paramtername,
                                              object[] parametervalue)
        {
            _Exception = null;
            conexion = null;
            command = new NpgsqlCommand();
            DataTable resultado = new DataTable();
            try
            {
                command.CommandText = commandtext;
                command.CommandType = commandtype;
                if (paramtername != null && paramtername.Length > 0)
                {
                    for (int i = 0; i < paramtername.Length; i++)
                    { command.Parameters.AddWithValue(paramtername[i], parametervalue[i]); }
                }
                try
                {
                    conexion = new NpgsqlConnection(_cadenaconexion);
                    conexion.Open();
                    command.Connection = conexion;
                    command.CommandTimeout = 999999999;
                    var adapter = new NpgsqlDataAdapter(command);

                    adapter.Fill(resultado);
                    CloseConexion();
                }
                catch (Exception ex)
                {
                    Exception = ex;
                    CloseConexion();
                    try
                    {
                        if (command != null) { command.Dispose(); }
                    }
                    catch (Exception)
                    { }
                }
            }

            catch (Exception ex)
            {
                Exception = ex;
            }
        }

        #endregion
    }
}

