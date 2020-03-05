using System;


namespace XYGraphLib {


  public enum TimeSpanUnitEnum {
    none,
    second,  //31.Dec 99 23:00:59 |00     |01     |02     |03   
    second10,//31.Dec 99 23:00:50 |00     |10     |20     |30   
    minute,  //31.Dec 99 23:00    |23:01  |23:02  |23:03  |23:04
    minute10,//31.Dec 99 23:00    |23:10  |23:20  |23:30  |23:40
    hour,    //31.Dec 99 23:00    |00:00  |01:00  |02:00  |03:00
    hour6,   //31.Dec 99 18:00    |00:00  |06:00  |12:00  |18:00
    day,     //31.Dec 99          |01.Dec |02.Dec |03.Dec |04.Dec    
    week,    //31.Dec 99          |03.Jan |10.Jan |17.Jan |24.Jan    
    month,   //Dec 99             |Jan 00 |Feb 00 |Mar 00 |Apr 00
    year,    //1999               |2000   |2001
    year5,   //1999               |2000   |2005
    year10,  //1990               |2000   |2010
    year50,  //1990               |2000   |2050
    year100, //1900               |2000   |2100
    year500, //1900               |2000   |2500
    year1000,//1000               |2000   |3000
    max= year1000
  }


  public struct TimeSpanUnitMask {
    public readonly TimeSpanUnitEnum TimeSpanUnit;
    public readonly string FirstLabelMask;
    public readonly string FirstLabelToString;
    public readonly string FollowLabelMask;
    public readonly string FollowLabelToString;
    public readonly long Ticks;

    public TimeSpanUnitMask(TimeSpanUnitEnum timeSpanUnit, string firstLabelMask, string firstLabelToString, 
      string followLabelMask, string followLabelToString, long ticks) {
      TimeSpanUnit = timeSpanUnit;
      FirstLabelMask = firstLabelMask;
      FirstLabelToString = firstLabelToString;
      FollowLabelMask = followLabelMask;
      FollowLabelToString = followLabelToString;
      Ticks = ticks;
    }


    public override string ToString() {
      return TimeSpanUnit + " FirstLabelMask: " + FirstLabelMask + "; FirstLabelToString: " + FirstLabelToString + 
        "; FollowLabelMask: " + FollowLabelMask + "; FollowLabelToString: " + FollowLabelToString + "; Ticks: " + Ticks + ";";
    }
  }


  public struct TimeSpanUnitConfig {
    public readonly TimeSpanUnitEnum TimeSpanUnit;
    public readonly double FirstLabelWidth;
    public readonly double FollowLabelWidth;


    public TimeSpanUnitConfig(TimeSpanUnitEnum timeSpanUnit, double firstLabelWidth, double followLabelWidth) {
      TimeSpanUnit = timeSpanUnit;
      FirstLabelWidth = firstLabelWidth;
      FollowLabelWidth = followLabelWidth;
    }


    public override string ToString() {
      return TimeSpanUnit + " FirstLabelWidth: " + FirstLabelWidth + "; FollowLabelWidth: " + FollowLabelWidth + ";";
    }
  }


  public static class TimeSpanUnitExtension {

    public const long SecondTicks = 10000000;
    public const long MinuteTicks = 60 * SecondTicks;
    public const long HourTicks = 60 * MinuteTicks;
    public const long DayTicks = 24 * HourTicks;
    public const long WeekTicks = 7 * DayTicks;
    public const long MonthTicks = 31 * DayTicks;
    public const long YearTicks = 365 * DayTicks;


    public static readonly TimeSpanUnitMask[] TimeSpanUnitMasks = new TimeSpanUnitMask[]{
      
                         //TimeSpanUnit              FirstLabelMask        FirstLabelToString    FollowLabelMask     Ticks
                         //                                                                                FollowLabelToString
      new TimeSpanUnitMask(TimeSpanUnitEnum.none,    null,                 null,                 null,     null,     long.MaxValue),
      new TimeSpanUnitMask(TimeSpanUnitEnum.second,  "31.WWW 99 23:00:59", "dd.MMM yy HH:mm:ss", "00",     "ss",     SecondTicks), 
      new TimeSpanUnitMask(TimeSpanUnitEnum.second10,"31.WWW 99 23:00:50", "dd.MMM yy HH:mm:ss", "00",     "ss",     10*SecondTicks), 
      new TimeSpanUnitMask(TimeSpanUnitEnum.minute,  "31.WWW 99 23:00",    "dd.MMM yy HH:mm",    "23:01",  "HH:mm",  MinuteTicks),
      new TimeSpanUnitMask(TimeSpanUnitEnum.minute10,"31.WWW 99 23:00",    "dd.MMM yy HH:mm",    "23:10",  "HH:mm",  10*MinuteTicks),
      new TimeSpanUnitMask(TimeSpanUnitEnum.hour,    "31.WWW 99 23:00",    "dd.MMM yy HH:mm",    "00:00",  "HH:mm",  HourTicks),
      new TimeSpanUnitMask(TimeSpanUnitEnum.hour6,   "31.WWW 99 18:00",    "dd.MMM yy HH:mm",    "00:00",  "HH:mm",  6*HourTicks),
      new TimeSpanUnitMask(TimeSpanUnitEnum.day,     "31.WWW 99",          "dd.MMM yy",          "01.WWW", "dd.MMM", DayTicks),
      new TimeSpanUnitMask(TimeSpanUnitEnum.week,    "31.WWW 99",          "dd.MMM yy",          "03.WWW", "dd.MMM", WeekTicks),
      new TimeSpanUnitMask(TimeSpanUnitEnum.month,   "WWW 99",             "MMM yy",             "WWW 00", "MMM yy", MonthTicks),
      new TimeSpanUnitMask(TimeSpanUnitEnum.year,    "1999",               "yyyy",               "2000",   "yyyy",   YearTicks),
      new TimeSpanUnitMask(TimeSpanUnitEnum.year5,   "1995",               "yyyy",               "2000",   "yyyy",   5*YearTicks),
      new TimeSpanUnitMask(TimeSpanUnitEnum.year10,  "1990",               "yyyy",               "2000",   "yyyy",   10*YearTicks),
      new TimeSpanUnitMask(TimeSpanUnitEnum.year50,  "1950",               "yyyy",               "2000",   "yyyy",   50*YearTicks),
      new TimeSpanUnitMask(TimeSpanUnitEnum.year100, "1900",               "yyyy",               "2000",   "yyyy",   100*YearTicks),
      new TimeSpanUnitMask(TimeSpanUnitEnum.year500, "1400",               "yyyy",               "2000",   "yyyy",   500*YearTicks),
      new TimeSpanUnitMask(TimeSpanUnitEnum.year1000,"1000",               "yyyy",               "2000",   "yyyy",   1000*YearTicks),
    };


