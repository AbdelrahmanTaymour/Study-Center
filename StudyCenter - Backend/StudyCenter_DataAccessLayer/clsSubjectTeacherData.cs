using System.Data;
using StudyCenter_DataAccessLayer.DTOs.SubjectTeacherDTOs;
using StudyCenter_DataAccessLayer.Global_Classes;
using StudyCenter_DataAccessLayer.Mappings;

namespace StudyCenter_DataAccessLayer;

public class clsSubjectTeacherData
{
    public static List<SubjectTeacherDto> All()
        => clsDataAccessHelper.All("SP_SubjectsTeachers_GetAllSubjectsTeachers",
            SubjectTeacherMapping.MapToSubjectTeacherDto);
    
    public static SubjectTeacherDto? GetInfoById(int? subjectTeacherId)
        => clsDataAccessHelper.GetBy("SP_SubjectsTeachers_GetSubjectsTeacherInfoByID", "SubjectTeacherID",
            subjectTeacherId, SubjectTeacherMapping.MapToSubjectTeacherDto);

    public static int? Add(SubjectTeacherCreationDto newSubjectTeacher)
        => clsDataAccessHelper.Add("SP_SubjectsTeachers_AddNewSubjectsTeacher", "NewSubjectTeacherID",
            newSubjectTeacher);

    public static bool Update(SubjectTeacherUpdateDto updatedSubjectTeacher)
        => clsDataAccessHelper.Update("SP_SubjectsTeachers_UpdateSubjectsTeacherInfo", updatedSubjectTeacher);

        public static bool Delete(int? subjectTeacherId)
            => clsDataAccessHelper.Delete("SP_SubjectsTeachers_DeleteSubjectsTeacher", "SubjectTeacherID", subjectTeacherId);

        public static bool Exists(int? subjectTeacherId)
            => clsDataAccessHelper.Exists("SP_SubjectsTeachers_DoesSubjectsTeacherExists", "SubjectTeacherID", subjectTeacherId);

        public static bool IsTeachingSubject(int? teacherId, int? subjectGradeLevelId)
            => clsDataAccessHelper.Exists("SP_SubjectsTeachers_IsTeachingSubject", "TeacherID", teacherId,
                "SubjectGradeLevelID", subjectGradeLevelId);
}