namespace WebApplication1.Models;

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

public class Book
{
    private readonly string _connectionString;

    public Book(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("BookDatabase")
            ?? throw new InvalidOperationException("Connection string 'BookDatabase' not found.");
    }

    public Book()
        : this(new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .Build())
    {
    }

    public int Id { get; set; }
    public string Bookname { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;


    public DataTable validateasTable(string bookname, string author, string isbn)
    {
        DataTable dt = new DataTable();
        using var connection = new NpgsqlConnection(_connectionString);
        using var command = new NpgsqlCommand("SELECT * FROM books", connection);

        command.CommandType = CommandType.Text;
        command.CommandTimeout = 0;
        command.Parameters.Clear();

        using var adapter = new NpgsqlDataAdapter(command);
        connection.Open();
        adapter.Fill(dt);
        return dt;
    }

    public DataTable validateasTableBySp(string bookname, string author, string isbn )
    {
        DataTable dt = new DataTable();
        using var connection = new NpgsqlConnection(_connectionString);
        using var command = new NpgsqlCommand("public.getusers", connection);

        command.CommandType = CommandType.StoredProcedure;
        command.CommandTimeout = 0;
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@Bookname", bookname);
        command.Parameters.AddWithValue("@Author", author);
        command.Parameters.AddWithValue("@Isbn", isbn);

        using var adapter = new NpgsqlDataAdapter(command);
        connection.Open();
        adapter.Fill(dt);
        return dt;
    }

    public List<Book> validateasList(string bookname, string author, string isbn)
    {
        List<Book> bookS = new List<Book>();

        using var connection = new NpgsqlConnection(_connectionString);
        using var command = new NpgsqlCommand("SELECT * FROM books WHERE bookname = @Bookname AND author = @Author AND isbn = @Isbn", connection);

        command.CommandType = CommandType.Text;
        command.CommandTimeout = 0;
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@Bookname", bookname);
        command.Parameters.AddWithValue("@Author", author);
        command.Parameters.AddWithValue("@Isbn", isbn);

        connection.Open();
        using var reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                Book book = new Book
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Bookname = reader["bookname"].ToString() ?? string.Empty,
                    Author = reader["author"].ToString() ?? string.Empty,
                    Isbn = reader["isbn"].ToString() ?? string.Empty
                };
                bookS.Add(book);
            }
        }
        else
        {
            Console.WriteLine("No rows found.");
        }

        return bookS;
    }

    public List<Book> getAllBooks()
    {
        List<Book> books = new List<Book>();

        using var connection = new NpgsqlConnection(_connectionString);
        using var command = new NpgsqlCommand("SELECT * FROM books", connection);

        command.CommandType = CommandType.Text;
        command.CommandTimeout = 0;

        connection.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            Book newbook = new Book
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Bookname = reader["bookname"].ToString() ?? string.Empty,
                Author = reader["author"].ToString() ?? string.Empty,
                Isbn = reader["isbn"].ToString() ?? string.Empty
            };
            books.Add(newbook);
        }

        return books;
    }

    public bool AddBook(string bookname, string author, string isbn)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        using var command = new NpgsqlCommand(
            "INSERT INTO books (bookname, author, isbn) VALUES (@Bookname, @Author, @Isbn)", connection);

        command.CommandType = CommandType.Text;
        command.CommandTimeout = 0;
        command.Parameters.AddWithValue("@Bookname", bookname);
        command.Parameters.AddWithValue("@Author", author);
        command.Parameters.AddWithValue("@Isbn", isbn);

        connection.Open();
        int rowsAffected = command.ExecuteNonQuery();
        return rowsAffected > 0;
    }
}

