using StudyCenter_DataAccessLayer;
using StudyCenter_DataAccessLayer.DTOs.AnswerDTOs;

namespace StudyCenter_BusinessLayer;

public class clsAnswer
{
    public enum enMode
    {
        AddNew = 0,
        Update = 1
    };

    public enMode Mode;

    public int? AnswerID { get; set; }
    public int? QuestionID { get; set; }
    public string AnswerText { get; set; }
    public bool IsCorrect { get; set; }

    public clsAnswer(AnswerDto answerDto, enMode mode = enMode.AddNew)
    {
        AnswerID = answerDto.AnswerID;
        QuestionID = answerDto.QuestionID;
        AnswerText = answerDto.AnswerText;
        IsCorrect = answerDto.IsCorrect;

        Mode = mode;
    }

    public AnswerDto ToAnswerDto()
        => new AnswerDto(this.AnswerID, this.QuestionID, this.AnswerText, this.IsCorrect);

    public AnswerCreationDto ToAnswerCreationDto()
        => new AnswerCreationDto(this.QuestionID, this.AnswerText, this.IsCorrect);

    private bool _Validate(out (bool isValid, string? errorMessage) result)
    {
        bool isValid = ValidationHelper.Validate(
            this,

            // ID Check: Ensure AnswerID is valid if in Update mode
            idCheck: answer => (answer.Mode != enMode.Update) || ValidationHelper.HasValue(answer.AnswerID),

            // Value Check: Ensure all required properties have values
            valueCheck: answer => ValidationHelper.HasValue(answer.QuestionID) &&
                                    (answer.Mode != enMode.Update ||
                                     !string.IsNullOrWhiteSpace(answer.AnswerText)),
                                    

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
        AnswerID = clsAnswerData.Add(this.ToAnswerCreationDto());
        return (AnswerID.HasValue);
    }

    private bool _Update() => clsAnswerData.Update(this.ToAnswerDto());

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

    public static List<AnswerDto> AllAnswers() => clsAnswerData.GetAllAnswers();
    public static List<AnswerDto> AllQuestionAnswers(int examId) => clsAnswerData.GetAllQuestionAnswers(examId);

    public static clsAnswer? Find(int answerId)
    {
        AnswerDto? answerDto = clsAnswerData.GetAnswerById(answerId);
        return (answerDto != null) ? new clsAnswer(answerDto, enMode.Update) : null;
    }

    public static bool Exists(int answerId) => clsAnswerData.Exists(answerId);

    public static bool Delete(int answerId) => clsAnswerData.Delete(answerId);
}