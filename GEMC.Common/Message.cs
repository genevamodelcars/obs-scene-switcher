using System;

namespace GEMC.Common
{
    using Newtonsoft.Json;

    public class Message
    {
        public string Json { get; set; }

        public TimingStatus Status { get; set; }

        public DisplayStatus DisplayStatus { get; set; }
        public DateTime TimeStamp { get; set; }

        public Event Event { get; set; }
        
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
            else if (countDown > Time.Zero
                     && currentTime.Equals(Time.Zero)
                     && raceTime.Equals(raceDuration)
                     && remainingTime.Equals(raceDuration))
            {
                newInstance.Status = TimingStatus.BetweenRaces;
            }
            else if (countDown.Equals(Time.Zero)
                     && currentTime.Equals(raceDuration)
                     && raceTime.Equals(raceDuration)
                     && remainingTime >= Time.Zero
                     && remainingTime < waitTime)
            {
                newInstance.Status = TimingStatus.RaceEnding;
            }
            else
            {
                newInstance.Status = TimingStatus.NoData;
            }

            return newInstance;
        }
    }
}

//Time waitTime = new Time("00:00:10");
//Time endTime = Time.MaxValue;
//Time divergence = new Time(newInstance.Event.Metadata.Divergence);

//if (divergence.Minutes > 3 || divergence.Hours > 0)
//{
//newInstance.Status = TimingStatus.NextRaceSchedule;
//}
//else
//{
//Time countdown = new Time(newInstance.Event.Metadata.Countdown);
//Time current = new Time(newInstance.Event.Metadata.CurrentTime);
//Time race = new Time(newInstance.Event.Metadata.RaceTime);
//Time remaining = new Time(newInstance.Event.Metadata.RemainingTime);

//    if (countdown.Equals(new Time("00:00:30")))
//{
//    endTime = Time.MaxValue;
//}

//if (countdown.Equals(new Time("00:00:30")) || !current.Equals(race))
//{
//    newInstance.Status = TimingStatus.RaceRunning;

//    if (remaining.Equals(new Time("00:00:01")))
//    {
//        endTime = Time.Now();
//    }

//    if (endTime + waitTime<Time.Now())
//    {
//        newInstance.Status = TimingStatus.RaceEnded;
//    }
//}
//else
//{
//    newInstance.Status = TimingStatus.RacePreparation;
//}
//}









/*
0 = standby
1 = nextrace schedule
2 = race preparation
3 = race running
4 = race ended
*/

//var now = Date.now();

//if (data.EVENT.METADATA.DIVERGENCE.split(":")[1] > 3 || data.EVENT.METADATA.DIVERGENCE.split(":")[0] > 0) 
//{
//  timingstatus = 1;
//  endtime = 9582744014867;
//}
//else 
//{
//  if (data.EVENT.METADATA.COUNTDOWN == "00:00:30") 
//  {
//    endtime = 9582744014867;
//  }

//  if (data.EVENT.METADATA.COUNTDOWN == "00:00:30" || data.EVENT.METADATA.CURRENTIME != data.EVENT.METADATA.RACETIME) 
//  {
//    timingstatus = 3;

//    if (data.EVENT.METADATA.REMAININGTIME == "00:00:01") 
//    {
//        endtime = Date.now();
//    }

//    if (now> endtime + waittime) 
//    {
//        timingstatus = 4;
//    }
//  }
//  else 
//  {
//    timingstatus = 2;
//  }
//}