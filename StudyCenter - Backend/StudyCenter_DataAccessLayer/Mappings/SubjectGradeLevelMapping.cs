using System.Data;
using StudyCenter_DataAccessLayer.DTOs.SubjectGradeLevelDTOs;

namespace StudyCenter_DataAccessLayer.Mappings;

public class SubjectGradeLevelMapping
{
    public static SubjectGradeLevelDto MapToSubjectGradeLevelDto(IDataRecord record)
    {
        return new SubjectGradeLevelDto
        (
            record.GetInt32(record.GetOrdinal("SubjectGradeLevelID")),
            record.GetInt32(record.GetOrdinal("SubjectID")),
            record.GetInt32(record.GetOrdinal("GradeLevelID")),
            record.GetDecimal(record.GetOrdinal("Fees")),
            record.GetBoolean(record.GetOrdinal("IsMandatory")),
            record.GetValue(record.GetOrdinal("Description")) as string ?? null
        );
    }

    public static SubjectGradeLevelCreationDto MapToSubjectGradeLevelCreationDto(IDataRecord record)
    {
        return new SubjectGradeLevelCreationDto
        (
            record.GetInt32(record.GetOrdinal("SubjectID")),
            record.GetInt32(record.GetOrdinal("GradeLevelID")),
            record.GetDecimal(record.GetOrdinal("Fees")),
            record.GetBoolean(record.GetOrdinal("IsMandatory")),
            record.GetValue(record.GetOrdinal("Description")) as string ?? null
        );
    }
}