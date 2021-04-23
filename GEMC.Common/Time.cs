namespace GEMC.Common
{
    using System;

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

            if (tokens.Length != 3)
            {
                throw new FormatException($"Arguments are not valid.");
            }

            this.Seconds = int.Parse(tokens[2]);
            this.Minutes = int.Parse(tokens[1]);
            this.Hours = int.Parse(tokens[0]);

            if (this.Seconds > 60 || this.Minutes > 60 || this.Hours > 24)
            {
                throw new ArgumentOutOfRangeException($"Arguments are not valid.");
            }
        }

        public Time(bool isNegative, int hours, int minutes, int seconds)
        {
            IsNegative = isNegative;
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
        }

        public static Time MaxValue => new Time("24:60:60");

        public Time AbsoluteValue => new Time(false, this.Hours, this.Minutes, this.Seconds);

        public bool IsNegative { get; set; }

        public int Hours { get; set; }

        public int Minutes { get; set; }

        public int Seconds { get; set; }
        
        public static Time Now()
        {
            string now = DateTime.Now.ToString("HH:mm:ss");
            return new Time(now);
        }

        public static Time GetBiggestAbsolute(Time a, Time b)
        {
            if (a.AbsoluteValue < b.AbsoluteValue)
            {
                return b;
            }

            return a;
        }

        public static Time operator +(Time a, Time b)
        {
            int total;
            int seconds;
            int minutes;
            int hours;
            int additionalMinutes;
            int additionalHours;
            int additionalDays;

            // Addition of positive values : normal additions in specific bases
            if (!a.IsNegative && !b.IsNegative)
            {
                total = a.Seconds + b.Seconds;
                additionalMinutes = Math.DivRem(total, 60, out seconds);

                total = a.Minutes + b.Minutes + additionalMinutes;
                additionalHours = Math.DivRem(total, 60, out minutes);

                total = a.Hours + b.Hours + additionalHours;
                additionalDays = Math.DivRem(total, 60, out hours);

                if (additionalDays > 0)
                {
                    throw new OverflowException("Result is more than one day.");
                }

                return new Time(false, hours, minutes, seconds);
            }

            // Addition of negative values : normal additions in specific bases, add negative at the end
            if (a.IsNegative && b.IsNegative)
            {
                total = a.Seconds + b.Seconds;
                additionalMinutes = Math.DivRem(total, 60, out seconds);

                total = a.Minutes + b.Minutes + additionalMinutes;
                additionalHours = Math.DivRem(total, 60, out minutes);

                total = a.Hours + b.Hours + additionalHours;
                additionalDays = Math.DivRem(total, 60, out hours);

                if (additionalDays > 0)
                {
                    throw new OverflowException("Result is more than one day.");
                }

                return new Time(true, hours, minutes, seconds);
            }

            // a>0,b<0,[a]>[b] -> [a]-[b], IsNegative = [a]>[b]
            //  01:01:14
            // -00:12:21
            // ---------
            //  00:48:53 

            // a<0,b>0,[a]>[b] -> [a]-[b], IsNegative = [a]>[b]
            // -01:01:14
            //  00:12:21
            // ---------
            // -00:48:53
            if (a.AbsoluteValue >= b.AbsoluteValue)
            {
                // Subtraction
                int provision = 0;
                while (a.Seconds < b.Seconds)
                {
                    a.Seconds = a.Seconds + 60;
                    provision--;
                }

                seconds = a.Seconds - b.Seconds;

                a.Minutes = a.Minutes + provision;
                provision = 0;
                while (a.Minutes < b.Minutes)
                {
                    a.Minutes = a.Minutes + 60;
                    provision--;
                }

                minutes = a.Minutes - b.Minutes;
                a.Hours = a.Hours + provision;
                hours = a.Hours - b.Hours;

                // IsNegative computation
                bool isNegative = GetBiggestAbsolute(a, b).IsNegative;

                return new Time(isNegative, hours, minutes, seconds);
            }

            // a>0,b<0,[a]<[b] -> [b]-[a], IsNegative = true
            //  00:08:14
            // -01:12:21
            // ---------
            // -01:04:07 

            // a<0,b>0,[a]<[b] -> [b]-[a], IsNegative = false
            // -01:01:14
            //  02:08:07
            // ---------
            //  01:06:53
            if (a.AbsoluteValue < b.AbsoluteValue)
            {
                // Subtraction
                int provision = 0;

                while (b.Seconds < a.Seconds)
                {
                    b.Seconds = b.Seconds + 60;
                    provision--;
                }

                seconds = b.Seconds - a.Seconds;

                b.Minutes = b.Minutes + provision;
                provision = 0;

                while (b.Minutes < a.Minutes)
                {
                    b.Minutes = b.Minutes + 60;
                    provision--;
                }

                minutes = b.Minutes - a.Minutes;
                b.Hours = b.Hours + provision;
                hours = b.Hours - a.Hours;

                // IsNegative computation
                bool isNegative = GetBiggestAbsolute(a, b).IsNegative;

                return new Time(isNegative, hours, minutes, seconds);
            }

            throw new NotImplementedException("Seems I did forget a case...");
        }

        public static Time operator -(Time a, Time b)
        {
            b.IsNegative = !b.IsNegative;
            return a + b;
        }

        public static Time operator *(Time a, Time b)
        {
            throw new NotImplementedException("Multiplication is not supported");
        }

        public static Time operator /(Time a, Time b)
        {
            throw new NotImplementedException("Division is not supported");
        }

        public static Time operator %(Time a, Time b)
        {
            throw new NotImplementedException("Division is not supported");
        }

        public static bool operator <(Time a, Time b)
        {
            if (a.Equals(b))
            {
                return false;
            }

            if (a.IsNegative && !b.IsNegative)
            {
                return true;
            }

            if (!a.IsNegative && b.IsNegative)
            {
                return false;
            }

            if (!a.IsNegative && !b.IsNegative)
            {
                if (a.Hours < b.Hours)
                {
                    return true;
                }

                if (a.Hours > b.Hours)
                {
                    return false;
                }

                if (a.Hours.Equals(b.Hours))
                {
                    if (a.Minutes < b.Minutes)
                    {
                        return true;
                    }

                    if (a.Minutes > b.Minutes)
                    {
                        return false;
                    }

                    if (a.Minutes.Equals(b.Minutes))
                    {
                        if (a.Seconds < b.Seconds)
                        {
                            return true;
                        }

                        if (a.Seconds > b.Seconds)
                        {
                            return false;
                        }
                    }
                }
            }

            if (a.IsNegative && b.IsNegative)
            {
                if (a.Hours < b.Hours)
                {
                    return false;
                }

                if (a.Hours > b.Hours)
                {
                    return true;
                }

                if (a.Hours.Equals(b.Hours))
                {
                    if (a.Minutes < b.Minutes)
                    {
                        return false;
                    }

                    if (a.Minutes > b.Minutes)
                    {
                        return true;
                    }

                    if (a.Minutes.Equals(b.Minutes))
                    {
                        if (a.Seconds < b.Seconds)
                        {
                            return false;
                        }

                        if (a.Seconds > b.Seconds)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;

        }

        public static bool operator >(Time a, Time b)
        {
            return !(a < b);
        }

        public static bool operator <=(Time a, Time b)
        {
            if (a.Equals(b))
            {
                return true;
            }

            return a < b;
        }

        public static bool operator >=(Time a, Time b)
        {
            if (a.Equals(b))
            {
                return true;
            }

            return a > b;
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
            return Equals((Time)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                var hashCode = IsNegative.GetHashCode();
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                hashCode = (hashCode * 397) ^ Hours;
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                hashCode = (hashCode * 397) ^ Minutes;
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                hashCode = (hashCode * 397) ^ Seconds;
                return hashCode;
            }
        }
    }
}