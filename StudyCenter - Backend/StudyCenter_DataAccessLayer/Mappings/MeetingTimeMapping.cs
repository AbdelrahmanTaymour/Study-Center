using System.Data;
using StudyCenter_DataAccessLayer.DTOs.MeetingTimeDTOs;

namespace StudyCenter_DataAccessLayer.Mappings;

public class MeetingTimeMapping
{
    public static MeetingTimeDto MapToMeetingTimeDto(IDataRecord record)
    {
        return new MeetingTimeDto
        (
            record.GetInt32(record.GetOrdinal("MeetingTimeID")),
            (TimeSpan)record.GetValue(record.GetOrdinal("StartTime")),
            (TimeSpan)record.GetValue(record.GetOrdinal("EndTime")),
            record.GetByte(record.GetOrdinal("MeetingDays"))
        );
    }

    public static MeetingTimeCreationDto MapToMeetingTimeCreationDto(IDataRecord record)
    {
        return new MeetingTimeCreationDto
        (
            (TimeSpan)record.GetValue(record.GetOrdinal("StartTime")),
            (TimeSpan)record.GetValue(record.GetOrdinal("EndTime")),
            record.GetByte((record.GetOrdinal("MeetingDays")))
        );
    }
}