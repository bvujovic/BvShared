using Bv.Shared.WinForms;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace WinFormsTests
{
    public class BatteryStatusUnitTests
    {
        int[] calls { get; set; } = [90, 85, 80, 75, 70, 65, 60];

        [Theory]
        [InlineData(100, false)]
        [InlineData(99, false)]
        [InlineData(91, false)]
        [InlineData(90, true)]
        [InlineData(87, true)]
        [InlineData(85, true)]
        [InlineData(70, true)]
        public void BatteryLevelNotif_Call1(int level, bool expected)
        {
            BatteryStatus.Init(calls);
            var res = BatteryStatus.BatteryLevelNotif(level, false);
            Assert.Equal(expected, res);
        }

        [Theory]
        [InlineData(100, false, 99, false)]
        [InlineData(100, false, 90, true)]
        [InlineData(90, true, 89, false)]
        [InlineData(80, true, 70, true)]
        [InlineData(80, true, 78, false)]
        [InlineData(60, true, 55, false)]
        [InlineData(40, true, 30, false)]
        [InlineData(83, true, 83, false)]
        public void BatteryLevelNotif_Call2(int level1, bool expected1, int level2, bool expected2)
        {
            BatteryStatus.Init(calls);
            var res = BatteryStatus.BatteryLevelNotif(level1, false);
            Assert.Equal(expected1, res);
            res = BatteryStatus.BatteryLevelNotif(level2, false);
            Assert.Equal(expected2, res);
        }

        [Theory]
        [InlineData(80, 100, 90, true)]
        [InlineData(95, 100, 91, false)]
        [InlineData(95, 100, 90, true)]
        [InlineData(80, 100, 50, true)]
        [InlineData(40, 100, 50, true)]
        public void BatteryLevelNotif_Charging(int level1, int level2, int level3, bool expected)
        {
            BatteryStatus.Init(calls);
            BatteryStatus.BatteryLevelNotif(level1, false);              // discharge
            BatteryStatus.BatteryLevelNotif(level2, true);               // charge
            var res = BatteryStatus.BatteryLevelNotif(level3, false);    // discharge
            Assert.Equal(expected, res);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1861:Avoid constant arrays as arguments", Justification = "<Pending>")]
        public static TheoryData<int[]> CorrectLevelsData => new() {
                { new[] { 100, 99, 95 } },
                { new[] { 85, 65 } },
                { new[] { 50, 25 } },
                { new[] { 50 } },
                { new[] { 0 } },
                { new[] { 100 } },
            };

        [Theory]
        [MemberData(nameof(CorrectLevelsData))]
        public void Init_WorksCorrectly(int[] levels)
        {
            var ex = Record.Exception(() =>
                BatteryStatus.Init(levels));
            Assert.Null(ex);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1861:Avoid constant arrays as arguments", Justification = "<Pending>")]
        public static TheoryData<int[]> IncorrectLevelsData => new() {
                { Array.Empty<int>() },
                { new[] { 1000 } },
                { new[] { 101 } },
                { new[] { -1 } },
                { new[] { -100 } },
                { new[] { 1, 2 } },
                { new[] { 2, 2 } },
                { new[] { 2, 3, 1} },
                { new[] { 2, 3, 3 } },
                { new[] { 25, 50 } },
                { new[] { 80, 90, 100 } },
            };

        [Theory]
        [MemberData(nameof(IncorrectLevelsData))]
        public void Init_ThrowsException(int[] levels)
        {
            Assert.Throws<ArgumentException>(() =>
                BatteryStatus.Init(levels));
        }
    }
}
