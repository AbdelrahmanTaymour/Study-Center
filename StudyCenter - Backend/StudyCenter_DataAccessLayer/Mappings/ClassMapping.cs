using System.Data;
using StudyCenter_DataAccessLayer.DTOs.ClassDTOs;

namespace StudyCenter_DataAccessLayer.Mappings;

public class ClassMapping
{
    public static ClassDto MapToClassDto(IDataRecord record)
    {
        return new ClassDto
        (
            record.GetInt32(record.GetOrdinal("ClassID")),
            record.GetString(record.GetOrdinal("ClassName")),
            record.GetByte(record.GetOrdinal("Capacity")),
            record.GetString(record.GetOrdinal("Description"))
        );
    }

    public static ClassCreationDto MapToClassCreationDto(IDataRecord record)
    {
        return new ClassCreationDto
        (
            record.GetString(record.GetOrdinal("ClassName")),
            record.GetByte(record.GetOrdinal("Capacity")),
            record.GetString(record.GetOrdinal("Description"))
        );
    }
}