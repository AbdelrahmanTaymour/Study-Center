using StudyCenter_DataAccessLayer.DTOs.SubjectDTOs;
using StudyCenter_DataAccessLayer.Global_Classes;
using StudyCenter_DataAccessLayer.Mappings;

namespace StudyCenter_DataAccessLayer;

public class clsSubjectData
{
    public static List<SubjectDto> All()
        => clsDataAccessHelper.All("SP_Subjects_GetAllSubjects", SubjectMapping.MapToSubjectDto);

    public static List<SubjectCreationDto> AllNames()
        => clsDataAccessHelper.All("SP_Subjects_GetAllSubjectsName", SubjectMapping.MapToSubjectCreationDto);

    public static SubjectDto? GetInfoById(int? subjectId)
        => clsDataAccessHelper.GetBy("SP_Subjects_GetSubjectInfoByID",
            "SubjectID", subjectId, SubjectMapping.MapToSubjectDto);

    public static int? Add(SubjectCreationDto newSubject) =>
        clsDataAccessHelper.Add("SP_Subjects_AddNewSubject", "NewSubjectID", newSubject);

    public static bool Update(SubjectDto updatedSubject) =>
        clsDataAccessHelper.Update("SP_Subjects_UpdateSubjectInfo", updatedSubject);

    public static bool Delete(int? subjectId)
        => clsDataAccessHelper.Delete("SP_Subjects_DeleteSubject", "SubjectID", subjectId);

    public static bool Exists(int? subjectId)
        => clsDataAccessHelper.Exists("SP_Subjects_DoesSubjectExists", "SubjectID", subjectId);

    public static bool Exists(string subjectName)
        => clsDataAccessHelper.Exists("SP_Subjects_DoesSubjectExistBySubjectName", "SubjectName", subjectName);
    
    public static int? GetSubjectID(string subjectName)
        => clsDataAccessHelper.GetIntValue("SP_Subjects_GetSubjectID", "SubjectName", subjectName, "SubjectID");

    public static string? GetSubjectNameBySubjectID(int? subjectId)
        => clsDataAccessHelper.GetStringValue("SP_Subjects_GetSubjectNameBySubjectID", "SubjectID", subjectId,
            "SubjectName", 100);
}