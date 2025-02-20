using StudyCenter_DataAccessLayer;
using StudyCenter_DataAccessLayer.DTOs.GroupDTOs;

namespace StudyCenter_BusinessLayer;

public class clsGroup
{
    public enum enMode
    {
        AddNew = 0,
        Update = 1
    };

    public enMode Mode;

    public int? GroupID { get; set; }
    public string GroupName { get; set; }
    public int? ClassID { get; set; }
    public int? TeacherID { get; set; }
    public int? SubjectTeacherID { get; set; }
    public int? MeetingTimeID { get; set; }
    public string? Description { get; set; }
    public int? CreatedByUserID { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public byte? StudentCount { get; set; }

    public clsGroup(GroupDto group, enMode mode = enMode.AddNew)
    {
        GroupID = group.GroupId;
        GroupName = group.GroupName;
        ClassID = group.ClassId;
        TeacherID = group.TeacherId;
        SubjectTeacherID = group.SubjectTeacherId;
        MeetingTimeID = group.MeetingTimeId;
        Description = group.Description;
        CreatedByUserID = group.CreatedByUserId;
        CreationDate = group.CreationDate;
        LastModifiedDate = group.LastModifiedDate;
        IsActive = group.IsActive;
        StudentCount = group.StudentCount;

        Mode = mode;
    }

    public GroupCreationDto ToGroupCreationDto()
        => new GroupCreationDto(this.GroupName, this.ClassID, this.TeacherID, this.SubjectTeacherID, this.MeetingTimeID,
            this.Description, this.CreatedByUserID, this.IsActive);

    public GroupDto ToGroupDto()
        => new GroupDto(this.GroupID, this.GroupName, this.ClassID, this.TeacherID, this.SubjectTeacherID,
            this.MeetingTimeID, this.Description, this.CreatedByUserID, this.CreationDate, this.LastModifiedDate,
            this.IsActive, this.StudentCount);

    public GroupUpdateDto ToGroupUpdateDto()
        => new GroupUpdateDto(this.GroupID, this.GroupName, this.ClassID, this.TeacherID, this.SubjectTeacherID,
            this.MeetingTimeID, this.Description, this.CreatedByUserID, this.IsActive);

   
    private bool _Validate(out (bool isValid, string? errorMessage) result)
    {
        bool isValid = ValidationHelper.Validate(
            this,
            idCheck: g => (g.Mode != enMode.Update) || ValidationHelper.HasValue(g.GroupID),

            // Value Check: Ensure all required properties have values and that GroupName is not empty if in Update mode
            valueCheck: g => ValidationHelper.HasValue(g.ClassID) &&
                             ValidationHelper.HasValue(g.TeacherID) &&
                             ValidationHelper.HasValue(g.SubjectTeacherID) &&
                             ValidationHelper.HasValue(g.MeetingTimeID) &&
                             ValidationHelper.HasValue(g.CreatedByUserID) &&
                             (g.Mode != enMode.Update || !string.IsNullOrWhiteSpace(g.GroupName)),

            // Date Check: Ensure CreationDate is valid and LastModifiedDate is valid if it exists
            dateCheck: g => (g.Mode != enMode.AddNew || ValidationHelper.IsDateValid(g.CreationDate, DateTime.Now)) &&
                            (g.Mode != enMode.Update || !g.LastModifiedDate.HasValue ||
                             ValidationHelper.IsDateValid(g.LastModifiedDate.Value, DateTime.Now)),

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
        GroupID = clsGroupData.Add(this.ToGroupCreationDto());
        return (GroupID.HasValue);
    }
    private bool _Update() => clsGroupData.Update(this.ToGroupUpdateDto());

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

    public static clsGroup? Find(int? groupID)
    {
        GroupDto? groupDto = clsGroupData.GetInfoById(groupID);
        return (groupDto != null) ? new clsGroup(groupDto, enMode.Update) : null;
    }

    public static bool Delete(int? groupID)
        => clsGroupData.Delete(groupID);

    public static bool Exists(int? groupID)
        => clsGroupData.Exists(groupID);

    public static List<GroupDto> All()
        => clsGroupData.All();
}