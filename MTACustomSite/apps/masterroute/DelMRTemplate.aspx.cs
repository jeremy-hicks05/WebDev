using System;
using System.Data.SqlClient;
using System.Web;

namespace MTAIntranet
{
    /// <summary>
    /// Deletes entry from MasterRoute template table
    /// </summary>
    public partial class DelMRTemplate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Populates dropdown box with MasterRoute template table information
        /// </summary>
        [System.Web.Services.WebMethod]
        public static void FillMasterRouteList()
        {
            //
            HttpContext.Current.Response.ContentType = "application/json";
            SqlConnection con = new SqlConnection();

            //begin try sql connection and command to fill dropdown boxes
            try
            {
                string query = "select * from dbo.Test_FR_Mile_Master_Small";

                string dbString = System.Configuration.ConfigurationManager.ConnectionStrings["MTATest"].ConnectionString;
                con = new SqlConnection(dbString);
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                HttpContext.Current.Response.Write("<option value=\"\">");
                while (reader.Read())
                {
                    HttpContext.Current.Response.Write("<option value=\"" + reader["pk_route_id"] + "\">");
                    HttpContext.Current.Response.Write(reader["mode"].ToString() + " ");
                    HttpContext.Current.Response.Write(reader["dow"].ToString() + " ");
                    HttpContext.Current.Response.Write(reader["route"].ToString() + " ");
                    HttpContext.Current.Response.Write(reader["run"].ToString() + " ");
                    HttpContext.Current.Response.Write(reader["suffix"].ToString() + " ");
                    HttpContext.Current.Response.Write(reader["route_name"].ToString() + " ");
                    HttpContext.Current.Response.Write("</option>");
                }
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

            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            //end try sql connection and command
        }

        /// <summary>
        /// Runs SQL command to delete MasterRoute from table
        /// </summary>
        /// <param name="pk_route_id"></param>
        [System.Web.Services.WebMethod]
        public static void DelMasterRouteSQL(string pk_route_id)
        {
            HttpContext.Current.Response.ContentType = "application/json";
            if (pk_route_id == "")
            {
                HttpContext.Current.Response.Write("Please select a route");
            }
            else
            {
                SqlConnection con = new SqlConnection();

                //begin try sql connection and command to fill dropdown boxes
                try
                {
                    string query = "delete from dbo.Test_FR_Mile_Master_Small where PK_Route_ID = @pk_route_id";

                    string dbString = System.Configuration.ConfigurationManager.ConnectionStrings["MTATest"].ConnectionString;
                    con = new SqlConnection(dbString);
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@pk_route_id", pk_route_id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    HttpContext.Current.Response.Write("Deleted Route from Master Route Template");
                }

                catch (Exception es)
                {
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Write("Error: " + es.Message);
                }
                finally
                {
                    con.Close();
                    MasterRouteTemplateRepository.BuildMasterRouteListTemplate();
                }
            }
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            //end try sql connection and command
        }
        // end DelPulloffSQL
    }
}


