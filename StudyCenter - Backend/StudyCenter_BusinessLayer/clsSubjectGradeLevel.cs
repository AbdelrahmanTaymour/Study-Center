using StudyCenter_DataAccessLayer;
using StudyCenter_DataAccessLayer.DTOs.SubjectGradeLevelDTOs;

namespace StudyCenter_BusinessLayer;

public class clsSubjectGradeLevel
{
    public enum enMode
    {
        AddNew = 0,
        Update = 1
    };

    public enMode Mode;

    private int? _oldSubjectID = null;
    private int? _subjectID = null;
    private int? _oldGradeLevelID = null;
    private int? _gradeLevelID = null;

    public int? SubjectGradeLevelID { get; set; }

    public int? SubjectID
    {
        get => _subjectID;

        set
        {
            if (!_oldSubjectID.HasValue)
            {
                _oldSubjectID = _subjectID;
            }

            _subjectID = value;
        }
    }

    public int? GradeLevelID
    {
        get => _gradeLevelID;

        set
        {
            if (!_oldGradeLevelID.HasValue)
            {
                _oldGradeLevelID = _gradeLevelID;
            }

            _gradeLevelID = value;
        }
    }

    public decimal Fees { get; set; }

    public bool IsMandatory { get; set; }
    public string? Description { get; set; }

    public clsSubjectGradeLevel(SubjectGradeLevelDto subjectGradeLevelDto, enMode mode = enMode.AddNew)
    {
        SubjectGradeLevelID = subjectGradeLevelDto.SubjectGradeLevelId;
        SubjectID = subjectGradeLevelDto.SubjectId;
        GradeLevelID = subjectGradeLevelDto.GradeLevelId;
        Fees = subjectGradeLevelDto.Fees;
        IsMandatory = subjectGradeLevelDto.IsMandatory;
        Description = subjectGradeLevelDto.Description;

        Mode = mode;
    }

    public SubjectGradeLevelCreationDto ToSubjectGradeLevelCreationDto()
        => new SubjectGradeLevelCreationDto(this.SubjectID, this.GradeLevelID, this.Fees, this.IsMandatory,
            this.Description);

    public SubjectGradeLevelDto ToSubjectGradeLevelDto()
        => new SubjectGradeLevelDto(this.SubjectGradeLevelID, this.SubjectID, this.GradeLevelID, this.Fees,
            this.IsMandatory, this.Description);

    private bool _Validate(out (bool isValid, string? errorMessage) result)
    {
        bool isValid = ValidationHelper.Validate(
            this,

            // ID Check: Ensure SubjectGradeLevelID is valid if in Update mode
            idCheck: sgl => (Mode != enMode.Update || ValidationHelper.HasValue(sgl.SubjectGradeLevelID)),

            // Value Check: Ensure SubjectID and GradeLevelID are provided, and Fees is non-negative
            valueCheck: sgl => ValidationHelper.HasValue(sgl.SubjectID) &&
                               ValidationHelper.HasValue(sgl.GradeLevelID) && sgl.Fees >= 0,

            // Date Validation: No date provided for verification.
            dateCheck: null,

            // Retrieve the error message returned by the validation method, if available.
            // This message provides specific details about the validation failure.
            out string? errorMessage,

            // Additional Checks: Check various conditions and provide corresponding error messages
            additionalChecks: new (Func<clsSubjectGradeLevel, bool>, string)[]
            {
                // Check if the combination of SubjectID and GradeLevelID already exists in the database
                ((sgl) => !((Mode == enMode.AddNew || sgl._oldSubjectID != sgl._subjectID ||
                             sgl._oldGradeLevelID != sgl._gradeLevelID) &&
                            Exists(sgl.SubjectID, sgl.SubjectGradeLevelID)),
                    "Subject grade level already exists."),
            }
        );

        // Package the overall validation outcome and its corresponding error message into the output tuple.
        result = (isValid, errorMessage);

        return isValid;
    }

    private bool _Add()
    {
        SubjectGradeLevelID = clsSubjectGradeLevelData.Add(this.ToSubjectGradeLevelCreationDto());
        return (SubjectGradeLevelID.HasValue);
    }

    private bool _Update() => clsSubjectGradeLevelData.Update(this.ToSubjectGradeLevelDto());


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

    public static List<SubjectGradeLevelDto> All() => clsSubjectGradeLevelData.All();

    public static clsSubjectGradeLevel? Find(int? subjectGradeLevelId)
    {
        SubjectGradeLevelDto? subjectGradeLevelDto = clsSubjectGradeLevelData.GetInfoById(subjectGradeLevelId);
        return (subjectGradeLevelDto != null) ? new clsSubjectGradeLevel(subjectGradeLevelDto, enMode.Update) : null;
    }

    public static bool Delete(int? subjectGradeLevelId)
        => clsSubjectGradeLevelData.Delete(subjectGradeLevelId);

    public static bool Exists(int? subjectGradeLevelId)
        => clsSubjectGradeLevelData.Exists(subjectGradeLevelId);

    public static bool Exists(int? subjectId, int? gradeLevelId)
        => clsSubjectGradeLevelData.Exists(subjectId, gradeLevelId);

}