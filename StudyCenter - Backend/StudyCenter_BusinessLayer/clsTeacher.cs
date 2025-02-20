using StudyCenter_DataAccessLayer;
using StudyCenter_DataAccessLayer.DTOs.TeacherDTOs;

namespace StudyCenter_BusinessLayer;

public class clsTeacher
{
    public enum enMode { AddNew = 0, Update = 1 };
    public enMode Mode = enMode.AddNew;

    private int? _oldPersonID = null;
    private int? _personID = null;
    private clsPerson? _personInfo;
    private clsEducationLevel? _educationLevel;
    private clsUser? _createdByUserInfo;

    public int? TeacherID { get; set; }
    public int? PersonID
    {
        get => _personID;

        set
        {
            if (!_oldPersonID.HasValue)
            {
                _oldPersonID = _personID;
            }

            _personID = value;
        }
    }
    public int? EducationLevelID { get; set; }
    public byte? TeachingExperience { get; set; }
    public string? Certifications { get; set; }
    public bool Status { get; set; }
    public string? Notes { get; set; }
    public int? CreatedByUserID { get; set; }

    public clsPerson? PersonInfo
    {
        get
        {
            if (_personInfo == null && PersonID.HasValue)
                _personInfo = clsPerson.Find(PersonID.Value);
            return _personInfo;
        }
    }
    public clsUser? CreatedByUserInfo
    {
        get
        {
            if (_createdByUserInfo == null && CreatedByUserID.HasValue)
                _createdByUserInfo = clsUser.FindByID(CreatedByUserID.Value);
            return _createdByUserInfo;
        }
    }
    public clsEducationLevel? EducationLevelInfo
    {
        get
        {
            if (_educationLevel == null && EducationLevelID.HasValue)
                _educationLevel = clsEducationLevel.Find(EducationLevelID.Value);
            return _educationLevel;
        }
    }

    public clsTeacher(TeacherDto teacher, enMode mode = enMode.AddNew)
    {
        TeacherID = teacher.TeacherId;
        PersonID = teacher.PersonId;
        EducationLevelID = teacher.EducationLevelId;
        TeachingExperience = teacher.TeachingExperience;
        Certifications = teacher.Certifications;
        Status = teacher.Status;
        Notes = teacher.Notes;
        CreatedByUserID = teacher.CreatedByUserId;

        Mode = mode;
    }


    public TeacherDto ToTeacherDto() => new TeacherDto(this.TeacherID, this.PersonID,
        this.EducationLevelID, this.TeachingExperience, this.Certifications, this.Status, this.Notes,
        this.CreatedByUserID);

    public TeacherUpdateDto ToTeachingUpdateDto() => new TeacherUpdateDto(this.TeacherID, this.PersonID,
        this.EducationLevelID, this.TeachingExperience, this.Certifications, this.Status, this.Notes);

    private bool _Validate(out (bool isValid, string? errorMessage) result)
    {
        bool isValid = ValidationHelper.Validate(
            this,

            // ID Check: Ensure TeacherID is valid if in Update mode
            idCheck: teacher => (Mode != enMode.Update || ValidationHelper.HasValue(teacher.TeacherID)),

            // Value Check: Ensure PersonID, EducationLevelID, and CreatedByUserID, and other required fields are provided
            valueCheck: teacher => ValidationHelper.HasValue(teacher.PersonID) &&
                                   ValidationHelper.HasValue(teacher.EducationLevelID) &&
                                   ValidationHelper.HasValue(teacher.CreatedByUserID),

            // Date Validation: No date provided for verification.
            dateCheck: null,

            // Retrieve the error message returned by the validation method, if available.
            // This message provides specific details about the validation failure.
            out string? errorMessage,

            // Additional Checks: Check various conditions and provide corresponding error messages
            additionalChecks: new (Func<clsTeacher, bool>, string)[]
            {
                // Check if PersonID already exists as a teacher, considering mode and previous value
                (teacher => (Mode != enMode.AddNew && _oldPersonID == teacher.PersonID) ||
                            !clsTeacher.IsTeacher(teacher.PersonID), "Teacher already exists."),
            }
        );

        // Package the overall validation outcome and its corresponding error message into the output tuple.
        result = (isValid, errorMessage);

        return isValid;
    }

    private bool _Add()
    {
        TeacherID = clsTeacherData.Add((new TeacherCreationDto(this.PersonID, this.EducationLevelID,
            this.TeachingExperience, this.Certifications, this.Status, this.Notes, this.CreatedByUserID)));

        return (TeacherID.HasValue);
    }

    private bool _Update() => clsTeacherData.Update(this.ToTeachingUpdateDto());

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

    public static List<TeacherDto> All() => clsTeacherData.All();

    public static clsTeacher? FindByTeacherId(int? teacherId)
    {
        TeacherDto? teacherDto = clsTeacherData.GetInfoByTeacherId(teacherId);
        return (teacherDto != null) ? new clsTeacher(teacherDto, enMode.Update) : null;
    }

    public static clsTeacher? FindByPersonId(int? personId)
    {
        TeacherDto? teacherDto = clsTeacherData.GetInfoByPersonId(personId);
        return (teacherDto != null) ? new clsTeacher(teacherDto, enMode.Update) : null;
    }

    public static bool Exists(int? teacherId) => clsTeacherData.Exists(teacherId);

    public static string? GetFullName(int? teacherId) => clsTeacherData.GetFullName(teacherId);
    public static bool IsTeacher(int? personId) => clsTeacherData.IsTeacher(personId);

    public static bool Delete(int? teacherId, int? deletedByUserId) =>
        clsTeacherData.Delete(teacherId, deletedByUserId);

}