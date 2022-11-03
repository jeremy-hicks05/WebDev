namespace MTAIntranet
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using System.Web;
    using System.Xml.Serialization;
    
    /// <summary>
    /// Class to store a list/array of Pulloffs
    /// </summary>
    public static class PulloffsRepository
    {
        /// <summary>
        /// Array of Lists of Pulloffs to store 12 months of pulloffs
        /// 
        /// </summary>
        public static List<Pulloff>[] PulloffsArray;

        // begin static functions
        #region static functions

        /// <summary>
        /// Builds one month's entry of the array using SQL call to table
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static void BuildPulloffsArray(string month)
        {
            ClearPulloffArray(month);
            GetPulloffsFromSql(month);
        }

        private static void ClearPulloffArray(string month)
        {
            if (PulloffsArray == null)
            {
                PulloffsArray = new List<Pulloff>[12];

                for (int i = 0; i < PulloffsArray.Length; i++)
                {
                    PulloffsArray[i] = new List<Pulloff>();
                }
            }
            else
            {
                foreach(List<Pulloff> pulloffs in PulloffsArray)
                {
                    pulloffs.Clear();
                }
            }
        }

        private static void GetPulloffsFromSql(string month)
        {
            int monthInt = int.Parse(month) - 1;
            string dbString = System.Configuration.ConfigurationManager.ConnectionStrings["MTATest"].ConnectionString;
            string pulloffsQuery = "SELECT * FROM dbo.TestPulloffs WHERE (DATEPART(yy, PulloffTime) = @year AND DATEPART(MM, PulloffTime) = @month) ORDER BY PulloffReturn";
            SqlConnection pulloffsCon = new SqlConnection();

            try
            {
                // connect to DB and see if any pulloffs exist for current year
                pulloffsCon = new SqlConnection(dbString);
                SqlCommand pulloffsCmd = new SqlCommand(pulloffsQuery, pulloffsCon);
                pulloffsCmd.Parameters.AddWithValue("@month", month);
                pulloffsCmd.Parameters.AddWithValue("@year", DateTime.Now.Year);
                pulloffsCon.Open();

                SqlDataReader pulloffsSQLDR = pulloffsCmd.ExecuteReader();

                while (pulloffsSQLDR.Read())
                {
                    Pulloff pullOff = new Pulloff(
                        pulloff_ID: int.Parse(pulloffsSQLDR["PulloffID"].ToString()),
                        route_Name: pulloffsSQLDR["Route_Name"].ToString(),
                        mode: pulloffsSQLDR["Mode"].ToString(),
                        doW: pulloffsSQLDR["DoW"].ToString(),
                        route: pulloffsSQLDR["Route"].ToString(),
                        pulloffTime: DateTime.Parse(pulloffsSQLDR["PullOffTime"].ToString()),
                        pulloffReturn: DateTime.Parse(pulloffsSQLDR["PullOffReturn"].ToString()),
                        run: pulloffsSQLDR["Run"].ToString(),
                        suffix: pulloffsSQLDR["Suffix"].ToString());

                    // add to list of pulloffs
                    PulloffsArray[monthInt].Add(pullOff);
                }// end while pulloffs read loop
            }
            catch (Exception es)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write("Error: " + es.Message);
            }
            finally
            {
                pulloffsCon.Close();
            }
            MatchAllPulloffs(month);
        }

        /// <summary>
        /// Assigns a pulloff to a MasterRoute by leveraging the Dictionary
        /// Key with a GetSignature Call to ID the correct MR
        /// </summary>
        /// <param name="month"></param>
        public static void MatchAllPulloffs(string month)
        {
            int monthInt = int.Parse(month) - 1;

            foreach (Pulloff pulloff in PulloffsArray[monthInt])
            {
                int pulloffDayInt = pulloff.PulloffTime.Day - 1;

                // effectively attach pulloff to masterRoute
                // * high efficiency here *

                // reverse attachment of pulloff to master route
                MasterRouteRepository.MasterRouteDictionary
                            [monthInt]
                            [pulloffDayInt]
                            [pulloff.GetSignature()] // find masterRoute using pulloff signature
                            .AddMatch(pulloff);
            }
        }
        #endregion
        // end static functions
    }
}