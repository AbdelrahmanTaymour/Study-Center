using System.Data;
using Microsoft.Data.SqlClient;
using StudyCenter_DataAccessLayer.DTOs.TeacherDTOs;
using StudyCenter_DataAccessLayer.Global_Classes;
using StudyCenter_DataAccessLayer.Mappings;

namespace StudyCenter_DataAccessLayer;

public class clsTeacherData
{
    public static TeacherDto? GetInfoByTeacherId(int? teacherId) =>
        clsDataAccessHelper.GetBy("SP_Teachers_GetTeacherInfoByID", "TeacherID", teacherId,
            TeacherMapping.MapToTeacherDto);

    public static TeacherDto? GetInfoByPersonId(int? personId) =>
        clsDataAccessHelper.GetBy("SP_Teachers_GetTeacherInfoByPersonID", "PersonID", personId,
            TeacherMapping.MapToTeacherDto);

    public static int? Add(TeacherCreationDto? newTeacher) =>
        clsDataAccessHelper.Add("SP_Teachers_AddNewTeacher", "NewTeacherID", newTeacher);

    public static bool Update(TeacherUpdateDto? updatedTeacher)
        => clsDataAccessHelper.Update("SP_Teachers_UpdateTeacherInfo", updatedTeacher);

    public static bool Delete(int? teacherId, int? deletedByUserId)
    {
        var parameters = new (string name, object? value)[]
        {
            (name: "TeacherID", value: teacherId),
            (name: "DeletedByUserID", value: deletedByUserId)
        };

        return clsDataAccessHelper.Delete("SP_Teachers_DeleteTeacher", parameters);
    }

    public static bool Exists(int? teacherId)
        => clsDataAccessHelper.Exists("SP_Teachers_DoesTeacherExists", "TeacherID", teacherId);

    public static List<TeacherDto> All()
        => clsDataAccessHelper.All("SP_Teachers_GetAllTeachers", TeacherMapping.MapToTeacherDto);

    public static bool IsTeacher(int? personId)
        => clsDataAccessHelper.Exists("SP_Teachers_IsTeacher", "PersonID", personId);
    
    public static string? GetFullName(int? teacherId)
        => clsDataAccessHelper.GetStringValue("SP_Teachers_GetFullTeacherName", "TeacherID", teacherId, "FullName",
            255);
    
}