using StudyCenter_DataAccessLayer.DTOs.QuestionDTOs;
using StudyCenter_DataAccessLayer.Global_Classes;
using StudyCenter_DataAccessLayer.Mappings;

namespace StudyCenter_DataAccessLayer;

public class clsQuestionData
{
    public static List<QuestionDto> GetAllQuestions()
        => clsDataAccessHelper.All("SP_Questions_GetAllQuestions", QuestionMapping.MapToQuestionDto);
    
    public static List<QuestionDto> GetAllExamQuestions(int examId)
        => clsDataAccessHelper.All("SP_Questions_GetAllExamQuestions", QuestionMapping.MapToQuestionDto, [("ExamID", examId)]);
    
    public static QuestionDto? Find(int questionId)
        => clsDataAccessHelper.GetBy("SP_Questions_GetQuestionInfoByID", "QuestionID", questionId, QuestionMapping.MapToQuestionDto);
    
    public static bool Exists(int questionId)
        => clsDataAccessHelper.Exists("SP_Questions_DoesQuestionExists", "QuestionID", questionId);

    public static int? Add(QuestionCreationDto newQuestion)
        => clsDataAccessHelper.Add("SP_Questions_AddNewQuestion", "NewQuestionID", newQuestion);

    public static bool Update(QuestionDto updatedQuestion)
        => clsDataAccessHelper.Update("SP_Questions_UpdateQuestionInfo", updatedQuestion);
    
    public static bool Delete(int questionId)
        => clsDataAccessHelper.Delete("SP_Questions_DeleteQuestion", "QuestionID", questionId);

}