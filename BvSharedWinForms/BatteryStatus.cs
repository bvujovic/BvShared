namespace Bv.Shared.WinForms
{
    /// <summary>
    /// Static class that provides information about the laptop battery status.
    /// </summary>
    public static class BatteryStatus
    {
        private static PowerStatus Status => SystemInformation.PowerStatus;

        /// <summary>Is the laptop battery currently charging</summary>
        public static bool IsCharging => Status.PowerLineStatus == PowerLineStatus.Online;

        /// <summary>Estimated minutes of battery life remaining</summary>
        public static int MinutesRemaining => Status.BatteryLifeRemaining / 60;

        /// <summary>Percent of battery life remaining [0..100]</summary>
        public static int BatteryLevel => (int)(Status.BatteryLifePercent * 100);

        public new static string ToString()
        {
            return (Status.BatteryLifePercent * 100) + "%, " +
                (IsCharging ? "charging..." : $"Remaining: {MinutesRemaining / 60}h {MinutesRemaining % 60}min");
        }

        /// <summary>Battery levels at which user will be notified - BatteryLevelNotif() will return true.</summary>
        private static int[] BatteryLevelNotifs { get; set; } = [90, 85, 80, 75, 70, 65, 60];

        private static int idxBatteryLevelNotifs = 0;

        /// <summary>Does user need to be notified about battery level change</summary>
        public static bool BatteryLevelNotif()
        {
            return BatteryLevelNotif(-1, null);
        }

        /// <summary>Does user need to be notified about battery level change</summary>
        internal static bool BatteryLevelNotif(int batteryLevel, bool? isCharging)
        {
            if (batteryLevel == -1)
                batteryLevel = BatteryLevel;
            if (!isCharging.HasValue)
                isCharging = IsCharging;
            if (isCharging.Value)
            {
                Charging(batteryLevel);
                return false;
            }
            else
                return Discharging(batteryLevel);
        }

        private static bool Discharging(int batteryLevel)
        {
            var res = false;
            while (batteryLevel <= BatteryLevelNotifs[idxBatteryLevelNotifs])
                if (idxBatteryLevelNotifs < BatteryLevelNotifs.Length - 1)
                {
                    idxBatteryLevelNotifs++;
                    res = true;
                }
                else
                    break;
            return res;
        }

        private static void Charging(int batteryLevel)
        {
            while (batteryLevel >= BatteryLevelNotifs[idxBatteryLevelNotifs] && idxBatteryLevelNotifs > 0)
                idxBatteryLevelNotifs--;

            //while (batteryLevel >= BatteryLevelCalls[idxBatteryLevelCalls])
            //if (idxBatteryLevelCalls > 0)
            //    idxBatteryLevelCalls--;
            //else
            //    break;
        }

        public static void Init(int[] batteryLevelNotifs)
        {
            BatteryLevelNotifs = batteryLevelNotifs;
            //TODO check if elements of (non empty) array are [0..100] in descending order
            idxBatteryLevelNotifs = 0;
        }
    }
}
