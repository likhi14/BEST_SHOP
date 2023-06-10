using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Data.SqlClient;

namespace BEST_SHOP.Pages
{
    public class contactModel : PageModel
    {
        public void OnGet()
        {
        }
        [BindProperty]
        [Required(ErrorMessage = "the First Name is required")]
        [Display(Name = "First Name*")]
        public string FirstName { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "the Last Name is required")]
        [Display(Name = "Last Name*")]
        public string LastName { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "the Email is required")]
        [EmailAddress]
        [Display(Name = "Email*")]
        public string Email { get; set; } = "";

        [BindProperty]
        public string Phone { get; set; } = "";

        [BindProperty, Required]
        [Display(Name = "Subject*")]
        public string Subject { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "the Message is required")]
        [MinLength(5, ErrorMessage = "the message should be aleast 5 characters")]
        [MaxLength(1024, ErrorMessage = "the message should be less than 1024 characters")]
        [Display(Name = "Message*")]
       
        public string Message { get; set; } = "";
        public List <SelectListItem> SubjectList { get; } = new List<SelectListItem>

        { 
            new SelectListItem { Value ="order status", Text = "order status"},
            new SelectListItem { Value ="Refund Request", Text = "Refund Request"},
            new SelectListItem { Value ="Job Application", Text = "Job Application"},
            new SelectListItem { Value ="Other", Text = "Other"},

        };
        

        public string SuccessMessage { get; set; } = "";
        public string ErrorMessage { get; set; } = "";

        public void OnPost()
        {
            
            //check if any required field is empty
            if (!ModelState.IsValid)
            {
                //error
                ErrorMessage = "please fill all required fields";
                return;
            }
            if (Phone == null) Phone = "";
            //add this message to the database
            try
            {
                string connectionString = "Data Source=mssqluk22.prosql.net;Initial Catalog=cmsapps;Persist Security Info=True;User ID=emp;Password=inDia@143";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string Sql = "INSERT INTO Messages_L" + "(firstname, lastname, email, phone, subject, message) VALUES " + "(@firstname, @lastname, @email, @phone, @subject, @message);";
                    using(SqlCommand command= new SqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@firstname", FirstName);
                        command.Parameters.AddWithValue("@lastname", LastName);
                        command.Parameters.AddWithValue("@email", Email);
                        command.Parameters.AddWithValue("@phone", Phone);
                        command.Parameters.AddWithValue("@subject", Subject);
                        command.Parameters.AddWithValue("@message", Message);

                        command.ExecuteNonQuery(); 
                    }
                }


            }
            catch(Exception ex) 
            {
                //Error
                ErrorMessage = ex.Message;
                return;
            }
            
            //send confirmation emal to the client
            SuccessMessage = "your message has been received correctly";
            FirstName = "";
            LastName = "";
            Email = "";
            Phone = "";
            Subject = "";
            Message = "";

            ModelState.Clear();

        }
    }
}
