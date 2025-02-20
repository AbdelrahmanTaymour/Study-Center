using System.Data;
using StudyCenter_DataAccessLayer.DTOs.StudentGroupDTOs;

namespace StudyCenter_DataAccessLayer.Mappings;

public class StudentGroupMapping
{
    public static StudentGroupDto MapToGroupDto(IDataRecord record)
    {
        return new StudentGroupDto
        (
            record.GetInt32(record.GetOrdinal("StudentGroupID")),
            record.GetInt32(record.GetOrdinal("StudentID")),
            record.GetInt32(record.GetOrdinal("GroupID")),
            record.GetDateTime(record.GetOrdinal("StartDate")),
            record.GetValue(record.GetOrdinal("EndDate")) as DateTime? ?? null,
            record.GetBoolean(record.GetOrdinal("IsActive")),
            record.GetInt32(record.GetOrdinal("CreatedByUserID"))
        );
    }

    public static StudentGroupCreationDto MapToStudentGroupCreationDto(IDataRecord record)
    {
        return new StudentGroupCreationDto
        (
            record.GetInt32(record.GetOrdinal("StudentID")),
            record.GetInt32(record.GetOrdinal("GroupID")),
            record.GetDateTime(record.GetOrdinal("StartDate")),
            record.GetValue(record.GetOrdinal("EndDate")) as DateTime? ?? null,
            record.GetBoolean(record.GetOrdinal("IsActive")),
            record.GetInt32(record.GetOrdinal("CreatedByUserID"))
        );
    }
}