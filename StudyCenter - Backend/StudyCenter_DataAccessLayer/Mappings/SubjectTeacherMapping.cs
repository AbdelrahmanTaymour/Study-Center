using System.Data;
using StudyCenter_DataAccessLayer.DTOs.SubjectTeacherDTOs;

namespace StudyCenter_DataAccessLayer.Mappings;

public class SubjectTeacherMapping
{
    public static SubjectTeacherDto MapToSubjectTeacherDto(IDataRecord record)
    {
        return new SubjectTeacherDto
        (
            record.GetInt32(record.GetOrdinal("SubjectTeacherID")),
            record.GetInt32(record.GetOrdinal("SubjectGradeLevelID")),
            record.GetInt32(record.GetOrdinal("TeacherID")),
            record.GetDateTime(record.GetOrdinal("AssignmentDate")),
            record.GetValue(record.GetOrdinal("LastModifiedDate")) as DateTime? ?? null,
            record.GetBoolean(record.GetOrdinal("IsActive"))
        );
    }

    public static SubjectTeacherCreationDto MapToSubjectTeacherCreationDto(IDataRecord record)
    {
        return new SubjectTeacherCreationDto
        (
            record.GetInt32(record.GetOrdinal("SubjectGradeLevelID")),
            record.GetInt32(record.GetOrdinal("TeacherID")),
            record.GetBoolean(record.GetOrdinal("IsActive"))
        );
    }

    public static SubjectTeacherUpdateDto MapToSubjectTeacherUpdateDto(IDataRecord record)
    {
        return new SubjectTeacherUpdateDto
        (
            record.GetInt32(record.GetOrdinal("SubjectTeacherID")),
            record.GetInt32(record.GetOrdinal("SubjectGradeLevelID")),
            record.GetInt32(record.GetOrdinal("TeacherID")),
            record.GetBoolean(record.GetOrdinal("IsActive"))
        );
    }
}