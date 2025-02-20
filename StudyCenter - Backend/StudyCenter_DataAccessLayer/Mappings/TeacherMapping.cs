using System.Data;
using StudyCenter_DataAccessLayer.DTOs.TeacherDTOs;

namespace StudyCenter_DataAccessLayer.Mappings;

public class TeacherMapping
{
    public static TeacherDto MapToTeacherDto(IDataRecord record)
    {
        return new TeacherDto
        (
            record.GetInt32(record.GetOrdinal("TeacherID")),
            record.GetInt32(record.GetOrdinal("PersonID")),
            record.GetInt32(record.GetOrdinal("EducationLevelID")),
            record.GetValue(record.GetOrdinal("TeachingExperience")) as byte? ?? null,
            record.GetValue(record.GetOrdinal("Certifications")) as string ?? null,
            record.GetBoolean(record.GetOrdinal("Status")),
            record.GetValue(record.GetOrdinal("Notes")) as string ?? null,
            record.GetInt32(record.GetOrdinal("CreatedByUserID"))
        );
    }

    public static TeacherCreationDto MapToTeacherCreationDto(IDataRecord record)
    {
        return new TeacherCreationDto
        (
            record.GetInt32(record.GetOrdinal("PersonID")),
            record.GetInt32(record.GetOrdinal("EducationLevelID")),
            record.GetValue(record.GetOrdinal("TeachingExperience")) as byte? ?? null,
            record.GetValue(record.GetOrdinal("Certifications")) as string ?? null,
            record.GetBoolean(record.GetOrdinal("Status")),
            record.GetValue(record.GetOrdinal("Notes")) as string ?? null,
            record.GetInt32(record.GetOrdinal("CreatedByUserID"))
        );
    }

    public static TeacherUpdateDto MapToTeachingUpdateDto(IDataRecord record)
    {
        return new TeacherUpdateDto
        (
            record.GetInt32(record.GetOrdinal("TeacherID")),
            record.GetInt32(record.GetOrdinal("PersonID")),
            record.GetInt32(record.GetOrdinal("EducationLevelID")),
            record.GetValue(record.GetOrdinal("TeachingExperience")) as byte? ?? null,
            record.GetValue(record.GetOrdinal("Certifications")) as string ?? null,
            record.GetBoolean(record.GetOrdinal("Status")),
            record.GetValue(record.GetOrdinal("Notes")) as string ?? null
        );
    }
}