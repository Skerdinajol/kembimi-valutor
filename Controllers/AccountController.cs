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
        public ActionResult logout()
        {
            Session.Abandon();
            return RedirectToAction("../Home/Index");
        }
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(Models.users user)
        {
            SqlConnectionStringBuilder constr = new SqlConnectionStringBuilder("Data Source=DESKTOP-N9AAJ82\\SKERDI;Initial Catalog=KEMBIM_VALUTOR;Integrated Security=True");
            string qrstr = "select user_id from users where username = '" + user.Username + "' and password = '" + user.Password +"'";
            
            using (SqlConnection con = new SqlConnection(constr.ConnectionString))
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
                        string qrstrwl = "insert into wallet(user_id) values('" + Session["user_id"] + "')";
                        SqlCommand cmdw = new SqlCommand(qrstrwl, con);
                        try
                        {
                        cmdw.ExecuteNonQuery();
                        }
                        catch (Exception)
                        { }
                       
                        return RedirectToAction("../Home/Index");
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
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ViewBag.Ex = ex;
                }
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
                string qrstr = "select name,surname,username,birthday,email,password from users where user_id = '"+ Session["user_id"] +"'";
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
                            user.Birthday = (string)reader[3];
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


                    //------------------------------------Favorites------------------------------------------------------------------------
                    
                    List<Models.rates> ratesf = new List<Models.rates>();
                    string qrFavStr = "SELECT RATES.RATE_ID AS rate_id,CURR1 AS curr1,CURR2 AS curr2,RATE AS rate FROM RATES INNER JOIN FAVORITES ON RATES.RATE_ID = FAVORITES.RATE_ID WHERE USER_ID = '" + Session["user_id"] + "'";
                    SqlCommand cmdFav = new SqlCommand(qrFavStr, con);
                    using (SqlDataReader readerf = cmdFav.ExecuteReader(System.Data.CommandBehavior.SingleResult))
                    {
                        while (readerf.Read())
                        {
                            Models.rates ratef = new Models.rates();

                            ratef.RateId = (int)readerf["rate_id"];
                            ratef.Curr1 = (string)readerf["curr1"];
                            ratef.Curr2 = (string)readerf["curr2"];
                            ratef.Rate = (double)readerf["rate"];
                            ratesf.Add(ratef);
                        }

                    }
                ViewBag.fav = ratesf;
                }
                        return View("userdetails", user);
            }
            return RedirectToAction("../Home/Index");
        }   
        public ActionResult wallet(Models.Wallet wallet)
        {

            SqlConnectionStringBuilder constr = new SqlConnectionStringBuilder("Data Source=DESKTOP-N9AAJ82\\SKERDI;Initial Catalog=KEMBIM_VALUTOR;Integrated Security=True");
            string qrstr = "select EUR,GBP,"+"\"ALL\""+",USD from wallet where user_id = '" + Session["user_id"] + "'";
            using (SqlConnection con = new SqlConnection(constr.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(qrstr, con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    try
                    {
                        wallet.Eur = (double)reader[0];
                    }
                    catch (Exception)
                    { wallet.Eur = 0; }
                    try
                    {
                        wallet.Gbp = (double)reader[1];
                    }
                    catch (Exception)
                    { wallet.Gbp = 0; }
                    try
                    {
                        wallet.All = (double)reader[2];
                    }
                    catch (Exception)
                    { wallet.All = 0; }
                    try
                    {
                        wallet.Usd = (double)reader[3];
                    }
                    catch (Exception) 
                    { wallet.Usd = 0; }

                    reader.Close();
                }
            }
            ViewBag.edit = false;

                return View(wallet);
        }
        public ActionResult add()
        {
            ViewBag.edit = true;
            return View("wallet");
        }
        [HttpPost]
        public ActionResult add(Models.Wallet wallet)
        {
            SqlConnectionStringBuilder constr = new SqlConnectionStringBuilder("Data Source=DESKTOP-N9AAJ82\\SKERDI;Initial Catalog=KEMBIM_VALUTOR;Integrated Security=True");
            string qrstr = "select eur, gbp, \"all\", usd from wallet where user_id = "+Session["user_id"]+"";
            using (SqlConnection con = new SqlConnection(constr.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(qrstr, con);
                con.Open();
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    try
                    {
                    wallet.Eur += (double)reader[0];
                    wallet.Gbp += (double)reader[1];
                    wallet.All += (double)reader[2];
                    wallet.Usd += (double)reader[3];
                    }
                    catch (Exception)
                    {
                    }
                }
            string qrstradd = "Update wallet set eur = " + wallet.Eur + ",gbp = " + wallet.Gbp + ",\"all\" = " + wallet.All + ",usd = " + wallet.Usd + "";
                SqlCommand cmdadd = new SqlCommand(qrstradd, con);
                cmdadd.ExecuteNonQuery();
                con.Close();
            }
            return RedirectToAction("wallet");
        }

        public ActionResult favorite(int RateId)
        {
            SqlConnectionStringBuilder constr = new SqlConnectionStringBuilder("Data Source=DESKTOP-N9AAJ82\\SKERDI;Initial Catalog=KEMBIM_VALUTOR;Integrated Security=True");
            string qrstr = "delete from favorites where rate_id = "+RateId+" and user_id = " + Session["user_id"] + "";
            using (SqlConnection con = new SqlConnection(constr.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(qrstr, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            return RedirectToAction("userdetails");
        }
        public ActionResult edit()
        {
            Models.users user = new Models.users();
            SqlConnectionStringBuilder constr = new SqlConnectionStringBuilder("Data Source=DESKTOP-N9AAJ82\\SKERDI;Initial Catalog=KEMBIM_VALUTOR;Integrated Security=True");
            string qrstr = "select name,surname,username,birthday,email,password from users where user_id = '" + Session["user_id"] + "'";
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
                        user.Birthday = (string)reader[3];
                    }
                    catch (Exception) { }

                    try
                    {
                        user.Email = (string)reader[4];
                    }
                    catch (Exception)
                    { }
                    try
                    {
                        user.Password = (string)reader[5];
                    }
                    catch (Exception)
                    { }
                    reader.Close();
                }
            }
                return View(user);
        }
        [HttpPost]
        public ActionResult edit(Models.users user)
        {
            SqlConnectionStringBuilder constr = new SqlConnectionStringBuilder("Data Source=DESKTOP-N9AAJ82\\SKERDI;Initial Catalog=KEMBIM_VALUTOR;Integrated Security=True");
            string qrstr = "update users set name = '" + user.Name + "', surname = '" + user.Surname + "', username = '" + user.Username + "', password = '"+user.Password+"', birthday = "+user.Birthday+", email = '"+user.Email+"' where user_id = "+Session["user_id"]+"";
            using (SqlConnection con = new SqlConnection(constr.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(qrstr, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            return RedirectToAction("userdetails");
        }
    }
}