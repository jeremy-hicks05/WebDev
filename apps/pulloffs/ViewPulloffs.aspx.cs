using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

namespace MTAIntranet
{
    /// <summary>
    /// Summary of ViewPulloffs
    /// </summary>
    public partial class ViewPulloffs : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Summary of FillPulloffTable
        /// </summary>
        /// <param name="month"></param>
        [WebMethod]
        public static void FillPulloffTable(string month)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.ContentType = "application/json";
            if (month == "")
            {
                response.Write("Please select a month");
            }
            else
            {
                int intMonth = int.Parse(month.ToString()) - 1;
                MasterRouteRepository.BuildMasterRouteDictionary(month);
                response.ContentType = "application/json";
                if (PulloffsRepository.PulloffsArray[intMonth].Count < 1)
                {
                    response.Write("No pulloffs for the month of " + month);
                }
                else
                {
                    MasterRouteRepository.WriteRouteTable(
                        MasterRouteRepository.GetRouteListWithPulloffsForMonth(month));
                }
            }
            response.Flush();
            response.End();
        }
        // end FillPulloffTable

        /// <summary>
        /// Summary of DelPulloffSQL
        /// </summary>
        /// <param name="pulloffid"></param>
        /// <param name="month"></param>
        [WebMethod]
        public static void DelPulloffSQL(string pulloffid, string month)
        {
            HttpResponse response = HttpContext.Current.Response;
            SqlConnection con = new SqlConnection();
            //begin try sql connection and command to fill dropdown boxes
            try
            {
                string query = "DELETE FROM dbo.TestPulloffs WHERE PulloffID = " + "@pulloffid";
                string dbString = System.Configuration.ConfigurationManager.ConnectionStrings["MTATest"].ConnectionString;

                con = new SqlConnection(dbString);
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@pulloffid", pulloffid);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
            }
            catch (Exception es)
            {
                response.ContentType = "application/json";

                response.Clear();
                response.Write("Error: " + es.Message);

                response.Flush();
            }
            finally
            {
                con.Close();
                
            }
            //end try sql connection and command
        }
        // end AddPulloffSQL
    }
}// end ViewPulloffs class