using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;

namespace MTAIntranet
{
    /// <summary>
    /// A class representation of the Master Route entry in the Database
    /// </summary>
    public class MasterRoute
    {
        // begin Properties
        #region Properties
        //public int MasterRouteID { get; private set; }
        public string Route_Name { get; set; }
        public string Mode { get; set; }
        public string Day_Of_Week { get; set; }
        public string Route { get; set; }
        public string Run { get; set; }
        public string Suffix { get; set; }
        /// <summary>
        /// Route_Info consists of Route_Name, Mode, Day_Of_Week, Route, Run, and Suffix
        /// </summary>
        public string Route_Info
        {
            get
            {
                return Route_Name + " " +
                       Mode + " " +
                       Day_Of_Week + " " +
                       Route + " " +
                       Run + " " +
                       Suffix;
            }
        }
        public DateTime Pull_Out_Time { get; set; }
        public DateTime Pull_In_Time { get; set; }
        public decimal Beg_DH_Miles { get; set; }
        public decimal End_DH_Miles { get; set; }
        private List<Pulloff> matches;
        public List<Pulloff> Matches
        {
            get
            {
                if(matches == null)
                {
                    matches = new List<Pulloff>();
                }
                return matches;
            }
        }
        #endregion
        // end attributes

        // begin constructors
        #region constructors

        
        public MasterRoute() { } //parameterless constructor for serialization
        public MasterRoute(
            string route_name,
            string mode,
            string day_of_week,
            string route,
            string run,
            string suffix,
            DateTime pull_out_time,
            DateTime pull_in_time,
            decimal beg_dh_miles,
            decimal end_dh_miles)
        {
            Route_Name = route_name;
            Mode = mode;
            Day_Of_Week = day_of_week;
            Route = route;
            Run = run;
            Suffix = suffix;

            Pull_Out_Time = new DateTime(
                DateTime.Now.Year,
                pull_out_time.Month,
                pull_out_time.Day,
                pull_out_time.Hour,
                pull_out_time.Minute,
                pull_out_time.Second,
                00);

            Pull_In_Time = new DateTime(
                DateTime.Now.Year,
                pull_in_time.Month,
                pull_in_time.Day,
                pull_in_time.Hour,
                pull_in_time.Minute,
                pull_in_time.Second,
                00);

            if (pull_in_time.Day < pull_out_time.Day)
            {
                Pull_In_Time = new DateTime(
                pull_out_time.AddDays(1).Year,
                pull_out_time.AddDays(1).Month,
                pull_out_time.AddDays(1).Day,
                pull_in_time.Hour,
                pull_in_time.Minute,
                pull_in_time.Second,
                00);
            }

            this.Beg_DH_Miles = beg_dh_miles;
            this.End_DH_Miles = end_dh_miles;
        }

        public MasterRoute(MasterRoute mr, DateTime dateIterator) 
        {
            Route_Name = mr.Route_Name;
            Mode = mr.Mode;
            Day_Of_Week = mr.Day_Of_Week;
            Route = mr.Route;
            Run = mr.Run;
            Suffix = mr.Suffix;
            Pull_Out_Time = new DateTime(year: dateIterator.Year,
                month: dateIterator.Month,
                day: dateIterator.Day,
                hour: mr.Pull_Out_Time.Hour,
                minute: mr.Pull_Out_Time.Minute,
                second: mr.Pull_Out_Time.Second);
            Pull_In_Time = new DateTime(year: dateIterator.Year,
                month: dateIterator.Month,
                day: dateIterator.Day,
                hour: mr.Pull_In_Time.Hour,
                minute: mr.Pull_In_Time.Minute,
                second: mr.Pull_In_Time.Second);
            Beg_DH_Miles = mr.Beg_DH_Miles;
            End_DH_Miles = mr.End_DH_Miles;

        }
        #endregion
        // end constructors

        // begin class methods
        #region methods

        /// <summary>
        /// Returns a concatenation of Route_Info, Month, and Day of Route
        /// </summary>
        /// <returns></returns>
        public string GetSignature()
        {
            return Route_Info + " " + Pull_Out_Time.ToString("MM") + " " + 
                Pull_Out_Time.ToString("dd");
        }

        /// <summary>
        /// Adds the pulloff that matches the signature of a Route
        /// </summary>
        /// <param name="p"></param>
        public void AddMatch(Pulloff p)
        {
            if (!(Matches.Contains(p)))
            {
                Matches.Add(p);
            }
        }

        /// <summary>
        /// Returns the match at position passed or null
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Pulloff GetMatch(int position)
        {
            if (Matches == null || Matches.Count == 0 || position > Matches.Count - 1)
            {
                return null;
            }
            return Matches[position];
        }

