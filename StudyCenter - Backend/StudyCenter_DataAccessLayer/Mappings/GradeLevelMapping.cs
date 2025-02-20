using System.Data;
using StudyCenter_DataAccessLayer.DTOs.GradeLevelDTOs;

namespace StudyCenter_DataAccessLayer.Mappings;

public class GradeLevelMapping
{
    public static GradeLevelDto MapToGradeLevelDto(IDataRecord record)
    {
        return new GradeLevelDto
        (
            record.GetInt32(record.GetOrdinal("GradeLevelID")),
            record.GetString(record.GetOrdinal("GradeName"))
        );
    }

    public static GradeLevelCreationDto MapToGradeLevelCreationDto(IDataRecord record)
    {
        return new GradeLevelCreationDto
        (
            record.GetString(record.GetOrdinal("GradeName"))
        );
    }
}