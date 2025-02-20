using StudyCenter_DataAccessLayer;
using StudyCenter_DataAccessLayer.DTOs.SubjectDTOs;

namespace StudyCenter_BusinessLayer;

public class clsSubject
{
    public enum enMode
    {
        AddNew = 0,
        Update = 1
    };

    public enMode Mode;

    private string _oldSubjectName = string.Empty;
    private string _subjectName = string.Empty;
    public int? SubjectID { get; set; }

    public string SubjectName
    {
        get => _subjectName;

        set
        {
            // If the old SubjectName is not set (indicating either a new user or the SubjectName is being set for the first time),
            // initialize it with the current SubjectName value to track changes.
            if (string.IsNullOrWhiteSpace(_oldSubjectName))
            {
                _oldSubjectName = _subjectName;
            }

            _subjectName = value;
        }
    }

    public clsSubject(SubjectDto subjectDto, enMode mode = enMode.AddNew)
    {
        this.SubjectID = subjectDto.SubjectId;
        this.SubjectName = subjectDto.SubjectName;

        this.Mode = mode;
    }

    public SubjectDto ToSubjectDto() => new SubjectDto(this.SubjectID, this.SubjectName);

    private bool _Validate(out (bool isValid, string? errorMessage) result)
    {
        bool isValid = ValidationHelper.Validate(
            this,

            // ID Check: Ensure SubjectID is valid if in Update mode
            idCheck: subject => (Mode != enMode.Update || ValidationHelper.HasValue(subject.SubjectID)),

            // Value Check: Ensure SubjectName is not empty
            valueCheck: subject => ValidationHelper.IsNotEmpty(subject.SubjectName),

            // Date Validation: No date provided for verification.
            dateCheck: null,

            // Retrieve the error message returned by the validation method, if available.
            // This message provides specific details about the validation failure.
            out string? errorMessage,

            // Additional Checks: Perform miscellaneous validations and return corresponding error messages.
            additionalChecks: new (Func<clsSubject, bool>, string)[]
            {
                // Check if the SubjectName already exists in the database
                (subject => (Mode != enMode.AddNew && _oldSubjectName.Equals(subject.SubjectName)) ||
                            !Exists(subject.SubjectName), "Subject name already exists.")
            }
        );

        // Package the overall validation outcome and its corresponding error message into the output tuple.
        result = (isValid, errorMessage);

        return isValid;
    }

    private bool _Add()
    {
        this.SubjectID = clsSubjectData.Add(new SubjectCreationDto(this.SubjectName));
        return (SubjectID.HasValue);
    }

    private bool _Update() => clsSubjectData.Update(this.ToSubjectDto());

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

    public static List<SubjectDto> All()
        => clsSubjectData.All();

    public static List<SubjectCreationDto> AllNames()
        => clsSubjectData.AllNames();
    
    public static string? GetSubjectNameBySubjectId(int? subjectId)
        => clsSubjectData.GetSubjectNameBySubjectID(subjectId);

    public static int? GetSubjectId(string subjectName)
        => clsSubjectData.GetSubjectID(subjectName);
    
    public static bool Exists(int? subjectId)
        => clsSubjectData.Exists(subjectId);

    public static bool Exists(string subjectName)
        => clsSubjectData.Exists(subjectName);
    
    public static clsSubject? Find(int? subjectId)
    {
        SubjectDto? subjectDto = clsSubjectData.GetInfoById(subjectId);
        return (subjectDto != null) ? new clsSubject(subjectDto, enMode.Update) : null;
    }

    public static bool Delete(int? subjectId)
        => clsSubjectData.Delete(subjectId);

}