using Application.Interfaces.PostgresqlWrapper;
using Confluent.Kafka;
using Domain.Models;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Application.Services.PostgresqlWrapper
{
    public class PostgresqlWrapper : IPostgresqlWrapper
    {
        public readonly IConfiguration _config;
        private readonly ApplicationDBContext _context;
        public PostgresqlWrapper(IConfiguration config, ApplicationDBContext context)
        {
            _context = context;
            _config = config;
        }
        public bool AddDocument(AddDocument addDocument)
        {
            //var res = _context.adddocument.FromSqlInterpolated($"Exec adddocument_insert @correlationid = {addDocument.CorrelationId} ,@tempbloburl = {addDocument.TempBlobUrl} ,@filename = {addDocument.FileName} ,@flag = {addDocument.Flag}");
            NpgsqlConnection con = new(_config.GetConnectionString("DefaultConnection"));
            try
            {
                con.Open();
                using var cmd = new NpgsqlCommand("adddocument_insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("correlationid", DbType.Guid).Value = addDocument.CorrelationId;
                cmd.Parameters.AddWithValue("tempbloburl", DbType.String).Value = addDocument.TempBlobUrl;
                cmd.Parameters.AddWithValue("filename", DbType.String).Value = addDocument.FileName;
                cmd.Parameters.AddWithValue("flag", DbType.String).Value = addDocument.Flag;
                cmd.Prepare();
                var affectedCount = cmd.ExecuteNonQuery();
                bool saved = affectedCount == 1;
                cmd.Dispose();
                return saved;

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
}
