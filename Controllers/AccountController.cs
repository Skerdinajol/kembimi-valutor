using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KembimValutor.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(Models.users user)
        {
            SqlConnectionStringBuilder constr = new SqlConnectionStringBuilder("Data Source=DESKTOP-N9AAJ82\\SKERDI;Initial Catalog=KEMBIM_VALUTOR;Integrated Security=True");
            string qrstr = "select user_id from users where username = '" + user.Username + "' and password = '" + user.Password +"'"; 
            using(SqlConnection con = new SqlConnection(constr.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(qrstr, con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    if (reader.FieldCount > 0)
                    {
                        Session["user_id"] = int.Parse(reader[0].ToString());
                        reader.Close();
                        return View("../Home/Index");
                    }
                    else 
                    {
                        //e ndryshoj m vone
                        reader.Close();
                        return View("../register");
                    }
                }
            }
        }
        public ActionResult register()
        {
            return View();
        }
        public ActionResult userdetails()
        {
            return View();
        }
    }
}