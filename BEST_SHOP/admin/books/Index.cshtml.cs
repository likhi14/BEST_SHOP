using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BEST_SHOP.admin.books
{
    
    public class IndexModel : PageModel
    {
        public List<Bookinfo> listBooks = new List<Bookinfo>();
        public string search = "";
        public void OnGet()
        {
            search = Request.Query["search"];
            if (search == null) search = "";

            try
            {
                string connectionString = "Data Source=mssqluk22.prosql.net;Initial Catalog=cmsapps;Persist Security Info=True;User ID=emp;Password=inDia@143";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string Sql = "SELECT* from Books";
                    if(search.Length > 0)
                    {
                        Sql += "where title like @search or authors like @search";
                    }
                    Sql += "order by id DESC";

                    
                    using (SqlCommand command = new SqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@search", "%" + search + "%");
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                           while (reader.Read())
                            {
                                Bookinfo bookInfo = new Bookinfo();
                                bookInfo.ID = reader.GetInt32(0);
                                bookInfo.Title = reader.GetString(1);
                                bookInfo.Authors = reader.GetString(2);
                                bookInfo.Isbn = reader.GetString(3);
                                bookInfo.NumPages = reader.GetInt32(4);
                                bookInfo.Price = reader.GetDecimal(5);
                                bookInfo.Category = reader.GetString(6);
                                bookInfo.Description = reader.GetString(7);
                                bookInfo.ImageFileName = reader.GetString(8);
                                bookInfo.CreatedAt = reader.GetDateTime(9).ToString("MM/dd/yyyy");

                                listBooks.Add(bookInfo);
                            }
                        }
                    }
                }

            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }   
        }

    }
    public class Bookinfo
    {
        public int ID { get; set; }
        public string Title { get; set; } = "";
        public string Authors { get; set; } = "";
        public string Isbn { get; set; } = "";
        public int NumPages { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; } = "";
        public string Description { get; set; } = "";
        public string ImageFileName { get; set; } = "";
        public string CreatedAt { get; set; } = "";
    }
}
