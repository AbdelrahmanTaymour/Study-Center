using StudyCenter_DataAccessLayer;
using StudyCenter_DataAccessLayer.DTOs.StudentDTOs;

namespace StudyCenter_BusinessLayer;

public class clsStudent
{
    public enum enMode
    {
        AddNew = 0,
        Update = 1
    };

    public enMode Mode = enMode.AddNew;


    private int? _oldPersonID;
    private int? _personID;
    private clsPerson? _personInfo;
    private clsGradeLevel? _gradeLevel;
    private clsUser? _createdByUserInfo;

    public int? StudentID { get; set; }

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

    public int? GradeLevelID { get; set; }
    public bool Status { get; set; }
    public string? Notes { get; set; }
    public int? CreatedByUserID { get; set; }
    public DateTime? CreationDate { get; set; }

    public clsPerson? PersonInfo
    {
        get
        {
            if (_personInfo == null && PersonID.HasValue)
                _personInfo = clsPerson.Find(PersonID.Value);
            return _personInfo;
        }
    }

    public clsGradeLevel? GradeLevelInfo
    {
        get
        {
            if (_gradeLevel == null && GradeLevelID.HasValue)
                _gradeLevel = clsGradeLevel.Find(GradeLevelID.Value);
            return _gradeLevel;
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

    public clsStudent(StudentDto student, enMode mode = enMode.AddNew)
    {
        this.StudentID = student.StudentId;
        this.PersonID = student.PersonId;
        this.GradeLevelID = student.GradeLevelId;
        this.Status = student.Status;
        this.Notes = student.Notes;
        this.CreatedByUserID = student.CreatedByUserId;
        this.CreationDate = student.CreationDate;
        Mode = mode;
    }

    public StudentDto ToStudentDto() => new StudentDto(this.StudentID, this.PersonID, this.GradeLevelID, this.Status,
        this.Notes, this.CreatedByUserID, this.CreationDate);

    public StudentUpdateDto ToStudentUpdateDto() =>
        new StudentUpdateDto(this.StudentID, this.PersonID, this.GradeLevelID, this.Status, this.Notes);

    private bool _Validate(out (bool isValid, string? errorMessage) result)
    {
        bool isValid = ValidationHelper.Validate(
            this,

            // ID Check: Ensure StudentID is valid if in Update mode
            idCheck: student => (Mode != enMode.Update || ValidationHelper.HasValue(student.StudentID)),

            // Value Check: Ensure required properties are not null or empty
            valueCheck: student => (ValidationHelper.HasValue(student.PersonID) && student.PersonID > 0) &&
                                   (ValidationHelper.HasValue(student.GradeLevelID) && student.GradeLevelID > 0) &&
                                   (ValidationHelper.HasValue(student.CreatedByUserID) && student.CreatedByUserID > 0),

            // Date Check: Ensure CreationDate is not in the future if in AddNew mode
            dateCheck: student => student.CreationDate == null ||(Mode != enMode.AddNew ||
                                   ValidationHelper.IsDateValid(student.CreationDate, DateTime.Now)),

            // Retrieve the error message returned by the validation method, if available.
            // This message provides specific details about the validation failure.
            out string? errorMessage,

            // Additional Checks: Perform miscellaneous validations and return corresponding error messages.
            additionalChecks: new (Func<clsStudent, bool>, string)[]
            {
                // Check if Student already exists, considering mode and previous value
                (student => (Mode != enMode.AddNew && _oldPersonID == student.PersonID) ||
                            !IsStudent(student.PersonID), "Student already exists."),
            }
        );

        // Package the overall validation outcome and its corresponding error message into the output tuple.
        result = (isValid, errorMessage);

        return isValid;
    }


    private bool _Add()
    {
        this.StudentID = clsStudentData.Add(new StudentCreationDto(this.PersonID, this.GradeLevelID, this.Status,
            this.Notes, this.CreatedByUserID));
        return this.StudentID.HasValue;
    }

    private bool _Update() => clsStudentData.Update(this.ToStudentUpdateDto());
    
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

    public static clsStudent? FindById(int? studentID)
    {
        StudentDto? studentDto = clsStudentData.GetInfoByStudentId(studentID);
        return (studentDto != null) ? new clsStudent(studentDto, enMode.Update) : null;
    }

    public static clsStudent? FindByPersonId(int? personID)
    {
        StudentDto? studentDto = clsStudentData.GetInfoByPersonId(personID);
        return (studentDto != null) ? new clsStudent(studentDto, enMode.Update) : null;
    }

    public static bool Delete(int? studentID, int? deletedByUserID)
        => clsStudentData.Delete(studentID, deletedByUserID);

    public static bool Exists(int? studentID)
        => clsStudentData.Exists(studentID);

    public static List<StudentDto> All() => clsStudentData.All();

    public static bool IsStudent(int? personID)
        => clsStudentData.IsStudent(personID);
}