using StudyCenter_DataAccessLayer;
using StudyCenter_DataAccessLayer.DTOs.StudentGroupDTOs;

namespace StudyCenter_BusinessLayer;

public class clsStudentGroup
{
    public enum enMode
    {
        AddNew = 0,
        Update = 1
    };

    public enMode Mode;

    public int? StudentGroupID { get; set; }
    public int? StudentID { get; set; }
    public int? GroupID { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
    public int? CreatedByUserID { get; set; }

    public clsStudentGroup(StudentGroupDto studentGroupDto, enMode mode = enMode.AddNew)
    {
        this.StudentGroupID = studentGroupDto.StudentGroupId;
        this.StudentID = studentGroupDto.StudentId;
        this.GroupID = studentGroupDto.GroupId;
        this.StartDate = studentGroupDto.StartDate;
        this.EndDate = studentGroupDto.EndDate;
        this.IsActive = studentGroupDto.IsActive;
        this.CreatedByUserID = studentGroupDto.CreatedByUserId;

        Mode = mode;
    }

    public StudentGroupDto ToStudentGroupDto()
        => new StudentGroupDto(this.StudentGroupID, this.StudentID, this.GroupID, this.StartDate, this.EndDate,
            this.IsActive, this.CreatedByUserID);

    public StudentGroupCreationDto ToStudentGroupCreationDto()
        => new StudentGroupCreationDto(this.StudentID, this.GroupID, this.StartDate, this.EndDate,
            this.IsActive, this.CreatedByUserID);

    private bool _Validate(out (bool isValid, string? errorMessage) result)
    {
        bool isValid = ValidationHelper.Validate(
            this,

            // ID Check: Ensure StudentGroupID is valid if in Update mode
            idCheck: studentGroup => (Mode != enMode.Update || ValidationHelper.HasValue(studentGroup.StudentGroupID)),

            // Value Check: Ensure required properties are not null
            valueCheck: studentGroup => ValidationHelper.HasValue(studentGroup.StudentID) &&
                                        ValidationHelper.HasValue(studentGroup.GroupID) &&
                                        ValidationHelper.HasValue(studentGroup.CreatedByUserID),

            // Date Check: Ensure dates are valid
            dateCheck: studentGroup => (Mode == enMode.AddNew && !ValidationHelper.IsDateValid(studentGroup.StartDate, DateTime.Now)) ||
                                       (Mode == enMode.Update && (!studentGroup.EndDate.HasValue || !ValidationHelper.IsDateValid(studentGroup.StartDate, studentGroup.EndDate.Value))),

            // Retrieve the error message returned by the validation method, if available.
            // This message provides specific details about the validation failure.
            out string? errorMessage,

            // Additional Checks: There is no addition checks
            additionalChecks: null
        );

        // Package the overall validation outcome and its corresponding error message into the output tuple.
        result = (isValid, errorMessage);

        return isValid;
    }

    private bool _Add()
    {
        StudentGroupID = clsStudentGroupData.Add(this.ToStudentGroupCreationDto());
        return (StudentGroupID.HasValue);
    }

    private bool _Update() => clsStudentGroupData.Update(this.ToStudentGroupDto());

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
    
    public static List<StudentGroupDto> All()
        => clsStudentGroupData.All();

    public static clsStudentGroup? Find(int? studentGroupID)
    {
        StudentGroupDto? studentGroupDto = clsStudentGroupData.GetInfoByID(studentGroupID);
        return (studentGroupDto != null) ? new clsStudentGroup(studentGroupDto, enMode.Update) : null;
    }
    
    public static bool Exists(int? studentGroupID)
        => clsStudentGroupData.Exists(studentGroupID);

    public static bool IsStudentAssignedToGroup(int? studentID, int? groupID)
        => clsStudentGroupData.IsStudentAssignedToGroup(studentID, groupID);

    public static bool Delete(int? studentGroupID)
        => clsStudentGroupData.Delete(studentGroupID);

    public static bool Delete(int? studentID, int? groupID)
        => clsStudentGroupData.Delete(studentID, groupID);

}