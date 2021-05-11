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
            string conec = "Data Source=DESKTOP-N9AAJ82\\SKERDI;Initial Catalog=KEMBIM_VALUTOR;Integrated Security=True";
            string qrstr = "select user_id from users where username = '" + user.Username + "' and password = '" + user.Password +"' "; 
            using(SqlConnection con = new SqlConnection(conec))
            {
                SqlCommand cmd = new SqlCommand(qrstr, con);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (int.Parse(reader[0].ToString()) > 0)
                    {
                        Session["user_id"] = int.Parse(reader[0].ToString());
                        return View("~/index");
                    }
                }
            }
            return View();
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