namespace GEMC.Common
{
    public class Data
    {
        // TODO : Add difference from previous car (laps or cars)

        public int Color { get; set; }

        public int Index { get; set; }

        public int Vehicle { get; set; }

        public int PilotNumber { get; set; }

        public int? Transponder { get; set; }

        public string Pilot { get; set; }

        public string Initials { get; set; }

        public string Club { get; set; }

        public string Country { get; set; }

        public string LapInfo { get; set; }

        public string Trend { get; set; }

        public int Laps { get; set; }

        public string LapTime { get; set; }

        public string AbsoluteTime { get; set; }

        public string BestTime { get; set; }

        public string BestTimeN { get; set; }

        public string MediumTime { get; set; }

        public string Forecast { get; set; }

        public string Progress { get; set; }

        public string DelayTimeFirst { get; set; }

        public string DelayTimePrevious { get; set; }

        public string StdDeviation { get; set; }

        public string CardId { get; set; }

        public string Voltage { get; set; }

        public string Temperature { get; set; }

        public string Speed { get; set; }
    }
}