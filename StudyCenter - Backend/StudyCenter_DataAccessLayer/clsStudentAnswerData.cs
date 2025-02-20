using StudyCenter_DataAccessLayer.DTOs.StudentAnswerDTOs;
using StudyCenter_DataAccessLayer.Global_Classes;
using StudyCenter_DataAccessLayer.Mappings;

namespace StudyCenter_DataAccessLayer;

public class clsStudentAnswerData
{
    public static List<StudentAnswerDto> All()
        => clsDataAccessHelper.All("SP_StudentAnswers_GetAllStudentAnswers", StudentAnswerMapping.MapToStudentAnswer);

    public static List<StudentAnswerDto> AllPerExamResult(int examResultId)
        => clsDataAccessHelper.All("SP_StudentAnswers_GetAllStudentAnswersPerExamResult",
            StudentAnswerMapping.MapToStudentAnswer, [("ExamResultID", examResultId)]);

    public static StudentAnswerDto? Find(int id)
        => clsDataAccessHelper.GetBy("SP_StudentAnswers_GetStudentAnswerInfoByID", "StudentAnswerID", id,
            StudentAnswerMapping.MapToStudentAnswer);

    public static bool Exists(int id)
        => clsDataAccessHelper.Exists("SP_StudentAnswers_DoesStudentAnswerExists", "StudentAnswerID", id);

    public static bool IsAnswered(int? examResultId, int? questionId)
        => clsDataAccessHelper.Exists("SP_StudentAnswers_DoesStudentAnswerExistsByExamResultIDAndQuestionID",
            "ExamResultID", examResultId, "QuestionID", questionId);

    public static int? Add(StudentAnswerCreationDto newStudentAnswerDto)
        => clsDataAccessHelper.Add("SP_StudentAnswers_AddNewStudentAnswer", "NewStudentAnswerID", newStudentAnswerDto);
    
    public static bool Update(StudentAnswerDto updateStudentAnswerDto)
        => clsDataAccessHelper.Update("SP_StudentAnswers_UpdateStudentAnswerInfo", updateStudentAnswerDto);
    
    public static bool Delete(int? id)
        => clsDataAccessHelper.Delete("SP_StudentAnswers_DeleteStudentAnswer", "StudentAnswerID", id);
}