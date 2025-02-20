using System.Data;
using Microsoft.Data.SqlClient;
using StudyCenter_DataAccessLayer.DTOs;
using StudyCenter_DataAccessLayer.Global_Classes;
using StudyCenter_DataAccessLayer.Mappings;

namespace StudyCenter_DataAccessLayer;

public class clsUserData
{
    
    public static List<UserViewDto> GetAllUsers()
        => clsDataAccessHelper.All("SP_Users_GetAllUsers", UserMapping.MapToUserViewDto);
    
    public static UserDto? GetUserInfoByID(int? userID)
        => clsDataAccessHelper.GetBy("SP_Users_GetUserInfoByID", "UserID", userID, UserMapping.MapToUserDto);
    
    public static UserDto? GetUserInfoByPersonID(int? personID)
        => clsDataAccessHelper.GetBy("SP_Users_GetUserInfoByPersonID", "PersonID", personID, UserMapping.MapToUserDto);

    public static UserDto? GetUserInfoByUsername(string username)
        => clsDataAccessHelper.GetBy("SP_Users_GetUserInfoByUsername", "Username", username, UserMapping.MapToUserDto);

    public static UserDto? Login(string username, string password)
    {
        var paramters = new (string name, object? password)[]
        {
            ("Username", username),
            ("Password", password)
        };
        return clsDataAccessHelper.GetBy("SP_Users_LoginByUsernameAndPassword", paramters, Mappings.UserMapping.MapToUserDto);
    }
    
    public static int? Add(UserCreationDto user)
        => clsDataAccessHelper.Add("SP_Users_AddNewUser", "NewUserID", user);

    public static bool Update(UserDto user)
        => clsDataAccessHelper.Update("SP_Users_UpdateUserInfo", user);
    
    public static bool Delete(int? userID)
        => clsDataAccessHelper.Delete("SP_Users_DeleteUser","UserID", userID);
    
    public static bool ExistsByUserID(int? userID)
        => clsDataAccessHelper.Exists("SP_Users_CheckIfUserExists", "UserID", userID);

    public static bool ExistsByPersonID(int? personID)
        => clsDataAccessHelper.Exists("SP_Users_DoesUserExistByPersonID", "PersonID", personID);

    public static bool ExistsByUsername(string? username)
        => clsDataAccessHelper.Exists("SP_Users_DoesUserExistByUsername", "Username", username);

    public static bool ExistsByUsernameAndPassword(string username, string password)
        => clsDataAccessHelper.Exists("SP_Users_DoesUserExistByUsernameAndPassword", "Username", username, "Password", password);

    public static bool ChangePassword(int? UserID, string NewPassword)
        => clsDataAccessHelper.UpdateValue("SP_Users_ChangePassword", "UserID", UserID, "NewPassword", NewPassword);
}