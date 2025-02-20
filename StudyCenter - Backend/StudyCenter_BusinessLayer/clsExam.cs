using StudyCenter_DataAccessLayer;
using StudyCenter_DataAccessLayer.DTOs.ExamDTOs;

namespace StudyCenter_BusinessLayer;

public class clsExam
{
    public enum enMode
    {
        AddNew = 0,
        Update = 1
    };

    public enMode Mode;

    public int? ExamID { get; set; }
    public int? SubjectGradeLevelID { get; set; }

    private string? _oldExamName = string.Empty;
    private string _examName = string.Empty;

    public string ExamName
    {
        get => _examName;

        set
        {
            // If the old ExamName is not set (indicating either a new user or the ExamName is being set for the first time),
            // initialize it with the current ExamName value to track changes.
            
            if (string.IsNullOrWhiteSpace(_oldExamName))
            {
                _oldExamName = _examName;
            }
            _examName = value;
        }
    }
    public DateTime ExamDate { get; set; }
    public decimal TotalMarks { get; set; }
    public decimal PassingMarks { get; set; }

    public clsExam(ExamDto examDto, enMode mode = enMode.AddNew)
    {
        ExamID = examDto.ExamID;
        SubjectGradeLevelID = examDto.SubjectGradeLevelID;
        ExamName = examDto.ExamName;
        ExamDate = examDto.ExamDate;
        TotalMarks = examDto.TotalMarks;
        PassingMarks = examDto.PassingMarks;

        Mode = mode;
    }

    public ExamDto ToExamDto()
        => new ExamDto(this.ExamID, this.SubjectGradeLevelID, this.ExamName, this.ExamDate, this.TotalMarks,
            this.PassingMarks);

    public ExamCreationDto ToExamCreationDto()
        => new ExamCreationDto(this.SubjectGradeLevelID, this.ExamName, this.ExamDate, this.TotalMarks,
            this.PassingMarks);


    private bool _Validate(out (bool isValid, string? errorMessage) result)
    {
        bool isValid = ValidationHelper.Validate(
            this,
            idCheck: exam => (exam.Mode != enMode.Update) || ValidationHelper.HasValue(exam.ExamID),

            // Value Check: Ensure all required properties have values and that ExamName is not empty if in Update mode
            valueCheck: exam => ValidationHelper.HasValue(exam.SubjectGradeLevelID) &&
                                (exam.Mode != enMode.Update || !string.IsNullOrWhiteSpace(exam.ExamName)) &&
                                exam is { TotalMarks: > 0, PassingMarks: > 0 },

            // Date Check: Ensure ExamDate is valid
            dateCheck: exam =>
                (exam.Mode != enMode.AddNew || !ValidationHelper.IsDateValid(exam.ExamDate, DateTime.Now)),

            // Retrieve the error message returned by the validation method, if available.
            // This message provides specific details about the validation failure.
            out string? errorMessage,

            // Additional Checks: Check various conditions and provide corresponding error messages
            additionalChecks: new (Func<clsExam, bool>, string)[]
            {
                // Check if the ExamName already exists in the database
                (exam => (Mode != enMode.AddNew && _oldExamName.Equals(exam.ExamName)) ||
                            !Exists(exam.ExamName), "Exam name already exists.")
            }
        );

        // Package the overall validation outcome and its corresponding error message into the output tuple.
        result = (isValid, errorMessage);

        return isValid;
    }

    private bool _Add()
    {
        ExamID = clsExamData.Add(this.ToExamCreationDto());
        return (ExamID.HasValue);
    }

    private bool _Update() => clsExamData.Update(this.ToExamDto());

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

    public static clsExam? Find(int? examId)
    {
        ExamDto? examDto = clsExamData.GetInfoById(examId);
        return (examDto != null) ? new clsExam(examDto, enMode.Update) : null;
    }

    public static bool Delete(int? examId)
        => clsExamData.Delete(examId);

    public static bool Exists(int? examId)
        => clsExamData.Exists(examId);

    public static bool Exists(string examName)
        => clsExamData.Exists(examName);

    public static List<ExamDto> All()
        => clsExamData.All();
}