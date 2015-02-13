using System;

namespace nucs.SystemCore.Time {
    public static class DateTimeExt {

        public static DateTime ToOffsetlessDateTime(this DateTime dt) {
            var dtoffset = TimeZone.CurrentTimeZone.GetUtcOffset(dt);
            return dt.ToUniversalTime().Add(dtoffset);
        }


         
    }
}