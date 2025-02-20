using System.Data;
using Microsoft.Data.SqlClient;
using StudyCenter_DataAccessLayer.DTOs.StudentGroupDTOs;
using StudyCenter_DataAccessLayer.Global_Classes;
using StudyCenter_DataAccessLayer.Mappings;

namespace StudyCenter_DataAccessLayer;

public class clsStudentGroupData
{
    public static List<StudentGroupDto> All()
        => clsDataAccessHelper.All("SP_StudentsGroups_GetAllStudentsGroups", StudentGroupMapping.MapToGroupDto);
    
    public static StudentGroupDto? GetInfoByID(int? studentGroupID)
        => clsDataAccessHelper.GetBy("SP_StudentsGroups_GetStudentsGroupInfoByID", "StudentGroupID", studentGroupID,
            StudentGroupMapping.MapToGroupDto);

    public static int? Add(StudentGroupCreationDto newStudentGroup)
        => clsDataAccessHelper.Add("SP_StudentsGroups_AddNewStudentsGroup", "NewStudentGroupID", newStudentGroup);

    public static bool Update(StudentGroupDto updatedStudentGroup)
        => clsDataAccessHelper.Update("SP_StudentsGroups_UpdateStudentsGroupInfo", updatedStudentGroup);

        public static bool Delete(int? studentGroupID)
            => clsDataAccessHelper.Delete("SP_StudentsGroups_DeleteStudentsGroup", "StudentGroupID", studentGroupID);

        public static bool Delete(int? studentID, int? groupID)
        {
            var parameters = new (string name, object? value)[]
            {
                (name: "StudentID", value: studentID),
                (name: "GroupID", value: groupID)
            };

            return clsDataAccessHelper.Delete("SP_StudentsGroups_DeleteStudentsGroupByStudentIDAndGroupID", parameters);
        }
        public static bool Exists(int? studentGroupID)
            => clsDataAccessHelper.Exists("SP_StudentsGroups_DoesStudentsGroupExists", "StudentGroupID", studentGroupID);

        public static bool IsStudentAssignedToGroup(int? studentID, int? groupID)
            => clsDataAccessHelper.Exists("SP_StudentsGroups_IsStudentAssignedToGroup",
                "StudentID", studentID, "GroupID", groupID);
}