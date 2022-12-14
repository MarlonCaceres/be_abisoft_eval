using System;
using be_abisoft.Models;
using be_abisoft.Utils;
using Npgsql;
using System.Data;

namespace be_abisoft.Services
{
    public static class UserService
    {
        public static async Task<FormResponseModel> Ins_upd_user(IConfiguration ao_config, RequestUser_CU request)
        {
            //CONEXION DB
            NpgsqlCommand command = new NpgsqlCommand();
            NpgsqlConnection conexion = new NpgsqlConnection(ConexionCore.CadenaBDD(ao_config));
            NpgsqlTransaction? transaction = null;

            try
            {
                DataSet results = new DataSet();

                //CONEXION DB
                conexion.Open();
                transaction = conexion.BeginTransaction();
                command.Connection = conexion;
                command.CommandText = Utils.ProcedureBase.schemaPublic+ Utils.ProcedureBase.SP_INS_UPD_USER;
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("ID".ToLower(), request.Id);
                command.Parameters.AddWithValue("NAME".ToLower(), request.Name.ToString());
                command.Parameters.AddWithValue("AGE".ToLower(), request.Age.ToString());
                command.Parameters.AddWithValue("BIRTHDATE".ToLower(), request.Birthdate.ToString());
                command.Parameters.AddWithValue("INSCRIPTIONDATE".ToLower(), request.InscriptionDate.ToString());
                command.Parameters.AddWithValue("PRICE".ToLower(), request.Price);
                
                command.Transaction = transaction;

                //ENVIO PETICIÓN
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
                DataSet ds_response = new DataSet();
                adapter.SelectCommand = command;
                adapter.Fill(ds_response);

                foreach (DataRow item in ds_response.Tables[0].Rows)
                {
                    DataTable dt = new DataTable();
                    command = new NpgsqlCommand("FETCH ALL IN " + "\"" + item[0].ToString() + "\"");
                    command.Connection = conexion;
                    NpgsqlDataAdapter datmp = new NpgsqlDataAdapter(command);
                    datmp.Fill(dt);
                    results.Tables.Add(dt);
                }

                transaction.Commit();

                adapter.Dispose();
                command.Dispose();

                FormResponseModel response = Utils.Utils.crearFormResponseModel(results);
                return response;
            }
            catch (Exception error)
            {
                FormResponseModel response = new FormResponseModel();
                response.msgError = error.ToString();

                transaction.Rollback();
                return response;
            }
            finally
            {
                conexion.Close();
                command.Dispose();
            }
        }

        public static async Task<FormResponseModel> Sel_user(IConfiguration ao_config, RequestUser request)
        {
            //CONEXION DB
            NpgsqlCommand command = new NpgsqlCommand();
            NpgsqlConnection conexion = new NpgsqlConnection(ConexionCore.CadenaBDD(ao_config));
            NpgsqlTransaction? transaction = null;

            try
            {
                DataSet results = new DataSet();

                //CONEXION DB
                conexion.Open();
                transaction = conexion.BeginTransaction();
                command.Connection = conexion;
                command.CommandText = Utils.ProcedureBase.schemaPublic + Utils.ProcedureBase.SP_SEL_USER;
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("ID".ToLower(), request.Id);
                
                command.Transaction = transaction;

                //ENVIO PETICIÓN
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
                DataSet ds_response = new DataSet();
                adapter.SelectCommand = command;
                adapter.Fill(ds_response);

                foreach (DataRow item in ds_response.Tables[0].Rows)
                {
                    DataTable dt = new DataTable();
                    command = new NpgsqlCommand("FETCH ALL IN " + "\"" + item[0].ToString() + "\"");
                    command.Connection = conexion;
                    NpgsqlDataAdapter datmp = new NpgsqlDataAdapter(command);
                    datmp.Fill(dt);
                    results.Tables.Add(dt);
                }

                transaction.Commit();

                adapter.Dispose();
                command.Dispose();

                FormResponseModel response = Utils.Utils.crearFormResponseModel(results);
                return response;
            }
            catch (Exception error)
            {
                FormResponseModel response = new FormResponseModel();
                response.msgError = error.ToString();

                transaction.Rollback();
                return response;
            }
            finally
            {
                conexion.Close();
                command.Dispose();
            }
        }
    }
}

