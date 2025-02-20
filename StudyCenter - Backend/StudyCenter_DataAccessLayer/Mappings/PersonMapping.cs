using System.Data;
using StudyCenter_DataAccessLayer.DTOs.PersonDTOs;

namespace StudyCenter_DataAccessLayer.Mappings;

public class PersonMapping
{
    public static PersonDto MapToPersonDto(IDataRecord record)
    {
        return new PersonDto
        (
            record.GetInt32(record.GetOrdinal("PersonID")),
            record.GetString(record.GetOrdinal("FirstName")),
            record.GetString(record.GetOrdinal("SecondName")),
            record.GetValue(record.GetOrdinal("ThirdName")) as string ?? null,
            record.GetString(record.GetOrdinal("LastName")),
            record.GetByte(record.GetOrdinal("Gender")),
            record.GetDateTime(record.GetOrdinal("DateOfBirth")),
            record.GetString(record.GetOrdinal("PhoneNumber")),
            record.GetValue(record.GetOrdinal("Email")) as string ?? null,
            record.GetValue(record.GetOrdinal("Address")) as string ?? null
        );
    }

    public static PersonViewDto MapToPersonViewDto(IDataRecord record)
    {
        return new PersonViewDto
        (
            record.GetInt32(record.GetOrdinal("PersonID")),
            record.GetString(record.GetOrdinal("FullName")),
            record.GetString(record.GetOrdinal("Gender")),
            record.GetDateTime(record.GetOrdinal("DateOfBirth")),
            record.GetString(record.GetOrdinal("PhoneNumber")),
            record.GetValue(record.GetOrdinal("Email")) as string ?? null,
            record.GetValue(record.GetOrdinal("Address")) as string ?? null
        );
    }
}