using System.Data;
using StudyCenter_DataAccessLayer.DTOs;

namespace StudyCenter_DataAccessLayer.Mappings;

public class UserMapping
{
    public static UserDto MapToUserDto(IDataRecord record)
    {
        return new UserDto
        (
            record.GetInt32(record.GetOrdinal("UserID")),
            record.GetInt32(record.GetOrdinal("PersonID")),
            record.GetString(record.GetOrdinal("Username")),
            record.GetString(record.GetOrdinal("Password")),
            record.GetInt32(record.GetOrdinal("Permissions")),
            record.GetBoolean(record.GetOrdinal("IsActive"))
        );
    }

    public static UserViewDto MapToUserViewDto(IDataRecord record)
    {
        return new UserViewDto
        (
            record.GetInt32(record.GetOrdinal("UserID")),
            record.GetString(record.GetOrdinal("FullName")),
            record.GetString(record.GetOrdinal("Username")),
            record.GetString(record.GetOrdinal("Gender")),
            record.GetString(record.GetOrdinal("PhoneNumber")),
            record.GetBoolean(record.GetOrdinal("IsActive"))
        );
    }
}