using StudyCenter_DataAccessLayer.DTOs.ExamResultDTOs;
using StudyCenter_DataAccessLayer.Global_Classes;
using StudyCenter_DataAccessLayer.Mappings;

namespace StudyCenter_DataAccessLayer;

public class clsExamResultData
{
    public static List<ExamResultDto> All()
        => clsDataAccessHelper.All("SP_ExamResults_GetAllExamResults", ExamResultMapping.MapToExamResultDto);
    
    public static List<ExamResultDto> GetStudentExamResult(int studentId)
        => clsDataAccessHelper.All("SP_ExamResults_GetStudentExamResults", ExamResultMapping.MapToExamResultDto, [("StudentID", studentId)]);
    
    public static ExamResultDto? Find(int examResultId)
        => clsDataAccessHelper.GetBy("SP_ExamResults_GetExamResultInfoByID", "ExamResultID", examResultId, ExamResultMapping.MapToExamResultDto);
    
    public static bool Exists(int examResultId)
        => clsDataAccessHelper.Exists("SP_ExamResults_DoesExamResultExists", "ExamResultID", examResultId);

    public static int? Add(ExamResultCreationDto newExamResult)
        => clsDataAccessHelper.Add("SP_ExamResults_AddNewExamResult", "NewExamResultID", newExamResult);

    public static bool Update(ExamResultDto updatedExamResult)
        => clsDataAccessHelper.Update("SP_ExamResults_UpdateExamResultInfo", updatedExamResult);
    
    public static bool Delete(int examResultId)
        => clsDataAccessHelper.Delete("SP_ExamResults_DeleteExamResult", "ExamResultID", examResultId);
}