        /// <summary>
        /// Returns the latest pulloff's return time for 
        /// earliest available future pulloff entry or
        /// start time if no pulloffs exist
        /// </summary>
        /// <returns></returns>
        public DateTime GetEarliestAvailableTime()
        {
            return Matches.Count > 0 ? 
                Matches[Matches.Count - 1].PulloffReturn : 
                Pull_Out_Time;
        }

        /// <summary>
        /// Writes the values of a route and its pulloff(s) to the http response
        /// in a table
        /// </summary>
        /// <param name="showDelete"></param>
        public void RespondRouteAndPulloffs(bool showDelete = true)
        {
            // headers already written

            HttpResponse response = HttpContext.Current.Response;

            //write base date, dow, route name, route, run, pullout time
            for (int i = 0; i < this.Matches.Count; i++)
            {
                if (i == 0)
                {
                    response.Write("<tr>");
                    response.Write("<td>" + 
                        this.Pull_Out_Time.ToString("yyyy-MM-dd") + "</td>");
                    response.Write("<td>" + 
                        this.Day_Of_Week + "</td>");
                    response.Write("<td>" + 
                        this.Route_Name + "</td>");
                    response.Write("<td>" + 
                        this.Route + "</td>");
                    response.Write("<td>" + 
                        this.Run + "</td>");
                    response.Write("<td>" + 
                        this.Pull_Out_Time.ToString("HH:mm") + "</td>");
                    response.Write("<td>" + 
                        this.GetMatch(i).PulloffTime.ToString("HH:mm") + "</td>");
                    response.Write("<td>" + 
                        this.Mode + "</td>");
                    response.Write("<td>" + 
                        this.Suffix + "</td>");
                    response.Write("<td>" + 
                        this.Beg_DH_Miles + "</td>");
                    response.Write("<td>" + 
                        this.End_DH_Miles + "</td>");

                    response.Write("<td class=\"pulloff\">" + 
                        this.GetMatch(i).DoW + "</td>");
                    response.Write("<td class=\"pulloff\">" + 
                        this.GetMatch(i).Route_Name + "</td>");
                    response.Write("<td class=\"pulloff\">" + 
                        this.GetMatch(i).Route + "</td>");
                    response.Write("<td class=\"pulloff\">" + 
                        this.GetMatch(i).Run + "</td>");
                    response.Write("<td class=\"pulloff\">" + 
                        this.GetMatch(i).PulloffReturn.ToString("HH:mm") + "</td>");
                    response.Write("<td class=\"pulloff\">" +
                        (this.GetMatch(i + 1) == null ?
                            this.Pull_In_Time.ToString("HH:mm") + "</td>" :
                            this.GetMatch(i + 1).PulloffTime.ToString("HH:mm") + "</td>"));
                    response.Write("<td class=\"pulloff\">" + 
                        this.GetMatch(i).Mode + "</td>");
                    response.Write("<td class=\"pulloff\">" + 
                        this.GetMatch(i).Suffix + "</td>");
                    if (showDelete)
                    {
                        response.Write(
                            "<td class=\"pulloff\"><button type=\"button\" " +
                            "class=\"delpulloff\" name=\"" + 
                            this.GetMatch(i).PulloffID + "\"> Delete</td>");
                    }
                }
                else
                {
                    response.Write("<tr>");
                    response.Write("<td>-</td><td>-</td><td>" +
                        "(Pulloff " + 
                        (i + 1) + ")</td><td>-</td><td>-</td><td>-" +
                        "</td><td>-</td><td>-</td><td>-" +
                        "</td><td>-</td><td>-</td>");
                    response.Write("<td class=\"pulloff\">" + 
                        this.GetMatch(i).DoW + "</td>");
                    response.Write("<td class=\"pulloff\">" + 
                        this.GetMatch(i).Route_Name + "</td>");
                    response.Write("<td class=\"pulloff\">" + 
                        this.GetMatch(i).Route + "</td>");
                    response.Write("<td class=\"pulloff\">" + 
                        this.GetMatch(i).Run + "</td>");
                    response.Write("<td class=\"pulloff\">" + 
                        this.GetMatch(i).PulloffReturn.ToString("HH:mm") + "</td>");
                    response.Write("<td class=\"pulloff\">" +
                        (this.GetMatch(i + 1) == null ?
                            this.Pull_In_Time.ToString("HH:mm") + "</td>" :
                            this.GetMatch(i + 1).PulloffTime.ToString("HH:mm") + "</td>"));
                    response.Write("<td class=\"pulloff\">" + 
                        this.GetMatch(i).Mode + "</td>");
                    response.Write("<td class=\"pulloff\">" + 
                        this.GetMatch(i).Suffix + "</td>");
                    if (showDelete)
                    {
                        response.Write("<td class=\"pulloff\">" +
                            "<button type=\"button\" class=\"delpulloff\" name=\"" +
                            this.GetMatch(i).PulloffID + "\"> Delete</td>");
                    }
                }
            }
            response.Write("</tr>");
        }
        #endregion
        // end class methods
    }
}