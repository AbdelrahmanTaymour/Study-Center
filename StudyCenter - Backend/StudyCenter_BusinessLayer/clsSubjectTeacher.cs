using System.Data;
using System.Runtime.CompilerServices;
using StudyCenter_DataAccessLayer;
using StudyCenter_DataAccessLayer.DTOs.SubjectTeacherDTOs;

namespace StudyCenter_BusinessLayer;

public class clsSubjectTeacher
{
    public enum enMode
    {
        AddNew = 0,
        Update = 1
    };

    public enMode Mode;

    public int? SubjectTeacherID { get; set; }
    public int? SubjectGradeLevelID { get; set; }
    public int? TeacherID { get; set; }
    public DateTime AssignmentDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public bool IsActive { get; set; }

    public clsSubjectTeacher(SubjectTeacherDto subjectTeacherDto, enMode mode = enMode.AddNew)
    {
        SubjectTeacherID = subjectTeacherDto.SubjectTeacherId;
        SubjectGradeLevelID = subjectTeacherDto.SubjectGradeLevelId;
        TeacherID = subjectTeacherDto.TeacherId;
        AssignmentDate = subjectTeacherDto.AssignmentDate;
        LastModifiedDate = subjectTeacherDto.LastModifiedDate;
        IsActive = subjectTeacherDto.IsActive;

        Mode = mode;
    }

    public SubjectTeacherDto ToSubjectTeacherDto() => new SubjectTeacherDto(this.SubjectTeacherID,
        this.SubjectGradeLevelID, this.TeacherID, this.AssignmentDate, this.LastModifiedDate, this.IsActive);

    public SubjectTeacherCreationDto ToSubjectTeacherCreationDto() =>
        new SubjectTeacherCreationDto(this.SubjectGradeLevelID, this.TeacherID, this.IsActive);

    public SubjectTeacherUpdateDto ToSubjectTeacherUpdateDto() => new SubjectTeacherUpdateDto(this.SubjectTeacherID,
        this.SubjectGradeLevelID, this.TeacherID, this.IsActive);

    private bool _Validate(out (bool isValid, string? errorMessage) result)
    {
        bool isValid = ValidationHelper.Validate(
            this,

            // ID Check: Ensure SubjectTeacherID is valid if in Update mode
            idCheck: subjectTeacher =>
                (Mode != enMode.Update || ValidationHelper.HasValue(subjectTeacher.SubjectTeacherID)),

            // Value Check: Ensure SubjectGradeLevelID and TeacherID are provided
            valueCheck: subjectTeacher => ValidationHelper.HasValue(subjectTeacher.SubjectGradeLevelID) &&
                                          ValidationHelper.HasValue(subjectTeacher.TeacherID),

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
        SubjectTeacherID =
            clsSubjectTeacherData.Add(new SubjectTeacherCreationDto(this.SubjectGradeLevelID, this.TeacherID,
                this.IsActive));
        return (SubjectTeacherID.HasValue);
    }

    private bool _Update() => clsSubjectTeacherData.Update(this.ToSubjectTeacherUpdateDto());

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

    public static List<SubjectTeacherDto> All()
        => clsSubjectTeacherData.All();
    
    public static clsSubjectTeacher? Find(int? subjectTeacherId)
    {
        SubjectTeacherDto? subjectTeacherDto = clsSubjectTeacherData.GetInfoById(subjectTeacherId);
        return (subjectTeacherDto != null) ? new clsSubjectTeacher(subjectTeacherDto, enMode.Update) : null;
    }

    public static bool Delete(int? subjectTeacherId)
        => clsSubjectTeacherData.Delete(subjectTeacherId);

    public static bool Exists(int? subjectTeacherId)
        => clsSubjectTeacherData.Exists(subjectTeacherId);

    public static bool IsTeachingSubject(int? teacherId, int? subjectGradeLevelId)
        => clsSubjectTeacherData.IsTeachingSubject(teacherId, subjectGradeLevelId);

}