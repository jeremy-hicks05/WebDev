using System;
using System.Data.SqlClient;
using System.Web;

namespace MTAIntranet
{
    /// <summary>
    /// Adds an entry into the MasterRoute template table
    /// </summary>
    public partial class AddMRTemplate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Summary for AddMasterRouteSQL
        /// </summary>
        /// <param name="route_name"></param>
        /// <param name="mr_start_date_time"></param>
        /// <param name="mr_start_hour"></param>
        /// <param name="mr_start_minute"></param>
        /// <param name="mr_return_hour"></param>
        /// <param name="mr_return_minute"></param>
        /// <param name="mr_return_date_time"
        /// <param name="mr_mode"></param>
        /// <param name="mr_suffix"></param>
        /// <param name="mr_route_num"></param>
        /// <param name="mr_run_num"></param>
        /// <param name="mr_day_of_week"></param>
        /// <param name="beg_dh_miles"></param>
        /// <param name="end_dh_miles"></param>
        [System.Web.Services.WebMethod]
        public static void AddMasterRouteSQL(
            string route_name,
            string mr_start_date_time,
            string mr_return_date_time,
            string mr_mode,
            string mr_suffix,
            string mr_route_num,
            string mr_run_num,
            string mr_day_of_week,
            string beg_dh_miles,
            string end_dh_miles
            )
        {
            HttpContext.Current.Response.ContentType = "application/json";
            foreach (char c in mr_day_of_week)
            {
                string dayOfWeek = "";
                SqlConnection con = new SqlConnection();

                //begin try sql connection and command to fill dropdown boxes
                try
                {
                    dayOfWeek = c.ToString();
                    string query = "Insert into dbo.Test_FR_Mile_Master_Small " +
                        "(route_name, " +
                        "mode, " +
                        "dow, " +
                        "route, " +
                        "run, " +
                        "suffix, " +
                        "pull_out_time, " +
                        "pull_in_time, " +
                        "beg_dh_miles, " +
                        "end_dh_miles) " +
                            "values (" +
                                "@route_name" + ", " +
                                "@mr_mode" + ", " +
                                "@dayOfWeek" + ", " +
                                "@mr_route_num" + ", " +
                                "@mr_run_num" + ", " +
                                "@mr_suffix" + ", " +
                                "@mr_start_date_time" + ", " +
                                "@mr_return_date_time" + ", " +
                                "@beg_dh_miles" + ", " +
                                "@end_dh_miles" + 
                                ")";

                    //string query = "Insert into dbo.Test_FR_Mile_Master_Small(route_name, mode, dow, route, run, suffix, pull_out_time, pull_in_time, beg_dh_miles, end_dh_miles) values" + 
                    //                "('name test', 'mode99', 'dow99', 'route99', 'run99', 'suffix99', '2022-01-01 01:01:00.000', '2022-01-01 01:11:00.000', '99', '99')";
                    string dbString = System.Configuration.ConfigurationManager.ConnectionStrings["MTATest"].ConnectionString;
                    con = new SqlConnection(dbString);
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@route_name", route_name);
                    cmd.Parameters.AddWithValue("@mr_mode", mr_mode);
                    cmd.Parameters.AddWithValue("@dayOfWeek", dayOfWeek);
                    cmd.Parameters.AddWithValue("@mr_route_num", mr_route_num);
                    cmd.Parameters.AddWithValue("@mr_run_num", mr_run_num);
                    cmd.Parameters.AddWithValue("@mr_suffix", mr_suffix);
                    cmd.Parameters.AddWithValue("@mr_start_date_time", mr_start_date_time);
                    cmd.Parameters.AddWithValue("@mr_return_date_time", mr_return_date_time);
                    cmd.Parameters.AddWithValue("@beg_dh_miles", beg_dh_miles);
                    cmd.Parameters.AddWithValue("@end_dh_miles", end_dh_miles);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                }

                catch (Exception es)
                {
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Write("Error: " + es.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            // Route_Info consists of Route_Name, Mode, Day_Of_Week, Route, Run, and Suffix
            HttpContext.Current.Response.Write("Added Route " + 
                    route_name + " " + 
                    mr_mode + " " + 
                    mr_day_of_week + " " +
                    mr_route_num + " " +
                    mr_suffix +
                " to Master Route Template");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            //end try sql connection and command
        }
        // end AddPulloffSQL
    }
}


