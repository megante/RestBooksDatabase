﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BookRestSimpleDatabase.model;
using Microsoft.AspNetCore.Mvc;

namespace BookRestSimpleDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private static readonly string ConnectionString = Controllers.ConnectionString.GetConnectionString();

        // GET: api/Books
        [HttpGet]
        public IEnumerable<Book> Get()
        {
            const string selectString = "select * from book order by id";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectString, databaseConnection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        List<Book> bookList = new List<Book>();
                        while (reader.Read())
                        {
                            Book book = ReadBook(reader);
                            bookList.Add(book);
                        }
                        return bookList;
                    }
                }
            }
        }

        private Book ReadBook(IDataRecord reader)
        {
            int id = reader.GetInt32(0);
            string title = reader.IsDBNull(1) ? null : reader.GetString(1);
            string author = reader.IsDBNull(2) ? null : reader.GetString(2);
            string publisher = reader.IsDBNull(3) ? null : reader.GetString(3);
            decimal price = reader.GetDecimal(4);
            Book book = new Book
            {
                Id = id,
                Title = title,
                Author = author,
                Publisher = publisher,
                Price = price
            };
            return book;
        }

        // GET: api/Books/5
        [Route("{id}")]
        public Book Get(int id)
        {
            const string selectString = "select * from book where id=@id";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectString, databaseConnection))
                {
                    selectCommand.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (!reader.HasRows) { return null; }
                        reader.Read(); // advance cursor to first row
                        return ReadBook(reader);
                    }
                }
            }
        }

        // POST: api/Books
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}