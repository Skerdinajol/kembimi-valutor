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
    }
}