    public static TimeSpanUnitMask GetMask(this TimeSpanUnitEnum timeSpanUnit) {
      return TimeSpanUnitMasks[(int)timeSpanUnit];
    }


    public static long GetTicks(this TimeSpanUnitEnum timeSpanUnit) {
      return TimeSpanUnitMasks[(int)timeSpanUnit].Ticks;
    }


    public static int GetYearMultiplier(this TimeSpanUnitEnum timeSpanUnit) {
      switch (timeSpanUnit) {
      case TimeSpanUnitEnum.year:
        return 1;
      case TimeSpanUnitEnum.year5:
        return 5;
      case TimeSpanUnitEnum.year10:
        return 10;
      case TimeSpanUnitEnum.year50:
        return 50;
      case TimeSpanUnitEnum.year100:
        return 100; 
      case TimeSpanUnitEnum.year500:
        return 500; 
      case TimeSpanUnitEnum.year1000:
        return 1000; 
      }
      throw new NotSupportedException();
    }

    public static string ToFirstLabel(this DateTime date, TimeSpanUnitEnum timeSpanUnit) {
      return date.ToString(TimeSpanUnitMasks[(int)timeSpanUnit].FirstLabelToString);
    }


    public static string ToFollowLabel(this DateTime date, TimeSpanUnitEnum timeSpanUnit) {
      return date.ToString(TimeSpanUnitMasks[(int)timeSpanUnit].FollowLabelToString);
    }
  }


  public class TimeSpanUnitFormatter {

    GlyphDrawer glyphDrawer;
    double fontSize;
    TimeSpanUnitConfig[] TimeSpanUnitConfigs;
    double maxFollowLabelWidth;
 

    public void SetGlyphDrawer(GlyphDrawer glyphDrawer, double fontSize) {
      if (this.glyphDrawer==glyphDrawer && this.fontSize==fontSize) return;

      this.glyphDrawer = glyphDrawer;
      this.fontSize = fontSize;

      if (TimeSpanUnitConfigs==null) TimeSpanUnitConfigs = new TimeSpanUnitConfig[(int)TimeSpanUnitEnum.max+1];

      maxFollowLabelWidth = double.MinValue;
      for (TimeSpanUnitEnum timeSpanUnitIndex = TimeSpanUnitEnum.none+1; timeSpanUnitIndex <= TimeSpanUnitEnum.max; timeSpanUnitIndex++) {
        TimeSpanUnitMask mask = timeSpanUnitIndex.GetMask();
        double firstLabelWidth = glyphDrawer.GetLength(mask.FirstLabelMask, fontSize);
        double followLabelWidth = glyphDrawer.GetLength(mask.FollowLabelMask, fontSize);
        maxFollowLabelWidth = Math.Max(followLabelWidth, maxFollowLabelWidth);
        TimeSpanUnitConfigs[(int)timeSpanUnitIndex] = new TimeSpanUnitConfig(timeSpanUnitIndex, firstLabelWidth, followLabelWidth);
      }
    }


    public TimeSpanUnitConfig GetWidths(TimeSpanUnitEnum timeSpanUnit) {
      return TimeSpanUnitConfigs[(int)timeSpanUnit];
    }


    public TimeSpanUnitEnum GetDefaultTimeSpanUnit(TimeSpan timeSpan, double totalWidth) {
      int stepCount = (int)(totalWidth / (1.1 * maxFollowLabelWidth));
      long ticksPerStep = timeSpan.Ticks;
      if (stepCount>1) {
        ticksPerStep /= stepCount;
      }
      TimeSpanUnitEnum timeSpanUnitIndex;
      for (timeSpanUnitIndex = TimeSpanUnitEnum.none+1; timeSpanUnitIndex < TimeSpanUnitEnum.max; timeSpanUnitIndex++) {
        if (ticksPerStep<timeSpanUnitIndex.GetTicks()) break;
      }
      return timeSpanUnitIndex;
    }
  }
}
