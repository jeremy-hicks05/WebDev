using System;

/// <summary>
/// Summary description for Utilities
/// </summary>

namespace MTAIntranet
{
    /// <summary>
    /// Summary for Utilities class
    /// </summary>
    public static class Utilities
    {
        //#warning this is a test warning
        /// <summary>
        /// Summary for ConvertToDoW
        /// </summary>
        /// <param name="myDate"></param>
        /// <returns></returns>
        public static string ConvertToDoW(string myDate)
        {
            if (myDate == "Thursday")
            {
                return "H";
            }
            else if (myDate == "Sunday")
            {
                return "Y";
            }
            else
            {
                return myDate.Substring(0, 1);
            }
        }

        /// <summary>
        /// Summary for DoWMatch
        /// </summary>
        /// <param name="mr"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static bool DoWMatch(MasterRoute mr, DateTime date2)
        {
            string dtDoW = ConvertToDoW(date2.DayOfWeek.ToString());
            return (mr.Day_Of_Week == dtDoW) || 
                (mr.Day_Of_Week == "D" && (dtDoW == "M" ||
                dtDoW == "T" ||
                dtDoW == "W" ||
                dtDoW == "H" ||
                dtDoW == "F"));
        }
    }
}