
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Xml.Serialization;

namespace MTAIntranet
{
    /// <summary>
    /// Summary of MasterRouteTemplate
    /// </summary>
    public static class MasterRouteTemplateRepository
    {
        /// <summary>
        /// Set to true to test editable Database
        /// </summary>
        static readonly bool smallDB = true;

        /// <summary>
        /// List of Master Routes to store the entries
        /// from the MasterRouteTemplate Table
        /// </summary>
        public static List<MasterRoute> MasterRouteTemplate;

        /// <summary>
        /// Builds MasterRouteListTemplate using a SQL call to the 
        /// table
        /// </summary>
        /// <returns></returns>
        public static void BuildMasterRouteListTemplate()
        {
            ClearMasterRouteList();
            GetMRsFromSql();
        }

        private static void ClearMasterRouteList()
        {
            if (MasterRouteTemplate == null)
            {
                MasterRouteTemplate = new List<MasterRoute>();
            }
            else
            {
                MasterRouteTemplate.Clear();
            }
        }

        private static void GetMRsFromSql()
        {
            DateTime dateIterator = new DateTime(DateTime.Now.Year, 01, 01);
            SqlConnection mileMasterCon = new SqlConnection();

            try
            {
                string mmDbString = System.Configuration.ConfigurationManager.ConnectionStrings["MTATest"].ConnectionString;
                string mileMasterQuery = "SELECT Route_Name, " +
                                            "Mode, " +
                                            "DoW, " +
                                            "Route, " +
                                            "Run, " +
                                            "Suffix, " +
                                            "pull_out_time, " +
                                            "pull_in_time, " +
                                            "pk_route_id, " +
                                            "beg_dh_miles, " +
                                            "end_dh_miles " +
                                            "FROM dbo.Test_FR_Mile_Master_Small WHERE DoW IS NOT NULL";

                mileMasterCon = new SqlConnection(mmDbString);
                SqlCommand mileMasterCmd = new SqlCommand(mileMasterQuery, mileMasterCon);
                //mileMasterCmd.Parameters.AddWithValue("@month", month);
                mileMasterCon.Open();
                SqlDataReader mileMasterSQLDR = mileMasterCmd.ExecuteReader();

                // load the first day of the month's instance of FR Mile Master's DB
                while (mileMasterSQLDR.Read()) // read row of Mile Master Template
                {
                    // get start time of master route
                    // ignore month and day due to inaccurate information in table

                    int startDay = DateTime.Parse(mileMasterSQLDR["pull_out_time"].ToString()).Day;
                    int endDay = DateTime.Parse(mileMasterSQLDR["pull_in_time"].ToString()).Day;

                    // instantiate MileMaster object
                    MasterRoute mileMasterEntry = new MasterRoute(
                        route_name: mileMasterSQLDR["route_name"].ToString(),
                        mode: mileMasterSQLDR["mode"].ToString(),
                        day_of_week: mileMasterSQLDR["dow"].ToString(),
                        route: mileMasterSQLDR["route"].ToString(),
                        run: mileMasterSQLDR["run"].ToString(),
                        suffix: mileMasterSQLDR["suffix"].ToString(),
                        pull_out_time: new DateTime(
                            DateTime.Now.Year,
                            dateIterator.Month,
                            dateIterator.Day,
                            DateTime.Parse(mileMasterSQLDR["pull_out_time"].ToString()).Hour,
                            DateTime.Parse(mileMasterSQLDR["pull_out_time"].ToString()).Minute,
                            DateTime.Parse(mileMasterSQLDR["pull_out_time"].ToString()).Second,
                            0),
                        pull_in_time: new DateTime(
                            DateTime.Now.Year,
                            dateIterator.Month,
                            dateIterator.Day,
                            DateTime.Parse(mileMasterSQLDR["pull_in_time"].ToString()).Hour,
                            DateTime.Parse(mileMasterSQLDR["pull_in_time"].ToString()).Minute,
                            DateTime.Parse(mileMasterSQLDR["pull_in_time"].ToString()).Second,
                            0),
                        beg_dh_miles: decimal.Parse(mileMasterSQLDR["beg_dh_miles"].ToString()),
                        end_dh_miles: decimal.Parse(mileMasterSQLDR["end_dh_miles"].ToString()));
                    // add object to MileMaster list
                    MasterRouteTemplate.Add(mileMasterEntry);
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            finally
            {
                mileMasterCon.Close();
            }
        }
    }

}