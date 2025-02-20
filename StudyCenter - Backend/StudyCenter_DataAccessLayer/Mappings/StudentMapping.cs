using System.Data;
using StudyCenter_DataAccessLayer.DTOs.StudentDTOs;

namespace StudyCenter_DataAccessLayer.Mappings;

public class StudentMapping
{
    public static StudentDto MapToStudentDto(IDataRecord record)
    {
        return new StudentDto
        (
            record.GetInt32(record.GetOrdinal("StudentID")),
            record.GetInt32(record.GetOrdinal("PersonID")),
            record.GetInt32(record.GetOrdinal("GradeLevelID")),
            record.GetBoolean(record.GetOrdinal("Status")),
            record.GetString(record.GetOrdinal("Notes")),
            record.GetInt32(record.GetOrdinal("CreatedByUserID")),
            record.GetDateTime(record.GetOrdinal("CreationDate"))
        );

    }
    
    public static StudentCreationDto MapToStudentCreationDto(IDataRecord record)
    {
        return new StudentCreationDto
        (
            record.GetInt32(record.GetOrdinal("PersonID")),
            record.GetInt32(record.GetOrdinal("GradeLevelID")),
            record.GetBoolean(record.GetOrdinal("Status")),
            record.GetString(record.GetOrdinal("Notes")),
            record.GetInt32(record.GetOrdinal("CreatedByUserID"))
        );
    }

    public static StudentUpdateDto MapToStudentUpdateDto(IDataRecord record)
    {
        return new StudentUpdateDto
        (
            record.GetInt32(record.GetOrdinal("StudentID")),
            record.GetInt32(record.GetOrdinal("PersonID")),
            record.GetInt32(record.GetOrdinal("GradeLevelID")),
            record.GetBoolean(record.GetOrdinal("Status")),
            record.GetString(record.GetOrdinal("Notes"))
        );
    }
}