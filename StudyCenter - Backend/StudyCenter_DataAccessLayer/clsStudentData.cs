using System.Data;
using Microsoft.Data.SqlClient;
using StudyCenter_DataAccessLayer.DTOs.StudentDTOs;
using StudyCenter_DataAccessLayer.Global_Classes;
using StudyCenter_DataAccessLayer.Mappings;

namespace StudyCenter_DataAccessLayer;

public class clsStudentData
{
    public static StudentDto? GetInfoByStudentId(int? studentID)
        => clsDataAccessHelper.GetBy("SP_Students_GetStudentInfoByID", "StudentID", studentID,
            StudentMapping.MapToStudentDto);
    
    public static StudentDto? GetInfoByPersonId(int? personID)
        => clsDataAccessHelper.GetBy("SP_Students_GetStudentInfoByPersonID", "PersonID", personID,
            StudentMapping.MapToStudentDto);


    public static int? Add(StudentCreationDto newStudent) =>
        clsDataAccessHelper.Add("SP_Students_AddNewStudent", "NewStudentID", newStudent);

    public static bool Update(StudentUpdateDto updatedStudent) =>
        clsDataAccessHelper.Update("SP_Students_UpdateStudentInfo", updatedStudent);

    public static bool Delete(int? studentID, int? deletedByUserID)
    {
        var parameters = new (string name, object? value)[]
        {
            (name: "StudentID", value: studentID) ,
            (name: "DeletedByUserID", value: deletedByUserID)
        };
        
        return clsDataAccessHelper.Delete("SP_Students_DeleteStudent", parameters);
    }

        public static bool Exists(int? studentID)
            => clsDataAccessHelper.Exists("SP_Students_DoesStudentExist", "StudentID", studentID);

        public static List<StudentDto> All()
            => clsDataAccessHelper.All("SP_Students_GetAllStudents", StudentMapping.MapToStudentDto);

        public static bool IsStudent(int? personID)
            => clsDataAccessHelper.Exists("SP_Students_IsStudent", "PersonID", personID);
}