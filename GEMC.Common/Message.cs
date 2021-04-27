using System;

namespace GEMC.Common
{
    using Newtonsoft.Json;

    public class Message
    {
        public string Json { get; set; }

        public TimingStatus Status { get; set; }

        public DateTime TimeStamp { get; set; }

        public Event Event { get; set; }

        public bool IsStart { get; set; }

        public bool IsEnd { get; set; }

        public bool IsOneMinuteBefore { get; set; }
        
        public static Message FromJson(string json)
        {
            Message newInstance = JsonConvert.DeserializeObject<Message>(json);
            newInstance.Json = json;
            newInstance.TimeStamp = DateTime.Now;
            Time raceDuration = new Time("00:05:00");
            Time waitTime = new Time("00:00:30");
            Time currentTime = new Time(newInstance.Event.Metadata.CurrentTime);
            Time countDown = new Time(newInstance.Event.Metadata.Countdown);
            Time raceTime = new Time(newInstance.Event.Metadata.RaceTime);
            Time remainingTime = new Time(newInstance.Event.Metadata.RemainingTime);



            if (countDown.Equals(Time.Zero)
                && currentTime >= Time.Zero
                && currentTime < raceDuration
                && raceTime.Equals(raceDuration)
                && remainingTime >= Time.Zero
                && remainingTime < raceDuration)
            {
                newInstance.Status = TimingStatus.RaceRunning;
            }
            else if (countDown.Equals(Time.Zero)
                     && currentTime.Equals(raceDuration)
                     && raceTime.Equals(raceDuration)
                     && remainingTime >= Time.Zero
                     && remainingTime < waitTime)
            {
                newInstance.Status = TimingStatus.RaceEnded;
            }
            else 
            {
                newInstance.Status = TimingStatus.BetweenRaces;
            }

            return newInstance;
        }
    }
}
