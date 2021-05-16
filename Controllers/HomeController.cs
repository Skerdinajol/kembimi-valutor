using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KembimValutor.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<Models.rates> rates = new List<Models.rates>();

            SqlConnectionStringBuilder constr = new SqlConnectionStringBuilder("Data Source=DESKTOP-N9AAJ82\\SKERDI;Initial Catalog=KEMBIM_VALUTOR;Integrated Security=True");
            string qrstr = "select * from rates";
            using (SqlConnection con = new SqlConnection(constr.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(qrstr, con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.SingleResult))
                {
                        while(reader.Read())
                        {
                            Models.rates rate = new Models.rates();

                            rate.RateId = (int)reader["RATE_ID"];
                            rate.Curr1 = (string)reader["CURR1"];
                            rate.Curr2 = (string)reader["CURR2"];
                            rate.Rate = (double)reader["RATE"];
                            rates.Add(rate);
                        }

                }
                if (ViewBag.transcomplete == null)
                {
                    ViewBag.transcomplete = false;
                }
                return View(rates);
            }

        }

        public ActionResult favorite(int RateId)
        {
            SqlConnectionStringBuilder constr = new SqlConnectionStringBuilder("Data Source=DESKTOP-N9AAJ82\\SKERDI;Initial Catalog=KEMBIM_VALUTOR;Integrated Security=True");
            string qrstr = "insert into favorites(user_id,rate_id) values("+Session["user_id"]+","+RateId+")";
            using (SqlConnection con = new SqlConnection(constr.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(qrstr, con);
                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    ViewBag.Ex = "Already added to favorites";
                }
                con.Close();
            }
            return RedirectToAction("Index");
        }

        public ActionResult search(string curr)
        {
            if (curr == "")
            {
                return RedirectToAction("Index");
            }
            List<Models.rates> rates = new List<Models.rates>();

            SqlConnectionStringBuilder constr = new SqlConnectionStringBuilder("Data Source=DESKTOP-N9AAJ82\\SKERDI;Initial Catalog=KEMBIM_VALUTOR;Integrated Security=True");
            string qrstr = "select * from RATES where curr1 ='"+curr+"' or curr2 = '"+curr+"'";
            using (SqlConnection con = new SqlConnection(constr.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(qrstr, con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.SingleResult))
                {
                    while (reader.Read())
                    {
                        Models.rates rate = new Models.rates();

                        rate.RateId = (int)reader["RATE_ID"];
                        rate.Curr1 = (string)reader["CURR1"];
                        rate.Curr2 = (string)reader["CURR2"];
                        rate.Rate = (double)reader["RATE"];
                        rates.Add(rate);
                    }

                }
            }
            return View("Index", rates);
        }
        public ActionResult exchange(int RateId)
        {
            Models.rates rate = new Models.rates();
            rate.RateId = RateId;
            SqlConnectionStringBuilder constr = new SqlConnectionStringBuilder("Data Source=DESKTOP-N9AAJ82\\SKERDI;Initial Catalog=KEMBIM_VALUTOR;Integrated Security=True");
            
            string qrstr = "select curr1, curr2, rate from rates where rate_id = " + RateId + "";

            using (SqlConnection con = new SqlConnection(constr.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(qrstr, con);

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    rate.Curr1 = (string)reader[0];
                    rate.Curr2 = (string)reader[1];
                    rate.Rate = (double)reader[2];
                    reader.Close();
                }
            }
            ViewBag.noval = false;
            ViewBag.transcomplete = false;
            return View("../Shared/exchange",rate);
            
        }
        public ActionResult exchanged(int RateId, string Curr1, string Curr2, double Rate, double Value)
        {
            double val1 = 0;
            double val2 = 0;
            SqlConnectionStringBuilder constr = new SqlConnectionStringBuilder("Data Source=DESKTOP-N9AAJ82\\SKERDI;Initial Catalog=KEMBIM_VALUTOR;Integrated Security=True");
            string qrstr = "select \"" + Curr1 + "\",\"" + Curr2 + "\" from wallet where user_id = " + Session["user_id"] + "";
            using (SqlConnection con = new SqlConnection(constr.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(qrstr, con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    try
                    {
                        val1 += (double)reader[0] - Value;
                        val2 += (double)reader[1] + Value*Rate;
                    }
                    catch (Exception)
                    {
                    }
                    if (val1 < 0)
                    {
                        ViewBag.noval = true; 
                        return View("../Shared/exchange");
                    }
                }
                string qrstradd = "Update wallet set \""+Curr1+ "\" = " + val1 + ",\"" + Curr2+ "\" = " + val2 +" where user_id = "+Session["user_id"]+"";
                SqlCommand cmdadd = new SqlCommand(qrstradd, con);
                cmdadd.ExecuteNonQuery();
                con.Close();
            }
            ViewBag.transcomplete = true;
            return RedirectToAction("Index");
        }
    }
}
