using System;
using System.Collections.Generic;
using System.Web;

namespace MTAIntranet
{
    /// <summary>
    /// Summary of MasterRouteRepository class
    /// </summary>
    public static class MasterRouteRepository
    {
        /// <summary>
        /// The 2D array of Dictionaries that holds all routes for a year
        /// </summary>
        public static Dictionary<string, MasterRoute>[][] MasterRouteDictionary;

        // begin static functions
        #region functions
        /// <summary>
        /// Builds the Master Route Dictionary for a specific month
        /// using the MasterRoute Template
        /// </summary>
        /// <param name="month"></param>
        public static void BuildMasterRouteDictionary(string month)
        {
            MasterRouteTemplateRepository.BuildMasterRouteListTemplate();

            ClearMasterRouteDictionary();

            // build rest of milemaster list for entire year using template from parameter
            for (DateTime dateIterator = new DateTime(DateTime.Now.Year, int.Parse(month), 1); 
                    dateIterator.Month == int.Parse(month); 
                    dateIterator = dateIterator.AddDays(1))
            {
                foreach (MasterRoute mr in MasterRouteTemplateRepository.MasterRouteTemplate)
                {
                    if (Utilities.DoWMatch(mr, dateIterator))
                    {
                        MasterRoute mileMasterEntry =
                            new MasterRoute(mr, dateIterator);

                        // add MasterRoute object to MileMaster list
                        MasterRouteDictionary
                            [mileMasterEntry.Pull_Out_Time.Month - 1]
                            [mileMasterEntry.Pull_Out_Time.Day - 1]
                                .Add(mileMasterEntry.GetSignature(), mileMasterEntry);
                    }
                }
            }
            CallBuildPulloffs(month);
        }

        public static void ClearMasterRouteDictionary()
        {
            if (MasterRouteDictionary == null)
            {
                MasterRouteDictionary = new Dictionary<string, MasterRoute>[12][];

                for (int i = 0; i < 12; i++)
                {
                    int daysInMonth = DateTime.DaysInMonth(DateTime.Today.Year, i + 1);

                    MasterRouteDictionary[i] = new Dictionary<string, MasterRoute>[daysInMonth];

                    for (int j = 0; j < daysInMonth; j++)
                    {
                        MasterRouteDictionary[i][j] = new Dictionary<string, MasterRoute>();
                    }
                }
            }
            else
            {
                for (int i = 0; i < 12; i++)
                {
                    int daysInMonth = DateTime.DaysInMonth(DateTime.Today.Year, i + 1);

                    for (int j = 0; j < daysInMonth; j++)
                    {
                        MasterRouteDictionary[i][j].Clear();
                    }
                }
            }
        }

        /// <summary>
        /// Calls the Pulloffs.BuildPulloffsArray function
        /// </summary>
        /// <param name="month"></param>
        private static void CallBuildPulloffs(string month)
        {
            PulloffsRepository.BuildPulloffsArray(month);
        }

        /// <summary>
        /// Returns list of MasterRoutes that have at least One (1) pulloff
        /// for a specific month
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static List<MasterRoute> GetRouteListWithPulloffsForMonth(string month)
        {
            int intMonth = int.Parse(month) - 1;
            int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, int.Parse(month));
            List<MasterRoute> myRouteList = new List<MasterRoute>();

            //MasterRouteRepository.BuildMasterRouteDictionary(month);
            //PulloffsRepository.BuildPulloffsArray(month);
            //PulloffsRepository.MatchAllPulloffs(month);

            for (int j = 0; j < daysInMonth; j++)
            {
                foreach (KeyValuePair<string, MasterRoute> masterRoute in MasterRouteDictionary[intMonth][j])
                //foreach (KeyValuePair<string, MasterRoute> masterRoute in GetRouteDictionary(month, (j + 1).ToString()))
                {
                    if (masterRoute.Value.Matches.Count > 0)
                    {
                        //yield return masterRoute.Value;
                        myRouteList.Add(masterRoute.Value);
                    }
                }
            }
            return myRouteList;
        }

