namespace WebApplication1.Models;

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

public class BaseMember
{
    private readonly string _connectionString;

    public BaseMember(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException("Connection string 'Database' not found.");
    }

    public BaseMember()
        : this(new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .Build())
    {
    }

    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public DataTable validateasTable(string username, string password)
    {
        DataTable dt = new DataTable();
        using var connection = new NpgsqlConnection(_connectionString);
        using var command = new NpgsqlCommand("SELECT * FROM Users", connection);

        command.CommandType = CommandType.Text;
        command.CommandTimeout = 0;
        command.Parameters.Clear();

        using var adapter = new NpgsqlDataAdapter(command);
        connection.Open();
        adapter.Fill(dt);
        return dt;
    }

    public DataTable validateasTableBySp(string username, string password)
    {
        DataTable dt = new DataTable();
        using var connection = new NpgsqlConnection(_connectionString);
        using var command = new NpgsqlCommand("public.getusers", connection);

        command.CommandType = CommandType.StoredProcedure;
        command.CommandTimeout = 0;
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@Username", username);
        command.Parameters.AddWithValue("@PasswordHash", password);

        using var adapter = new NpgsqlDataAdapter(command);
        connection.Open();
        adapter.Fill(dt);
        return dt;
    }

    public List<BaseMember> validateasList(string username, string password)
    {
        List<BaseMember> baseMembers = new List<BaseMember>();

        using var connection = new NpgsqlConnection(_connectionString);
        using var command = new NpgsqlCommand("SELECT * FROM users", connection);

        command.CommandType = CommandType.Text;
        command.CommandTimeout = 0;
        command.Parameters.Clear();

        connection.Open();
        using var reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                BaseMember baseMember = new BaseMember
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Username = reader["Username"].ToString() ?? string.Empty,
                    Password = reader["PasswordHash"].ToString() ?? string.Empty
                };
                baseMembers.Add(baseMember);
            }
        }
        else
        {
            Console.WriteLine("No rows found.");
        }

        return baseMembers;
    }

    public List<BaseMember> getAllUsers()
    {
        List<BaseMember> users = new List<BaseMember>();

        using var connection = new NpgsqlConnection(_connectionString);
        using var command = new NpgsqlCommand("SELECT * FROM users", connection);

        command.CommandType = CommandType.Text;
        command.CommandTimeout = 0;

        connection.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            BaseMember member = new BaseMember
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Username = reader["Username"].ToString() ?? string.Empty,
                Password = reader["PasswordHash"].ToString() ?? string.Empty
            };
            users.Add(member);
        }

        return users;
    }

    public bool registerUser(string username, string password)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        using var command = new NpgsqlCommand(
            "INSERT INTO users (username, passwordhash) VALUES (@Username, @PasswordHash)", connection);

        command.CommandType = CommandType.Text;
        command.CommandTimeout = 0;
        command.Parameters.AddWithValue("@Username", username);
        command.Parameters.AddWithValue("@PasswordHash", password);

        connection.Open();
        int rowsAffected = command.ExecuteNonQuery();
        return rowsAffected > 0;
    }

    public BaseMember? getUserById(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        using var command = new NpgsqlCommand("SELECT * FROM users WHERE id = @Id", connection);

        command.CommandType = CommandType.Text;
        command.CommandTimeout = 0;
        command.Parameters.AddWithValue("@Id", id);

        connection.Open();
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new BaseMember
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Username = reader["Username"].ToString() ?? string.Empty,
                Password = reader["PasswordHash"].ToString() ?? string.Empty
            };
        }

        return null;
    }
}
