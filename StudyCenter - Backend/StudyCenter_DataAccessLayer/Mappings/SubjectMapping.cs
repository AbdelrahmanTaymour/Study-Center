using System.Data;
using StudyCenter_DataAccessLayer.DTOs.SubjectDTOs;

namespace StudyCenter_DataAccessLayer.Mappings;

public class SubjectMapping
{
    public static SubjectDto MapToSubjectDto(IDataRecord record)
    {
        return new SubjectDto
        (
            record.GetInt32(record.GetOrdinal("SubjectID")),
            record.GetString(record.GetOrdinal("SubjectName"))
        );
    }

    public static SubjectCreationDto MapToSubjectCreationDto(IDataRecord record)
    {
        return new SubjectCreationDto
        (
            record.GetString(record.GetOrdinal("SubjectName"))
        );
    }
}