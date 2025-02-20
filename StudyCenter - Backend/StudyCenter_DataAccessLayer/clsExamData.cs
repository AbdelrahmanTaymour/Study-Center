using StudyCenter_DataAccessLayer.DTOs.ExamDTOs;
using StudyCenter_DataAccessLayer.Global_Classes;
using StudyCenter_DataAccessLayer.Mappings;

namespace StudyCenter_DataAccessLayer;

public class clsExamData
{
    public static ExamDto? GetInfoById(int? examId)
        => clsDataAccessHelper.GetBy("SP_Exams_GetExamInfoByID", "ExamID", examId, ExamMapping.MapToExamDto);

    public static int? Add(ExamCreationDto newExamDto)
        => clsDataAccessHelper.Add("SP_Exams_AddNewExam", "NewExamID", newExamDto);

    public static bool Update(ExamDto updatedExamDto)
        => clsDataAccessHelper.Update("SP_Exams_UpdateExamInfo", updatedExamDto);

    public static bool Delete(int? examId)
        => clsDataAccessHelper.Delete("SP_Exams_DeleteExam", "ExamID", examId);

    public static bool Exists(int? examId)
        => clsDataAccessHelper.Exists("SP_Exams_DoesExamExists", "ExamID", examId);

    public static bool Exists(string examName)
        => clsDataAccessHelper.Exists("SP_Exams_DoesExamExistsByExamName", "ExamName", examName);

    public static List<ExamDto> All()
        => clsDataAccessHelper.All("SP_Exams_GetAllExams", ExamMapping.MapToExamDto);
}