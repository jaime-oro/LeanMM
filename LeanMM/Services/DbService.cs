using LeanMM.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;

namespace LeanMM.Services
{
    public partial class DbService
    {
        private readonly string? _connectionString;
        public DbService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<List<T>> QueryAsync<T>(string query, object? parameters = null)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var data = await db.QueryAsync<T>(query, parameters);
            return data.ToList();
        }

        public async Task<T?> GetFirstOrDefaultAsync<T>(string query, object? parameters = null)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return await db.QueryFirstOrDefaultAsync<T>(query, parameters);
        }

        public async Task<int> ExecuteAsync(string query, object? parameters = null)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return await db.ExecuteAsync(query, parameters);
        }

        public async Task<T?> ExecuteScalarAsync<T>(string query, object? parameters = null)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return await db.ExecuteScalarAsync<T>(query, parameters);
        }

    }
}
