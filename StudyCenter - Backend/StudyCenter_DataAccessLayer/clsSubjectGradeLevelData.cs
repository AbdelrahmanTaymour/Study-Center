using StudyCenter_DataAccessLayer.DTOs.SubjectGradeLevelDTOs;
using StudyCenter_DataAccessLayer.Global_Classes;
using StudyCenter_DataAccessLayer.Mappings;

namespace StudyCenter_DataAccessLayer;

public class clsSubjectGradeLevelData
{
    public static List<SubjectGradeLevelDto> All()
        => clsDataAccessHelper.All("SP_SubjectsGradeLevels_GetAllSubjectsGradeLevels",
            SubjectGradeLevelMapping.MapToSubjectGradeLevelDto);
    
    public static SubjectGradeLevelDto? GetInfoById(int? subjectGradeLevelId)
        => clsDataAccessHelper.GetBy("SP_SubjectsGradeLevels_GetSubjectsGradeLevelInfoByID", "SubjectGradeLevelID",
            subjectGradeLevelId, SubjectGradeLevelMapping.MapToSubjectGradeLevelDto);

    public static int? Add(SubjectGradeLevelCreationDto newSubjectGradeLevel)
        => clsDataAccessHelper.Add("SP_SubjectsGradeLevels_AddNewSubjectsGradeLevel", "NewSubjectGradeLevelID",
            newSubjectGradeLevel);

    public static bool Update(SubjectGradeLevelDto updatedSubjectGradeLevel) =>
        clsDataAccessHelper.Update("SP_SubjectsGradeLevels_UpdateSubjectsGradeLevelInfo", updatedSubjectGradeLevel);

        public static bool Delete(int? subjectGradeLevelId)
            => clsDataAccessHelper.Delete("SP_SubjectsGradeLevels_DeleteSubjectsGradeLevel", "SubjectGradeLevelID", subjectGradeLevelId);

        public static bool Exists(int? subjectGradeLevelId)
            => clsDataAccessHelper.Exists("SP_SubjectsGradeLevels_DoesSubjectsGradeLevelExists", "SubjectGradeLevelID",
                subjectGradeLevelId);

        public static bool Exists(int? subjectId, int? gradeLevelId)
            => clsDataAccessHelper.Exists("SP_SubjectsGradeLevels_DoesSubjectGradeLevelExistBySubjectIDAndGradeLevelID",
                "SubjectID", subjectId, "GradeLevelID", gradeLevelId);
}