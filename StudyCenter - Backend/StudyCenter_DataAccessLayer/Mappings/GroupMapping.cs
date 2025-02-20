using System.Data;
using StudyCenter_DataAccessLayer.DTOs.GroupDTOs;

namespace StudyCenter_DataAccessLayer.Mappings;

public class GroupMapping
{
    public static GroupDto MapToGroupDto(IDataRecord record)
    {
        return new GroupDto
        (
            record.GetInt32(record.GetOrdinal("GroupID")),
            record.GetString(record.GetOrdinal("GroupName")),
            record.GetInt32(record.GetOrdinal("ClassID")),
            record.GetInt32(record.GetOrdinal("TeacherID")),
            record.GetInt32(record.GetOrdinal("SubjectTeacherID")),
            record.GetInt32(record.GetOrdinal("MeetingTimeID")),
            record.GetValue(record.GetOrdinal("Description")) as string ?? null,
            record.GetInt32(record.GetOrdinal("CreatedByUserID")),
            record.GetDateTime(record.GetOrdinal("CreationDate")),
            record.GetValue(record.GetOrdinal("LastModifiedDate")) as DateTime? ?? null,
            record.GetBoolean(record.GetOrdinal("IsActive")),
            record.GetValue(record.GetOrdinal("StudentCount")) as byte? ?? null
        );
    }

    public static GroupCreationDto MapToGroupCreationDto(IDataRecord record)
    {
        return new GroupCreationDto
        (
            record.GetString(record.GetOrdinal("GroupName")),
            record.GetInt32(record.GetOrdinal("ClassID")),
            record.GetInt32(record.GetOrdinal("TeacherID")),
            record.GetInt32(record.GetOrdinal("SubjectTeacherID")),
            record.GetInt32(record.GetOrdinal("MeetingTimeID")),
            record.GetValue(record.GetOrdinal("Description")) as string ?? null,
            record.GetInt32(record.GetOrdinal("CreatedByUserID")),
            record.GetBoolean(record.GetOrdinal("IsActive"))
        );
    }

    public static GroupUpdateDto MapToGroupUpdateDto(IDataRecord record)
    {
        return new GroupUpdateDto
        (
            record.GetInt32(record.GetOrdinal("GroupID")),
            record.GetString(record.GetOrdinal("GroupName")),
            record.GetInt32(record.GetOrdinal("ClassID")),
            record.GetInt32(record.GetOrdinal("TeacherID")),
            record.GetInt32(record.GetOrdinal("SubjectTeacherID")),
            record.GetInt32(record.GetOrdinal("MeetingTimeID")),
            record.GetValue(record.GetOrdinal("Description")) as string ?? null,
            record.GetInt32(record.GetOrdinal("CreatedByUserID")),
            record.GetBoolean(record.GetOrdinal("IsActive"))
        );
    }
}