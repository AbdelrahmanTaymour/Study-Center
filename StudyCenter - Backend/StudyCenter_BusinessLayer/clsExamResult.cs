using StudyCenter_DataAccessLayer;
using StudyCenter_DataAccessLayer.DTOs.ExamResultDTOs;

namespace StudyCenter_BusinessLayer;

public class clsExamResult
{
    public enum enMode
    {
        AddNew = 0,
        Update = 1
    };

    public enMode Mode;

    public int? ExamResultID { get; set; }
    public int? ExamID { get; set; }
    public int? StudentID { get; set; }
    public decimal MarksObtained { get; set; }

    public clsExamResult(ExamResultDto examResultDto, enMode mode = enMode.AddNew)
    {
        ExamResultID = examResultDto.ExamResultID;
        ExamID = examResultDto.ExamID;
        StudentID = examResultDto.StudentID;
        MarksObtained = examResultDto.MarksObtained;

        Mode = mode;
    }

    public ExamResultDto ToExamResultDto()
        => new ExamResultDto(this.ExamResultID, this.ExamID, this.StudentID, this.MarksObtained);

    public ExamResultCreationDto ToExamResultCreationDto()
        => new ExamResultCreationDto(this.ExamID, this.StudentID, this.MarksObtained);

    private bool _Validate(out (bool isValid, string? errorMessage) result)
    {
        bool isValid = ValidationHelper.Validate(
            this,

            // ID Check: Ensure ExamResultID is valid if in Update mode
            idCheck: examResult => (examResult.Mode != enMode.Update) || ValidationHelper.HasValue(examResult.ExamResultID),

            // Value Check: Ensure all required properties have values
            valueCheck: examResult => ValidationHelper.HasValue(examResult.ExamID) &&
                                    ValidationHelper.HasValue(examResult.StudentID),

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
        ExamResultID = clsExamResultData.Add(this.ToExamResultCreationDto());
        return (ExamResultID.HasValue);
    }

    private bool _Update() => clsExamResultData.Update(this.ToExamResultDto());

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

    public static List<ExamResultDto> All() => clsExamResultData.All();
    
    public static List<ExamResultDto> GetStudentExamResult(int studentId)
        => clsExamResultData.GetStudentExamResult(studentId);
    public static clsExamResult? Find(int examResultId)
    {
        ExamResultDto? examResultDto = clsExamResultData.Find(examResultId);
        return (examResultDto != null) ? new clsExamResult(examResultDto, enMode.Update) : null;
    }

    public static bool Exists(int examResultId) => clsExamResultData.Exists(examResultId);

    public static bool Delete(int examResultId) => clsExamResultData.Delete(examResultId);
}