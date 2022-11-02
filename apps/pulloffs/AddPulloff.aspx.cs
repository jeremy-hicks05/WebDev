using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web;

namespace MTAIntranet
{
    /// <summary>
    /// Adds pulloff to SQL table of pulloffs
    /// </summary>
    public partial class AddPulloff : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //---For testing JS objects pushed to functions---//
        //[System.Web.Services.WebMethod]
        //public static void TestFunction(Pulloff pulloff)
        //{
        //    // fills this year, the year before, and next year into a dropdown box not currently editable
        //    HttpContext.Current.Response.ContentType = "application/json";

        //    //int thisYear = int.Parse(DateTime.Now.ToString("yyyy"));

        //    HttpContext.Current.Response.Write(pulloff.ToString());

        //    HttpContext.Current.Response.Flush();
        //    HttpContext.Current.Response.Close();
        //}
        //---End testing JS objects pushed to functions---//

        /// <summary>
        /// Populate Year dropdown box with previous, current, and next year for
        /// special cases and historical queries
        /// </summary>
        [System.Web.Services.WebMethod]
        public static void FillYears()
        {
            HttpResponse response = HttpContext.Current.Response;
            // fills this year, the year before, and next year into a dropdown box not currently editable
            response.ContentType = "application/json";

            int thisYear = int.Parse(DateTime.Now.ToString("yyyy"));

            response.Write("<option value = \"\"></option>");
            response.Write("<option value = \"" + (thisYear - 1).ToString() + "\">" + (thisYear - 1).ToString() + "</option>");
            response.Write("<option value = \"" + (thisYear).ToString() + "\" selected>" + (thisYear).ToString() + "</option>");
            response.Write("<option value = \"" + (thisYear + 1).ToString() + "\">" + (thisYear + 1).ToString() + "</option>");

            response.Flush();
            response.End();
        }

        /// <summary>
        /// Fills the dropdown box with 1-12
        /// </summary>
        [System.Web.Services.WebMethod]
        public static void FillMonths()
        {
            HttpResponse response = HttpContext.Current.Response;
            // fills the months 1-12
            response.ContentType = "application/json";
            response.Write("<option value = \"\"></option>");
            int thisYear = int.Parse(DateTime.Now.ToString("yyyy"));
            for (int i = 1; i < 13; i++)
            {
                DateTime tempDate = new DateTime(thisYear, i, 01);
                string month = DateTime.Parse(tempDate.ToString()).ToString("MM");
                response.Write("<option value = \"" + month + "\">" + month + "</option>");
            }

            response.Flush();
            response.End();
        }

        /// <summary>
        /// Fills dropdown box with days of the month
        /// </summary>
        /// <param name="month"></param>
        [System.Web.Services.WebMethod]
        public static void FillDays(string month)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.ContentType = "application/json";

            int intMonth = int.Parse(month);
            int thisYear = int.Parse(DateTime.Now.ToString("yyyy"));
            DateTime tempDate;

            response.Write("<option value = \"\"></option>");
            for (tempDate = new DateTime(thisYear, intMonth, 1); tempDate.Month == intMonth; tempDate = tempDate.AddDays(1))
            {
                response.Write("<option value = \"" + tempDate.ToString("dd") + "\">" + tempDate.ToString("dd") + "</option>");
            }

