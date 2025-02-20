using StudyCenter_DataAccessLayer;
using StudyCenter_DataAccessLayer.DTOs.StudentAnswerDTOs;

namespace StudyCenter_BusinessLayer;

public class clsStudentAnswer
{
    public enum enMode
    {
        AddNew = 0,
        Update = 1
    };

    public enMode Mode;

    public int? StudentAnswerID { get; set; }
    public int? ExamResultID { get; set; }
    public int? QuestionID { get; set; }
    public int? AnswerID { get; set; }
    public string? AnswerText { get; set; }
    public decimal? MarksAwarded { get; set; }

    public clsStudentAnswer(StudentAnswerDto studentAnswerDto, enMode mode = enMode.AddNew)
    {
        StudentAnswerID = studentAnswerDto.StudentAnswerID;
        ExamResultID = studentAnswerDto.ExamResultID;
        QuestionID = studentAnswerDto.QuestionID;
        AnswerID = studentAnswerDto.AnswerID;
        AnswerText = studentAnswerDto.AnswerText;
        MarksAwarded = studentAnswerDto.MarksAwarded;

        Mode = mode;
    }

    public StudentAnswerDto ToStudentAnswerDto()
        => new StudentAnswerDto(this.StudentAnswerID, this.ExamResultID, this.QuestionID, this.AnswerID,
            this.AnswerText, this.MarksAwarded);

    public StudentAnswerCreationDto ToStudentAnswerCreationDto()
        => new StudentAnswerCreationDto(this.ExamResultID, this.QuestionID, this.AnswerID, this.AnswerText,
            this.MarksAwarded);

    private bool _Validate(out (bool isValid, string? errorMessage) result)
    {
        bool isValid = ValidationHelper.Validate(
            this,

            // ID Check: Ensure AnswerID is valid if in Update mode
            idCheck: sa => (sa.Mode != enMode.Update) || ValidationHelper.HasValue(sa.StudentAnswerID),

            // Value Check: Ensure all required properties have values
            valueCheck: sa => ValidationHelper.HasValue(sa.QuestionID) &&
                              ValidationHelper.HasValue(sa.ExamResultID) &&
                              ValidationHelper.HasValue(sa.QuestionID) &&
                              (ValidationHelper.HasValue(sa.AnswerID) || !string.IsNullOrWhiteSpace(sa.AnswerText)),

            // Date Validation: No date provided for verification.
            dateCheck: null,

            // Retrieve the error message returned by the validation method, if available.
            // This message provides specific details about the validation failure.
            out string? errorMessage,

            // Additional Checks: Check various conditions and provide corresponding error messages
            additionalChecks: new (Func<clsStudentAnswer, bool>, string)[]
            {
                (sa => (Mode != enMode.AddNew) || 
                       !IsAnswered(sa.ExamResultID, sa.QuestionID), "The question is already answered.")
            }
        );
        // Package the overall validation outcome and its corresponding error message into the output tuple.
        result = (isValid, errorMessage);
        
        return isValid;
    }
    
    private bool _Add()
    {
        this.StudentAnswerID = clsStudentAnswerData.Add(this.ToStudentAnswerCreationDto());
        return this.StudentAnswerID.HasValue;
    }
    private bool _Update() => clsStudentAnswerData.Update(this.ToStudentAnswerDto());
    
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
    
    public static List<StudentAnswerDto> All() => clsStudentAnswerData.All();
    public static List<StudentAnswerDto> AllPerExamResult(int examResultId) => clsStudentAnswerData.AllPerExamResult(examResultId);

    public static clsStudentAnswer? Find(int id)
    {
        StudentAnswerDto? studentAnswerDto = clsStudentAnswerData.Find(id);
        return (studentAnswerDto != null) ? new clsStudentAnswer(studentAnswerDto, enMode.Update) : null;
    }
    
    public static bool Exists(int id) => clsStudentAnswerData.Exists(id);
    public static bool IsAnswered(int? examResultId, int? questionId)
        => clsStudentAnswerData.IsAnswered(examResultId, questionId);
    
    public static bool Delete(int id) => clsStudentAnswerData.Delete(id);
}