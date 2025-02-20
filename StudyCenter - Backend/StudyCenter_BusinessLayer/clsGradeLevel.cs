using StudyCenter_DataAccessLayer;
using StudyCenter_DataAccessLayer.DTOs.GradeLevelDTOs;

namespace StudyCenter_BusinessLayer;

public class clsGradeLevel
{
    public enum enMode { AddNew = 0, Update = 1 };
    public enMode Mode = enMode.AddNew;
    
    public int? GradeLevelID { get; set; }
    
    private string? _oldGradeName = string.Empty;
    private string? _gradeName = string.Empty;
    public string? GradeName
    {
        get => _gradeName;

        set
        {
            // If the old GradeName is not set (indicating either a new user or the GradeName is being set for the first time),
            // initialize it with the current GradeName value to track changes.
            if (string.IsNullOrWhiteSpace(_oldGradeName))
            {
                _oldGradeName = _gradeName;
            }

            _gradeName = value;
        }
    }

    public clsGradeLevel(GradeLevelDto gradeLevelDto, enMode mode = enMode.AddNew)
    {
        this.GradeLevelID = gradeLevelDto.GradeLevelID;
        this.GradeName = gradeLevelDto.GradeName;
        
        this.Mode = mode;
    }
    public GradeLevelDto ToGradeLevelDto() => new GradeLevelDto(this.GradeLevelID, this.GradeName);
    
    private bool _Validate(out (bool isValid, string? errorMessage) result)
    {
        bool isValid = ValidationHelper.Validate
        (
            this,
            
            // ID Check: Ensure GradeLevelID is valid if in Update mode
            idCheck: gradeLevel => (Mode != clsGradeLevel.enMode.Update) || ValidationHelper.HasValue(gradeLevel.GradeLevelID),
            
            // Value Check: Ensure required properties are not null or empty
            valueCheck: gradeLevel => ValidationHelper.IsNotEmpty(gradeLevel.GradeName),
            
            // Date Validation: No date provided for verification.
            dateCheck: null,
                                  
            // Retrieve the error message returned by the validation method, if available.
            out string? errorMessage,
            
            // Additional Checks: Perform miscellaneous validations and return corresponding error messages.
            additionalChecks: new (Func<clsGradeLevel, bool>, string )[]
            {
                // Check if Garde Level Name already exists, considering mode and previous value
                (gradeLevel => (Mode != enMode.AddNew && _oldGradeName == gradeLevel.GradeName) || 
                               !Exists(gradeLevel.GradeName), "Grade Name already exists."),
            }
        );
        
        // Package the validation result and message into the output tuple.
        result = (isValid, errorMessage);
        return isValid;
    }

    private bool _Add()
    {
        this.GradeLevelID = clsGradeLevelData.Add(new GradeLevelCreationDto(this.GradeName));
        return this.GradeLevelID.HasValue;
    }

    private bool _Update()
    {
        var gradeLevelId = this.GradeLevelID;
        return gradeLevelId != null &&
               clsGradeLevelData.Update(new GradeLevelDto(gradeLevelId.Value, this.GradeName));
    }

    
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
    
    public static clsGradeLevel? Find(int? gradeLevelId)
    {
        GradeLevelDto? gradeLevelDto = clsGradeLevelData.GetInfoById(gradeLevelId);
        return (gradeLevelDto != null) ? new clsGradeLevel(gradeLevelDto, enMode.Update) : null;
    }

    public static List<GradeLevelDto> GetAllGradeLevels() => clsGradeLevelData.GetAllGradeLevels();
    
    public static List<GradeLevelCreationDto> GetAllGradeLevelsName() => clsGradeLevelData.GetAllGradeLevelsName();
    
    public static int? GetGradeLevelId(string gradeName) => clsGradeLevelData.GetGradeLevelId(gradeName);
    
    public static bool Delete(int? gradeLevelId) => clsGradeLevelData.Delete(gradeLevelId);
    
    public static bool Exists(int? gradeLevelId) => clsGradeLevelData.Exists(gradeLevelId);
    
    public static bool Exists(string? gradeName) => clsGradeLevelData.Exists(gradeName);
    
    
    
}