using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MystoreProj.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            if(clientInfo.name.Length  == 0 || clientInfo.email.Length == 0 ||
                clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
            {
                errorMessage = "Required field.";
                return;
            }

            //save data into db
            try
            {
                String connectionString = "Data Source=LAPTOP-RLT7QM6G\\SQLEXPRESS;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "Insert into clients" +
                        "(name,email,phone,address) Values " +
                        "(@name,@email,@phone,@address);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            clientInfo.name = "";
            clientInfo.email = "";
            clientInfo.phone = "";
            clientInfo.address = "";
            successMessage = "New Client Added Successfully.";

            Response.Redirect("/Clients/Index");

        }
    }
}
