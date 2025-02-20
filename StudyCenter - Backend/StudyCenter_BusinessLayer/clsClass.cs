using System.Data;
using StudyCenter_DataAccessLayer;
using StudyCenter_DataAccessLayer.DTOs.ClassDTOs;

namespace StudyCenter_BusinessLayer;

public class clsClass
{
    public enum enMode
    {
        AddNew = 0,
        Update = 1
    };
    public enMode Mode;

    private string _className = string.Empty;
    private string _oldClassName = string.Empty;

    public int? ClassId { get; set; }

    public string ClassName
    {
        get => _className;

        set
        {
            // If the old ClassName is not set (indicating either a new user or the ClassName is being set for the first time),
            // initialize it with the current ClassName value to track changes.
            if (string.IsNullOrWhiteSpace(_oldClassName))
            {
                _oldClassName = _className;
            }

            _className = value;
        }
    }

    public byte Capacity { get; set; }
    public string? Description { get; set; }

    public clsClass(ClassDto classDto, enMode mode = enMode.AddNew)
    {
        ClassId = classDto.ClassId;
        ClassName = classDto.ClassName;
        Capacity = classDto.Capacity;
        Description = classDto.Description;

        Mode = mode;
    }

    public ClassDto ToClassDto() => new ClassDto(this.ClassId, this.ClassName, this.Capacity, this.Description);

    private bool _Validate(out (bool isValid, string? errorMessage) result)
    {
        bool isValid = ValidationHelper.Validate
        (
            this,

            // ID Check: Ensure ClassID is valid if in Update mode
            idCheck: c => (c.Mode != enMode.Update) || ValidationHelper.HasValue(c.ClassId),

            // Value Check: Ensure ClassName is not empty and Capacity is positive
            valueCheck: c => !string.IsNullOrWhiteSpace(c.ClassName) && c.Capacity > 0,

            // Date Validation: No date provided for verification.
            dateCheck: null,

            // Retrieve the error message returned by the validation method, if available.
            // This message provides specific details about the validation failure.
            out var errorMessage,

            // Additional Checks: Ensure ClassName does not already exist in the database
            additionalChecks: new (Func<clsClass, bool>, string)[]
            {
                (c => (c.Mode != enMode.AddNew && _oldClassName.Equals(c.ClassName)) || !Exists(c.ClassName),
                    "Class name already exists.")
            }
        );

        // Package the overall validation outcome and its corresponding error message into the output tuple.
        result = (isValid, errorMessage);

        return isValid;
    }

    private bool _Add()
    {
        this.ClassId = clsClassData.Add(new ClassCreationDto(this.ClassName, this.Capacity, this.Description));
        return (this.ClassId.HasValue);
    }

    private bool _Update() => clsClassData.Update(this.ToClassDto());


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

    public static List<ClassDto> All() => clsClassData.All();

    public static clsClass? Find(int? classId)
    {
        ClassDto? classDto = clsClassData.GetInfoById(classId);
        return (classDto != null) ? new clsClass(classDto, enMode.Update) : null;
    }

    public static bool Delete(int? classId) => clsClassData.Delete(classId);

    public static bool Exists(int? classId) => clsClassData.Exists(classId);


    public static bool Exists(string className) => clsClassData.Exists(className);
}