        /// <summary>
        /// Gets a dictionary of master routes for a specific month and day
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static Dictionary<string, MasterRoute> GetRouteDictionary(string month, string day)
        {
            int intMonth = int.Parse(month) - 1;
            int intDay = int.Parse(day) - 1;
            BuildMasterRouteDictionary(month);

            return MasterRouteDictionary[intMonth][intDay];
        }

        /// <summary>
        /// Returns dictionary of master routes for a specific month and day
        /// only if it has any pulloffs
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static Dictionary<string, MasterRoute> GetRouteDictionaryWithPulloffs(string month, string day)
        {
            int intMonth = int.Parse(month) - 1;
            int intDay = int.Parse(day) - 1;
            bool hasMatches = false;

            BuildMasterRouteDictionary(month);

            foreach (KeyValuePair<string, MasterRoute> masterRoute in MasterRouteDictionary[intMonth][intDay])
            {
                if (masterRoute.Value.Matches.Count != 0)
                {
                    hasMatches = true;
                }
            }
            if (hasMatches)
            {
                return MasterRouteDictionary[intMonth][intDay];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns single Master Route using routeinfo, month, and day
        /// to find a unique match
        /// </summary>
        /// <param name="routeinfo"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static MasterRoute GetRoute(string routeinfo, string month, string day)
        {
            int intMonth = int.Parse(month) - 1;
            int intDay = int.Parse(day) - 1;

            return MasterRouteDictionary[intMonth][intDay][routeinfo + " " + month + " " + day];
        }

        /// <summary>
        /// Writes headers and rows filled with data from list of master routes
        /// </summary>
        /// <param name="masterRoutes"></param>
        public static void WriteRouteTable(List<MasterRoute> masterRoutes)
        {
            WriteHeadersResponse();
            // build and write response
            foreach (MasterRoute masterRoute in masterRoutes)
            {
                masterRoute.RespondRouteAndPulloffs();
            }
            HttpContext.Current.Response.Write("</table>");
            HttpContext.Current.Response.Flush();
            //HttpContext.Current.Response.Close();
            // end build and write response
        }

        /// <summary>
        /// Writes the pulloffs for a single MasterRoute
        /// </summary>
        /// <param name="masterRoute"></param>
        public static void WritePulloffsFor(MasterRoute masterRoute)
        {
            BuildMasterRouteDictionary(masterRoute.Pull_Out_Time.Month.ToString());
            //PulloffsList.BuildPulloffsArray(masterRoute.Pull_Out_Time.Month.ToString());
            if (masterRoute.Matches.Count != 0)
            {
                WriteHeadersResponse(showDelete: false);
                masterRoute.RespondRouteAndPulloffs(showDelete: false);
                HttpContext.Current.Response.Write("</table>");
            }
            else
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write("No pulloffs found for " + masterRoute.Route_Info);
            }

            HttpContext.Current.Response.Flush();
            //HttpContext.Current.Response.Close();

            // end build and write response
        }

        /// <summary>
        /// Writes the header information for the table
        /// per the 
        /// </summary>
        /// <param name="showDelete"></param>
        public static void WriteHeadersResponse(bool showDelete = true)
        {
            /// <summary>
            /// Headers used for first section of table display
            /// </summary>
            string[] firstHeaders = {
                "Date",
                "DoW",
                "RouteName",
                "Route",
                "Run",
                "Pull out Time",
                "Pulloff Time",
                "Mode",
                "Suffix",
                "Beg DH",
                "End DH"
            };

            /// <summary>
            /// Headers used for second section of table display
            /// </summary>
            string[] secondHeaders =
            {
                "DoW",
                "RouteName",
                "Route",
                "Run",
                "Return Time",
                "Pulloff/End Time",
                "Mode",
                "Suffix"
            };

            HttpResponse response = HttpContext.Current.Response;

            response.Write("<table id='pulloffresults'>");
            response.Write("<caption>Pulloff Results</caption>");
            response.Write("<tr class=\"tableheaders\">");
            foreach (string header in firstHeaders)
            {
                response.Write("<th>" + header + "</th>");
            }

            foreach (string header in secondHeaders)
            {
                response.Write("<th>" + header + "</th>");
            }
            if (showDelete)
            {
                response.Write("<th>Delete</th>");
            }
            response.Write("</tr>");
        }
        #endregion
        // end static functions
    }
}