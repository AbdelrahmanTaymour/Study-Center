using StudyCenter_DataAccessLayer.DTOs.AnswerDTOs;
using StudyCenter_DataAccessLayer.Global_Classes;
using StudyCenter_DataAccessLayer.Mappings;

namespace StudyCenter_DataAccessLayer;

public class clsAnswerData
{
    public static List<AnswerDto> GetAllAnswers()
        => clsDataAccessHelper.All("SP_Answers_GetAllAnswers", AnswerMapping.MapToAnswerDto);

    public static List<AnswerDto> GetAllQuestionAnswers(int questionId)
        => clsDataAccessHelper.All("SP_Answers_GetAllQuestionAnswers", AnswerMapping.MapToAnswerDto,
            [("QuestionID", questionId)]);

    public static AnswerDto? GetAnswerById(int answerId)
        => clsDataAccessHelper.GetBy("SP_Answers_GetAnswerInfoByID", "AnswerID", answerId,
            AnswerMapping.MapToAnswerDto);
    
    public static bool Exists(int answerId)
        => clsDataAccessHelper.Exists("SP_Answers_DoesAnswerExists", "AnswerID", answerId);
    
    public static int? Add(AnswerCreationDto newAnswer)
        => clsDataAccessHelper.Add("SP_Answers_AddNewAnswer", "NewAnswerID", newAnswer);

    public static bool Update(AnswerDto updatedAnswer)
        => clsDataAccessHelper.Update("SP_Answers_UpdateAnswerInfo", updatedAnswer);
    
    public static bool Delete(int id)
        => clsDataAccessHelper.Delete("SP_Answers_DeleteAnswer", "AnswerID", id);
}