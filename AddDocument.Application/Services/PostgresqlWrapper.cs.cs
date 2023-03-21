using Application.Interfaces.PostgresqlWrapper;
using Confluent.Kafka;
using Domain.Models;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.PostgresqlWrapper
{
    public class PostgresqlWrapper : IPostgresqlWrapper
    {
        public readonly IConfiguration _config;
        public PostgresqlWrapper(IConfiguration config)
        {
            _config = config;
        }
        public void AddDocument(AddDocument addDocument)
        {
            NpgsqlConnection con = new(_config.GetConnectionString("DefaultConnection"));
            try
            {
                con.Open();
                using var cmd = new NpgsqlCommand("adddocument_insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("correlationid", addDocument.CorrelationId);
                cmd.Parameters.AddWithValue("tempbloburl", addDocument.TempBlobUrl);
                cmd.Parameters.AddWithValue("filename", addDocument.FileName);
                cmd.Parameters.AddWithValue("flag", addDocument.Flag);
                cmd.Prepare();
                var res = Convert.ToString(cmd.ExecuteScalar());
                cmd.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
}
