using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BEST_SHOP.admin.messages
{
    public class DetailsModel : PageModel
    {
        public MessageInfo messageinfo = new MessageInfo();
        public void OnGet()
        {
            string requestID = Request.Query["id"];
            try
            {
                string connectionstring = "Data Source=mssqluk22.prosql.net;Initial Catalog=cmsapps;Persist Security Info=True;User ID=emp;Password=inDia@143";
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    connection.Open();
                    string Sql = "SELECT * FROM Messages_L WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", requestID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                messageinfo.ID = reader.GetInt32(0);
                                messageinfo.FirstName = reader.GetString(1);
                                messageinfo.LastName = reader.GetString(2); 
                                messageinfo.Email = reader.GetString(3);
                                messageinfo.Phone = reader.GetString(4);
                                messageinfo.Subject = reader.GetString(5);
                                messageinfo.Message = reader.GetString(6);
                                messageinfo.CreatedAt = reader.GetDateTime(7).ToString("MM/dd/yyyy");

                                



                            }
                            else
                            {
                                Response.Redirect("/admin/messages/index");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Response.Redirect("/admin/message/index");
            }
        }
    }
}
