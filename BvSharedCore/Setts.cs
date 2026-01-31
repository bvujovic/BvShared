using System.Data;

namespace Bv.Shared.Core
{
    /// <summary>
    /// Provides static methods for initializing and accessing application settings stored in a DataTable.
    /// Supports reading and writing settings as integer, boolean, and string values by name.
    /// </summary>
    /// <remarks>The Setts class must be initialized by calling Init with a valid DataTable before any read or
    /// write operations are performed. All members are static and thread safety is not guaranteed.
    /// Settings DataTable has to have 2 string columns named 'Name' (unique) and 'Value'.
    /// </remarks>
    public static class Setts
    {
        /// <summary>Initialize Setts with Settings data table (from DataSet).</summary>
        public static void Init(DataTable settings)
        {
            Settings = settings;
        }

        private static DataRowCollection Rows
        {
            get
            {
                CheckInitCall();
                return Settings!.Rows;
            }
        }

        private static void CheckInitCall()
        {
            if (Settings == null)
                throw new InvalidOperationException("Setts is not initialized. Call Setts.Init() first.");
        }

        private static DataTable? Settings;

        /// <summary>Find Settings row by the name of the setting.</summary>
        private static DataRow? FindByName(string Name)
        {
            //return Rows.Find(new object[] { Name });
            return Rows?.Find([Name]);
        }

        private static string GetValue(DataRow row)
            => (string)row["Value"];

        /// <summary>Read int setting by name.</summary>
        public static int ReadInt(string name, int defValue, Func<int, bool>? checkMethod = null)
        {
            var sett = FindByName(name);
            if (sett != null)
            {
                //var val = int.Parse(s.Value);
                //var val = int.Parse((string)s["Value"]);
                var val = int.Parse(GetValue(sett));
                if (checkMethod == null)
                    return val;
                else
                    return checkMethod(val) ? val : defValue;
            }
            return defValue;
        }

        /// <summary>Read bool setting by name.</summary>
        public static bool ReadBool(string name, bool defValue)
        {
            var sett = FindByName(name);
            if (sett != null)
                return bool.Parse(GetValue(sett));
            return defValue;
        }

        /// <summary>Read string setting by name.</summary>
        public static string? ReadString(string name, string? defValue = null)
        {
            var sett = FindByName(name);
            if (sett != null)
                return GetValue(sett);
            return defValue;
        }

        //public static DateTime ReadDateTime(string name, DateTime defValue)
        //{
        //    var s = Ds.Settings.FindByName(name);
        //    if (s != null)
        //        return DateTime.Parse(s.Value);
        //    return defValue;
        //}

        /// <summary>Write name of the setting with the string representation of its value.</summary>
        public static void WriteValue(string name, string? value)
        {
            var sett = FindByName(name);
            if (sett == null)
            {
                CheckInitCall();
                sett = Settings!.NewRow();
                sett["Name"] = name;
            }
            if (value != null)
            {
                sett["Value"] = value;
                if (sett.RowState == DataRowState.Detached)
                    //AddSettingsRow(sett);
                    Rows.Add(sett);
            }
            ////? this code might not be necessary
            //else if (sett.RowState != DataRowState.Detached)
            //    RemoveSettingsRow(sett);
        }
    }
}
