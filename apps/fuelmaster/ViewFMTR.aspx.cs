using System;
using System.Data.SqlClient;
using System.Web;

namespace MTAIntranet
{
    /// <summary>
    /// Summary for ViewFMTR
    /// </summary>
    public partial class ViewFMTR : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Fills dropdown box with VehicleIDs from SQL call
        /// </summary>
        [System.Web.Services.WebMethod]
        public static void FillVehIDDropBox()
        {
            HttpContext.Current.Response.ContentType = "application/json";
            string dbString = System.Configuration.ConfigurationManager.ConnectionStrings["FuelmasterTest"].ConnectionString;
            string fmVehQuery = "select distinct VEHICLEID from dbo.MainTrans where vehicleID < 10000 AND LEN(VEHICLEID) > 7 order by VEHICLEID";

            //begin try sql connection and command to fill dropdown boxes
            try
            {
                SqlConnection fmVehCon = new SqlConnection(dbString);
                SqlCommand fmVehCmd = new SqlCommand(fmVehQuery, fmVehCon);
                fmVehCon.Open();
                SqlDataReader fmVehReader = fmVehCmd.ExecuteReader();

                String fmVehIDInfo;

                // add empty option at the top for visual effect
                HttpContext.Current.Response.Write("<option>");
                HttpContext.Current.Response.Write("</option>");
                while (fmVehReader.Read())
                {
                    fmVehIDInfo = fmVehReader["VEHICLEID"].ToString().Remove(0, 4);

                    HttpContext.Current.Response.Write("<option>");
                    HttpContext.Current.Response.Write(fmVehIDInfo);
                    HttpContext.Current.Response.Write("</option>");
                }

                //Response.Write("</select>");

                fmVehCon.Close();
            }

            catch (Exception es)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write("Error: " + es.Message);
            }
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            //end try sql connection and command
        }// end FilVehIDDropBox

        /// <summary>
        /// Finds all FuelMaster transactions for vehicle ID and odometer
        /// entered by user with SQL call
        /// </summary>
        /// <param name="vehID"></param>
        /// <param name="vehOdometer"></param>
        [System.Web.Services.WebMethod]
        public static void GetFMTransactions(string vehID, string vehOdometer)
        {
            HttpContext.Current.Response.ContentType = "application/json";
            string dbString = System.Configuration.ConfigurationManager.ConnectionStrings["FuelmasterTest"].ConnectionString;
            string fmVehODQuery = "if EXISTS(select maintrans.userid from maintrans, [user] where VEHICLEID=" + "@vehID" + " and ODOMETER=" + "@vehOdometer" + " and maintrans.userid=[user].userid) " +
                                    " BEGIN " +
                                    " SELECT TC, LNAME, FNAME, SITEID,VEHICLEID, MAINTRANS.USERID, PRODUCT, TRANTIME, QUANTITY, ODOMETER from MAINTRANS, [user] WHERE " +
                                    " VEHICLEID = " + "@vehID" + " and ODOMETER = " + "@vehOdometer" + " and DAY(TRANTIME)>= 0 and maintrans.userid =[user].userid ORDER BY TRANTIME DESC" +
                                    " END " +
                                    " ELSE " +
                                    " BEGIN " +
                                    " SELECT TC, SITEID,VEHICLEID, MAINTRANS.USERID, PRODUCT, TRANTIME, QUANTITY, ODOMETER from MAINTRANS WHERE " +
                                    " VEHICLEID = " + "@vehID" + " and ODOMETER = " + "@vehOdometer" + " and DAY(TRANTIME)>= 0 ORDER BY TRANTIME DESC" +
                                    " END " +
                                    " if EXISTS(select maintrans.userid from maintrans, [user] where VEHICLEID = " + "@vehID" + " and ODOMETER = " + "@vehOdometer" + " and maintrans.userid =[user].userid) " +
                                    " BEGIN " +
                                    " SELECT TC, LNAME, FNAME, SITEID,VEHICLEID, MAINTRANS.USERID, PRODUCT, TRANTIME, QUANTITY, ODOMETER from MAINTRANS, [user] WHERE " +
                                    " VEHICLEID = " + "@vehID" + " and ODOMETER = " + "@vehOdometer" + " and DAY(TRANTIME)>= 0 and maintrans.userid =[user].userid ORDER BY TRANTIME DESC" +
                                    " END " +
                                    " ELSE " +
                                    " BEGIN " +
                                    " SELECT TC, SITEID,VEHICLEID, MAINTRANS.USERID, PRODUCT, TRANTIME, QUANTITY, ODOMETER from MAINTRANS WHERE " +
                                    " VEHICLEID = " + "@vehID" + " and ODOMETER = " + "@vehOdometer" + " and DAY(TRANTIME)>= 0 ORDER BY TRANTIME DESC" +
                                    " END ";

            SqlConnection fmVehODCon = new SqlConnection(dbString);
            SqlCommand fmVehODCmd = new SqlCommand(fmVehODQuery, fmVehODCon);
            fmVehODCmd.Parameters.AddWithValue("@vehID", vehID);
            fmVehODCmd.Parameters.AddWithValue("@vehOdometer", vehOdometer);
            fmVehODCon.Open();
            SqlDataReader fmVehODReader = fmVehODCmd.ExecuteReader();

            String fmVehODInfo;

            if (!fmVehODReader.HasRows)
            {
                HttpContext.Current.Response.Write("No transactions found for Vehicle " + vehID + " with odometer " + vehOdometer);
            }
            else
            {
                // build table of results
                HttpContext.Current.Response.Write("<table id=\"fmresultstable\">");
                HttpContext.Current.Response.Write("<caption><b>Fuelmaster Transactions</b></caption>");

                HttpContext.Current.Response.Write("<tr><th>TC " + "</th>" +
                               "<th>SITEID " + "</th>" +
                               "<th>VEHICLEID " + "</th>" +
                               "<th>LNAME " + "</th>" +
                               "<th>FNAME " + "</th>" +
                                "<th>USERID " + "</th>" +
                               "<th>PRODUCT " + "</th>" +
                               "<th>TRANTIME " + "</th>" +
                               "<th>QUANTITY " + "</th>" +
                               "<th>ODOMETER</th></tr>");

                while (fmVehODReader.Read())
                {
                    string userID = fmVehODReader["USERID"].ToString().Trim();
                    string lastName;
                    string firstName;

                    if (userID != "")
                    {
                        lastName = fmVehODReader["LNAME"].ToString();
                        firstName = fmVehODReader["FNAME"].ToString();
                    }
                    else
                    {
                        userID = "AIMS";
                        firstName = "AIMS";
                        lastName = "AIMS";
                    }

                    fmVehODInfo = "<td>" + fmVehODReader["TC"].ToString() + "</td>" +
                                  "<td>" + fmVehODReader["SITEID"].ToString() + "</td>" +
                                  "<td>" + fmVehODReader["VEHICLEID"].ToString() + "</td>" +
                                  "<td>" + lastName + "</td>" +
                                  "<td>" + firstName + "</td>" +
                                  "<td>" + userID + "</td>" +
                                  "<td>" + fmVehODReader["PRODUCT"].ToString() + "</td>" +
                                  "<td>" + fmVehODReader["TRANTIME"].ToString() + "</td>" +
                                  "<td>" + fmVehODReader["QUANTITY"].ToString() + "</td>" +
                                  "<td>" + fmVehODReader["ODOMETER"].ToString() + "</td>";
                    HttpContext.Current.Response.Write("<tr>");
                    HttpContext.Current.Response.Write(fmVehODInfo);
                    HttpContext.Current.Response.Write("</tr>");
                }
                HttpContext.Current.Response.Write("</table>");

                fmVehODCon.Close();
            }

            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }// end GetFMTransactions function
    }
}