using System.Runtime.CompilerServices;
using StudyCenter_DataAccessLayer;
using StudyCenter_DataAccessLayer.DTOs.QuestionDTOs;
using StudyCenter_DataAccessLayer.Global_Classes;

namespace StudyCenter_BusinessLayer;

public class clsQuestion
{
    public enum enMode
    {
        AddNew = 0,
        Update = 1
    };

    public enMode Mode;

    public enum enQuestionType
    {
        MultipleChoice = 1,
        TrueFalse = 2,
        Essay = 3
    }

    public int? QuestionID { get; set; }
    public int? ExamID { get; set; }
    public string QuestionText { get; set; }
    public enQuestionType QuestionType { get; set; }
    public decimal Marks { get; set; }

    public clsQuestion(QuestionDto questionDto, enMode mode = enMode.AddNew)
    {
        QuestionID = questionDto.QuestionID;
        ExamID = questionDto.ExamID;
        QuestionText = questionDto.QuestionText;
        QuestionType = (enQuestionType)questionDto.QuestionType;
        Marks = questionDto.Marks;

        Mode = mode;
    }

    public QuestionDto ToQuestionDto()
        => new QuestionDto(this.QuestionID, this.ExamID, this.QuestionText, (byte)this.QuestionType, this.Marks);

    public QuestionCreationDto ToQuestionCreationDto()
        => new QuestionCreationDto(this.ExamID, this.QuestionText, (byte)this.QuestionType, this.Marks);

    private bool _Validate(out (bool isValid, string? errorMessage) result)
    {
        bool isValid = ValidationHelper.Validate(
            this,

            // ID Check: Ensure QuestionID is valid if in Update mode
            idCheck: question => (question.Mode != enMode.Update) || ValidationHelper.HasValue(question.QuestionID),

            // Value Check: Ensure all required properties have values
            valueCheck: question => ValidationHelper.HasValue(question.ExamID) &&
                                    (question.Mode != enMode.Update ||
                                     !string.IsNullOrWhiteSpace(question.QuestionText)) &&
                                    (question.QuestionType == enQuestionType.MultipleChoice ||
                                     question.QuestionType == enQuestionType.TrueFalse ||
                                     question.QuestionType == enQuestionType.Essay) &&
                                    question.Marks > 0,

            // Date Validation: No date provided for verification.
            dateCheck: null,

            // Retrieve the error message returned by the validation method, if available.
            // This message provides specific details about the validation failure.
            out string? errorMessage,

            // Additional Checks: Check various conditions and provide corresponding error messages
            additionalChecks: null
        );

        // Package the overall validation outcome and its corresponding error message into the output tuple.
        result = (isValid, errorMessage);

        return isValid;
    }

    private bool _Add()
    {
        QuestionID = clsQuestionData.Add(this.ToQuestionCreationDto());
        return (QuestionID.HasValue);
    }

    private bool _Update() => clsQuestionData.Update(this.ToQuestionDto());

    public bool Save(out string? validationMessage)
    {
        // Validate the current user data. If validation fails, return false with the error message.
        if (!_Validate(out (bool success, string? message) result))
        {
            validationMessage = result.message;
            return false;
        }

        // Clear the validation message if validation succeeds.
        validationMessage = string.Empty;

        switch (Mode)
        {
            case enMode.AddNew:
                if (_Add())
                {
                    Mode = enMode.Update;
                    return true;
                }

                return false;

            case enMode.Update:
                return _Update();
        }

        return false;
    }

    public static List<QuestionDto> AllQuestions() => clsQuestionData.GetAllQuestions();
    public static List<QuestionDto> AllExamQuetions(int examId) => clsQuestionData.GetAllExamQuestions(examId);

    public static clsQuestion? Find(int questionId)
    {
        QuestionDto? questionDto = clsQuestionData.Find(questionId);
        return (questionDto != null) ? new clsQuestion(questionDto, enMode.Update) : null;
    }

    public static bool Exists(int questionId) => clsQuestionData.Exists(questionId);

    public static bool Delete(int questionId) => clsQuestionData.Delete(questionId);
}