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
                    if (reader.HasRows)
                    {
                        Session["user_id"] = int.Parse(reader[0].ToString());
                        reader.Close();
                        return View("../Home/Index");
                    }
                    else 
                    {
                        //e ndryshoj m vone
                        reader.Close();
                        ViewBag.valMsg = "Your username or password is incorrect";
                        return View();
                    }
                }
            }
        }
        public ActionResult register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult register(Models.users user)
        {
            SqlConnectionStringBuilder constr = new SqlConnectionStringBuilder("Data Source=DESKTOP-N9AAJ82\\SKERDI;Initial Catalog=KEMBIM_VALUTOR;Integrated Security=True");
            string qrstr = "insert into users (username, password, name, surname, email, type) values('"+user.Username+ "','" + user.Password + "','" + user.Name + "','" + user.Surname + "','" + user.Email + "','U')";
            using (SqlConnection con = new SqlConnection(constr.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(qrstr, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return View("../Account/login");
            }
            
        }
        public ActionResult userdetails()
        {
            if (Session["user_id"] != null)
            {
                Models.users user = new Models.users();
                SqlConnectionStringBuilder constr = new SqlConnectionStringBuilder("Data Source=DESKTOP-N9AAJ82\\SKERDI;Initial Catalog=KEMBIM_VALUTOR;Integrated Security=True");
                string qrstr = "select name,surname,username,birthday,email from users where user_id = '"+ Session["user_id"] +"'";
                using (SqlConnection con = new SqlConnection(constr.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(qrstr, con);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        try
                        {
                            user.Name = (string)reader[0];
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            user.Surname = (string)reader[1];
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            user.Username = (string)reader[2];
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            user.Birthday = (DateTime)reader[3];
                        }
                        catch (Exception){ }

                        try
                        {
                            user.Email = (string)reader[4];
                        }
                        catch (Exception)
                        { }
                        
                        reader.Close();
                    }
                }
                        return View("userdetails", user);
            }
            return View("../Home/Index");
        }
    }
}