            response.Flush();
            response.End();
        }

        /// <summary>
        /// Fills dropdown box with applicable hours based on routes and current pulloffs
        /// </summary>
        /// <param name="routeInfo"></param>
        /// <param name="pulloffMonth"></param>
        /// <param name="pulloffDay"></param>
        [System.Web.Services.WebMethod]
        public static void FillPulloffHours(string routeInfo, string pulloffMonth, string pulloffDay)
        {
            HttpResponse response = HttpContext.Current.Response;
            //HttpContext.Current.Response.Flush();
            response.ContentType = "application/json";

            MasterRoute myMasterRoute = MasterRouteRepository.GetRoute(routeInfo, pulloffMonth, pulloffDay);

            DateTime startTime = myMasterRoute.GetEarliestAvailableTime();
            DateTime endTime = myMasterRoute.Pull_In_Time;

            // write hour response
            response.Write("<option value = \"\"></option>");
            if(startTime > endTime)
            {
                endTime = endTime.AddDays(1);
            }
            for (; startTime <= endTime; startTime = startTime.AddHours(1))
            {
                response.Write("<option value = \"" + startTime.ToString("HH") + "\">" + startTime.ToString("HH") + "</option>");
            }

            if (startTime.Minute > endTime.Minute)
            {
                response.Write("<option value = \"" + endTime.ToString("HH") + "\">" + endTime.ToString("HH") + "</option>");
            }
            // end write hour response

            response.Flush();
            response.End();
            //HttpContext.Current.Response.Close();
        }

        /// <summary>
        /// Fills dropdown box with applicable minutes based on routes and current pulloffs
        /// </summary>
        /// <param name="routeInfo"></param>
        /// <param name="pulloffMonth"></param>
        /// <param name="pulloffDay"></param>
        /// <param name="pulloffHour"></param>
        [System.Web.Services.WebMethod]
        public static void FillPulloffMinutes(string routeInfo, string pulloffMonth, string pulloffDay, string pulloffHour)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.ContentType = "application/json";

            MasterRoute myMasterRoute = MasterRouteRepository.GetRoute(routeInfo, pulloffMonth, pulloffDay);

            int intHour = int.Parse(pulloffHour);

            DateTime startTime = myMasterRoute.GetEarliestAvailableTime();
            DateTime endTime = myMasterRoute.Pull_In_Time;

            response.Write("<option value = \"\"></option>");

            // write minute response
            if (startTime.Hour == intHour)
            {
                for (; startTime.Hour == intHour && startTime <= endTime; startTime = startTime.AddMinutes(5))
                {
                    response.Write("<option value = \"" + startTime.ToString("mm") + "\">" + startTime.ToString("mm") + "</option>");
                }
            }
            else if(endTime.Hour == intHour)
            {
                for (int i = 0; i <= endTime.Minute; i+=5)
                {
                    response.Write("<option value = \"" + i.ToString("00") + "\">" + i.ToString("00") + "</option>");
                }
            }
            else
            {
                for (int i = 0; i < 60; i+=5)
                {
                    response.Write("<option value = \"" + i.ToString("00") + "\">" + i.ToString("00") + "</option>");
                }
            }
            // end write minute response

            response.Flush();
            response.End();
        }

        /// <summary>
        /// Fills dropdown box with applicable return hours based on routes and current pulloffs
        /// </summary>
        /// <param name="routeInfo"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="pHour"></param>
        [System.Web.Services.WebMethod]
        public static void FillReturnHours(string routeInfo, string month, string day, string pHour)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.ContentType = "application/json";

            MasterRoute myMasterRoute = MasterRouteRepository.GetRoute(routeInfo, month, day);

            DateTime startTime = myMasterRoute.GetEarliestAvailableTime();
            DateTime tempTime = startTime;

            int pulloffHour = int.Parse(pHour);

            while (tempTime.Hour != pulloffHour)
            {
                tempTime = tempTime.AddHours(1);
            }
            DateTime endTime = myMasterRoute.Pull_In_Time;

            response.Write("<option value = \"\"></option>");

            if (startTime > endTime) // if start time is later than end time
            {
                endTime = endTime.AddDays(1);
            }
            for (; tempTime <= endTime; tempTime = tempTime.AddHours(1))
            {
                response.Write("<option value = \"" + tempTime.ToString("HH") + "\">" + tempTime.ToString("HH") + "</option>");
            }

            if (startTime.Minute > endTime.Minute)
            {
                response.Write("<option value = \"" + endTime.ToString("HH") + "\">" + endTime.ToString("HH") + "</option>");
            }

            response.Flush();
            response.End();
        }

        /// <summary>
        /// Fills dropdown box with applicable return minutes based on routes and current pulloffs
        /// </summary>
        /// <param name="routeInfo"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="pHour"></param>
        /// <param name="rHour"></param>
        /// <param name="pMin"></param>
        [System.Web.Services.WebMethod]
        public static void FillReturnMinutes(string routeInfo, string month, string day, string pHour, string rHour, string pMin)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.ContentType = "application/json";
            MasterRoute myMasterRoute = MasterRouteRepository.GetRoute(routeInfo, month, day);

            int pulloffMin = int.Parse(pMin);

            //DateTime startTime = myMasterRoute.GetEarliestAvailableTime();
            DateTime endTime = myMasterRoute.Pull_In_Time;

            response.Write("<option value = \"\"></option>");
            if (pHour == rHour)
            {
                // add only minutes greater than or equal to pMin to Response
                if (rHour == endTime.ToString("HH"))
                {
                    for (int i = pulloffMin; i <= endTime.Minute; i += 5)
                    {
                        response.Write("<option value = \"" + i.ToString("00") + "\">" + i.ToString("00") + "</option>");
                    }
                }
                else
                {
                    for (int i = pulloffMin; i <= 55; i += 5)
                    {
                        response.Write("<option value = \"" + i.ToString("00") + "\">" + i.ToString("00") + "</option>");
                    }
                }
            }
            else
            {
                if (rHour == endTime.ToString("HH"))
                {
                    for (int i = 0; i <= endTime.Minute; i += 5)
                    {
                        response.Write("<option value = \"" + i.ToString("00") + "\">" + i.ToString("00") + "</option>");
                    }
                }
                // add all minutes 00 - 55 to Response
                // add only minutes greater than or equal to pMin to Response
                else
                {
                    DateTime tempDate = new DateTime(2000, 1, 1, 0, 0, 0);
                    string thisHour = tempDate.ToString("HH");

                    for (; tempDate.ToString("HH") == thisHour; tempDate = tempDate.AddMinutes(5))
                    {
                        response.Write("<option value = \"" + tempDate.ToString("mm") + "\">" + tempDate.ToString("mm") + "</option>");
                    }
                }
            }
            response.Flush();
            response.End();
        }

        /// <summary>
        /// Displays table of current pulloffs for selected route, month, and day
        /// to inform users of reason for hour/minute restriction as well as
        /// prevent dual input after seeing that pulloffs already exist
        /// </summary>
        /// <param name="routeInfo"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        [System.Web.Services.WebMethod]
        public static void FillRoutePreview(string routeInfo, string month, string day)
        {
            HttpContext.Current.Response.ContentType = "application/json";
            PulloffsRepository.BuildPulloffsArray(month);

            MasterRouteRepository.WritePulloffsFor(
                MasterRouteRepository.GetRoute(routeInfo, month, day));
            HttpContext.Current.Response.End();
        }
        // end FillRoutePreview

        /// <summary>
        /// Populates dropdown box with Route info (route name, mode, day of week, route num, run num, and suffix
        /// for selecting which route is accepting the pulloff addition
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        [System.Web.Services.WebMethod]
        public static void FillRouteInfo(string month, string day)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.ContentType = "application/json";

            //HttpContext.Current.Response.Write("<optgroup label = \"RouteName - Mode - DoW - RouteNum - RunNum - Suffix\">");
            response.Write("<option>");
            response.Write("</option>");

            foreach (KeyValuePair<string, MasterRoute> masterRoute in
                        MasterRouteRepository.GetRouteDictionary(month, day))
            {
                response.Write("<option>");
                response.Write(masterRoute.Value.Route_Info);
                response.Write("</option>");
            }

            //HttpContext.Current.Response.Write("</optgroup>");

            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        // end FillRouteInfo

        /// <summary>
        /// Runs SQL to insert pulloff into the SQL table
        /// </summary>
        /// <param name="route"></param>
        /// <param name="pulloffmonth"></param>
        /// <param name="pulloffday"></param>
        /// <param name="pulloffyear"></param>
        /// <param name="pulloffhour"></param>
        /// <param name="pulloffminute"></param>
        /// <param name="returnhour"></param>
        /// <param name="returnminute"></param>
        [System.Web.Services.WebMethod]
        public static void AddPulloffSQL(string route,
            string pulloffmonth,
            string pulloffday,
            string pulloffyear,
            string pulloffhour,
            string pulloffminute,
            string returnhour,
            string returnminute)
        {
            HttpContext.Current.Response.ContentType = "application/json";

            SqlConnection con = new SqlConnection();

            //begin try sql connection and command to fill dropdown boxes
            try
            {
                string pulloffDateTime = pulloffyear + "-" + pulloffmonth + "-" + pulloffday + " " + pulloffhour + ":" + pulloffminute;
                string returnDateTime = pulloffyear + "-" + pulloffmonth + "-" + pulloffday + " " + returnhour + ":" + returnminute;

                DateTime pulloffTime = new DateTime(
                                                    int.Parse(pulloffyear),
                                                    int.Parse(pulloffmonth),
                                                    int.Parse(pulloffday),
                                                    int.Parse(pulloffhour),
                                                    int.Parse(pulloffminute),
                                                    0);
                DateTime returnTime = new DateTime(
                                                    int.Parse(pulloffyear),
                                                    int.Parse(pulloffmonth),
                                                    int.Parse(pulloffday),
                                                    int.Parse(returnhour),
                                                    int.Parse(returnminute),
                                                    0);

                // for pulloffs that span over to the next day
                if (pulloffTime > returnTime)
                {
                    returnTime = returnTime.AddDays(1);
                }

                string[] routeExploded = route.Split(' ');
                int i = routeExploded.Length - 1;

                string suffix = routeExploded[i];
                string runNum = routeExploded[--i];
                string routeNum = routeExploded[--i];
                string dayOfWeek = routeExploded[--i];
                string mode = routeExploded[--i];
                string routeName = "";

                for (int j = 0; j < i; j++)
                {
                    routeName = routeName + routeExploded[j] + " ";
                }
                routeName = routeName.Substring(0, routeName.Length - 1);

                //String query = "Insert into dbo.TestPulloffsDH values ('route_name', 'mode', 'DoW', 'route', 'date', 'run', 'suffix')";
                //String query = "Insert into dbo.TestPulloffsDH values ('SHOPPER', '01', 'F', '100', '2022-01-01 02:30:00', '01', '0')";
                // insert into TestPulloffs values ('SHOPPER', '01', 'F', '100', '2022-01-01 10:00', '2022-01-01 10:05' ,'01', '0');
                String query = "Insert into dbo.TestPulloffs (Route_Name, Mode, DoW, Route, PulloffTime, PulloffReturn, Run, Suffix) values (" +
                    "@routeName" + ", " +
                    "@mode" + ", " +
                    "@dayOfWeek" + ", " +
                    "@routeNum" + ", " +
                    "@pulloffTime"+ ", " +
                    "@returnTime" + ", " +
                    "@runNum" + ", " +
                    "@suffix )";

                string dbString = System.Configuration.ConfigurationManager.ConnectionStrings["MTATest"].ConnectionString;
                con = new SqlConnection(dbString);
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@routeName", routeName);
                cmd.Parameters.AddWithValue("@mode", mode);
                cmd.Parameters.AddWithValue("@dayOfWeek", dayOfWeek);
                cmd.Parameters.AddWithValue("@routeNum", routeNum);
                cmd.Parameters.AddWithValue("@pulloffTime", pulloffTime.ToString("yyyy - MM - dd HH: mm"));
                cmd.Parameters.AddWithValue("@returnTime", returnTime.ToString("yyyy - MM - dd HH: mm"));
                cmd.Parameters.AddWithValue("@runNum", runNum);
                cmd.Parameters.AddWithValue("@suffix", suffix);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                //HttpContext.Current.Response.Write(FillRoutePreview(route, pulloffmonth, pulloffday));

                HttpContext.Current.Response.Write("Added pulloff");
            }

            catch (Exception es)
            {
                //HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write("Error: " + es.Message);
            }
            finally
            {
                con.Close();
                //PulloffsList.BuildPulloffsArray(month: pulloffmonth);
            }

            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            //end try sql connection and command
        }
        // end AddPulloffSQL

        // -- Testing Adding a SQL Object from JS anonymous object call -- //
        //[System.Web.Services.WebMethod]
        //public static void AddPulloffObjectSQL(Pulloff pulloff)
        //{
        //    string pulloffyear = pulloff.PulloffTime.ToString("yyyy");
        //    string pulloffmonth = pulloff.PulloffTime.ToString("MM");
        //    string pulloffday = pulloff.PulloffTime.ToString("dd");
        //    string pulloffhour = pulloff.PulloffTime.ToString("HH");
        //    string pulloffminute = pulloff.PulloffTime.ToString("mm");

        //    string returnhour = pulloff.PulloffReturn.ToString("HH");
        //    string returnminute = pulloff.PulloffReturn.ToString("mm");

        //    HttpContext.Current.Response.ContentType = "application/json";

        //    //SqlConnection con = new SqlConnection();

        //    //begin try sql connection and command to fill dropdown boxes
        //    try
        //    {
        //        string pulloffDateTime = pulloffyear + "-" + pulloffmonth + "-" + pulloffday + " " + pulloffhour + ":" + pulloffminute;
        //        string returnDateTime = pulloffyear + "-" + pulloffmonth + "-" + pulloffday + " " + returnhour + ":" + returnminute;

        //        DateTime pulloffTime = new DateTime(
        //                                            int.Parse(pulloffyear),
        //                                            int.Parse(pulloffmonth),
        //                                            int.Parse(pulloffday),
        //                                            int.Parse(pulloffhour),
        //                                            int.Parse(pulloffminute),
        //                                            0);
        //        DateTime returnTime = new DateTime(
        //                                            int.Parse(pulloffyear),
        //                                            int.Parse(pulloffmonth),
        //                                            int.Parse(pulloffday),
        //                                            int.Parse(returnhour),
        //                                            int.Parse(returnminute),
        //                                            0);

        //        // for pulloffs that span over to the next day
        //        if (pulloffTime > returnTime)
        //        {
        //            returnTime = returnTime.AddDays(1);
        //        }

        //        string[] routeExploded = pulloff.Route.Split(' ');
        //        int i = routeExploded.Length - 1;

        //        string suffix = routeExploded[i];
        //        string runNum = routeExploded[--i];
        //        string routeNum = routeExploded[--i];
        //        string dayOfWeek = routeExploded[--i];
        //        string mode = routeExploded[--i];
        //        string routeName = "";

        //        for (int j = 0; j < i; j++)
        //        {
        //            routeName = routeName + routeExploded[j] + " ";
        //        }
        //        routeName = routeName.Substring(0, routeName.Length - 1);

        //        //String query = "Insert into dbo.TestPulloffsDH values ('route_name', 'mode', 'DoW', 'route', 'date', 'run', 'suffix')";
        //        //String query = "Insert into dbo.TestPulloffsDH values ('SHOPPER', '01', 'F', '100', '2022-01-01 02:30:00', '01', '0')";
        //        // insert into TestPulloffs values ('SHOPPER', '01', 'F', '100', '2022-01-01 10:00', '2022-01-01 10:05' ,'01', '0');
        //        String query = "Insert into dbo.TestPulloffs (Route_Name, Mode, DoW, Route, PulloffTime, PulloffReturn, Run, Suffix) values (" +
        //            "@routeName" + ", " +
        //            "@mode" + ", " +
        //            "@dayOfWeek" + ", " +
        //            "@routeNum" + ", " +
        //            "@pulloffTime" + ", " +
        //            "@returnTime" + ", " +
        //            "@runNum" + ", " +
        //            "@suffix )";

        //        //string dbString = System.Configuration.ConfigurationManager.ConnectionStrings["MTATest"].ConnectionString;
        //        //con = new SqlConnection(dbString);
        //        //SqlCommand cmd = new SqlCommand(query, con);
        //        //cmd.Parameters.AddWithValue("@routeName", routeName);
        //        //cmd.Parameters.AddWithValue("@mode", mode);
        //        //cmd.Parameters.AddWithValue("@dayOfWeek", dayOfWeek);
        //        //cmd.Parameters.AddWithValue("@routeNum", routeNum);
        //        //cmd.Parameters.AddWithValue("@pulloffTime", pulloffTime.ToString("yyyy - MM - dd HH: mm"));
        //        //cmd.Parameters.AddWithValue("@returnTime", returnTime.ToString("yyyy - MM - dd HH: mm"));
        //        //cmd.Parameters.AddWithValue("@runNum", runNum);
        //        //cmd.Parameters.AddWithValue("@suffix", suffix);
        //        //con.Open();
        //        //SqlDataReader reader = cmd.ExecuteReader();

        //        //HttpContext.Current.Response.Write(FillRoutePreview(route, pulloffmonth, pulloffday));

        //        HttpContext.Current.Response.Write("Test complete");
        //    }

        //    catch (Exception es)
        //    {
        //        HttpContext.Current.Response.Clear();
        //        HttpContext.Current.Response.Write("Error: " + es.Message);
        //    }
        //    finally
        //    {
        //        //con.Close();
        //        //PulloffsList.BuildPulloffsArray(month: pulloffmonth);
        //    }

        //    HttpContext.Current.Response.Flush();
        //    HttpContext.Current.Response.Close();
        //    //end try sql connection and command
        //}
        //// end AddPulloffSQL
        // -- End Test adding pulloff through Object from anonymous object JS call -- //
    }
}


