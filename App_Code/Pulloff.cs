using System;
using System.Web.Routing;

namespace MTAIntranet
{
    /// <summary>
    /// Summary description for Pulloff:
    /// This class represents a single entry in the TestPulloffs database
    /// on FLTAS003.
    /// </summary>
    public class Pulloff
    {
        // begin Properties
        #region Properties

        public int PulloffID { get; set; }
        public string Route_Name { get; set; }
        public string Mode { get; set; }
        public string DoW { get; set; }
        public string Route { get; set; }
        public DateTime PulloffTime { get; set; }
        public DateTime PulloffReturn { get; set; }
        public string Run { get; set; }
        public string Suffix { get; set; }

        public string RouteInfo 
        {
            get
            {
                return Route_Name + " " +
                       Mode + " " +
                       DoW + " " +
                       Route + " " +
                       Run + " " +
                       Suffix;
            }
        }
        #endregion
        // end Properties

        // begin constructors
        #region constructors

        public Pulloff() { } // parameterless, empty contructor for serialization

        public Pulloff(int pulloff_ID,
            string route_Name,
            string mode,
            string doW,
            string route,
            DateTime pulloffTime,
            DateTime pulloffReturn,
            string run,
            string suffix)
        {
            PulloffID = pulloff_ID;
            Route_Name = route_Name;
            Mode = mode;
            DoW = doW;
            Route = route;
            PulloffTime = pulloffTime;
            PulloffReturn = pulloffReturn;
            Run = run;
            Suffix = suffix;
        }

        #endregion
        // end constructors

        // begin class methods
        #region methods

        /// <summary>
        /// override equals for use with "Contains()" method
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object obj)
        {
            // if compared object is of a different type than Pulloff, return false
            // otherwise compare the pulloff id's to determine if they are equal
            if (obj == null || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                // compare pulloff id's
                Pulloff p = (Pulloff)obj;
                return p.PulloffID ==
                    this.PulloffID;
            }
        }
        /// <summary>
        /// Required GetHashCode function to join "Equals" override
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Uses the RouteInfo, start and end time to uniquely identify a pulloff
        /// </summary>
        /// <returns></returns>
        public string GetSignature()
        {
            //return this.route_name + this.mode + this.dow + this.route + this.run + this.suffix + this.getPullOutTime().ToString("MM") + this.getPullOutTime().ToString("dd");
            //return Route_Name + this.Mode + this.DoW + this.Route + this.Run + this.Suffix + this.getPulloffTime().ToString("MM") + this.getPulloffTime().ToString("dd");
            return this.RouteInfo + " " + this.PulloffTime.ToString("MM") + " " + this.PulloffTime.ToString("dd");
        }
        #endregion
        // end class methods

        #region settersGetters
        // begin setters and getters
        // end setters and getters
        #endregion
    }
    // end class Pulloff
}