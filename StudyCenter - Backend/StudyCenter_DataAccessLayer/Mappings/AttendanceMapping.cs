using System.Data;
using StudyCenter_DataAccessLayer.DTOs.AttendanceDTOs;

namespace StudyCenter_DataAccessLayer.Mappings;

public class AttendanceMapping
{
    public static AttendanceDto MapToAttendanceDto(IDataRecord record)
    {
        return new AttendanceDto
        (
            record.GetInt32(record.GetOrdinal("AttendanceID")),
            record.GetInt32(record.GetOrdinal("StudentGroupID")),
            record.GetDateTime(record.GetOrdinal("AttendanceDate")),
            record.GetValue(record.GetOrdinal("Notes")) as string ?? null
        );
    }

    public static AttendanceCreationDto MapToAttendanceCreationDto(IDataRecord record)
    {
        return new AttendanceCreationDto
        (
            record.GetInt32(record.GetOrdinal("StudentGroupID")),
            record.GetValue(record.GetOrdinal("Notes")) as string ?? null
        );
    }
}