using System;

namespace GEMC.Common
{
    public class Time : IEquatable<Time>
    {
        public Time(string text)
        {
            if (text.StartsWith("-"))
            {
                this.IsNegative = true;
                text = text.Substring(1);
            }

            string[] tokens = text.Split(':');

            this.Seconds = int.Parse(tokens[2]);
            this.Minutes = int.Parse(tokens[1]);
            this.Hours = int.Parse(tokens[0]);
        }

        public bool IsNegative { get; set; }

        public int Hours { get; set; }

        public int Minutes { get; set; }

        public int Seconds { get; set; }

        public Time Max => new Time("99:99:99");

        public Time Now()
        {
            string now = DateTime.Now.ToString("HH:mm:ss"); 
            return new Time(now);
        }

        public bool Equals(Time other)
        {
            if (other == null) return false;
            return IsNegative == other.IsNegative && Hours == other.Hours && Minutes == other.Minutes && Seconds == other.Seconds;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Time) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = IsNegative.GetHashCode();
                hashCode = (hashCode * 397) ^ Hours;
                hashCode = (hashCode * 397) ^ Minutes;
                hashCode = (hashCode * 397) ^ Seconds;
                return hashCode;
            }
        }
    }
}