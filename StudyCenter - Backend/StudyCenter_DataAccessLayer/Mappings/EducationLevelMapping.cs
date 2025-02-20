using System.Data;
using StudyCenter_DataAccessLayer.DTOs.EducationLevelDTOs;

namespace StudyCenter_DataAccessLayer.Mappings;

public class EducationLevelMapping
{
    public static EducationLevelDto MapToEducationLevelDto(IDataRecord record)
    {
        return new EducationLevelDto
        (
            record.GetInt32(record.GetOrdinal("EducationLevelID")),
            record.GetString(record.GetOrdinal("LevelName"))
        );
    }

    public static EducationLevelCreationDto MapToEducationLevelCreationDto(IDataRecord record)
    {
        return new EducationLevelCreationDto
        (
            record.GetString(record.GetOrdinal("LevelName"))
        );
    }
        
}