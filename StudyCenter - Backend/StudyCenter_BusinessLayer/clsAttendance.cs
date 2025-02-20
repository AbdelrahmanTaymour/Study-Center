using StudyCenter_DataAccessLayer;
using StudyCenter_DataAccessLayer.DTOs.AttendanceDTOs;

namespace StudyCenter_BusinessLayer;

public class clsAttendance
{
    public enum enMode
    {
        AddNew = 0,
        Update = 1
    };

    public enMode Mode;

    public int? AttendanceID { get; set; }
    public int? StudentGroupID { get; set; }
    public DateTime? AttendanceDate { get; set; }
    public string? Notes { get; set; }

    public clsAttendance(AttendanceDto attendanceDto, enMode mode = enMode.AddNew)
    {
        this.AttendanceID = attendanceDto.AttendanceId;
        this.StudentGroupID = attendanceDto.StudentGroupId;
        this.AttendanceDate = attendanceDto.AttendanceDate;
        this.Notes = attendanceDto.Notes;

        Mode = mode;
    }

    public AttendanceDto ToAttendanceDto() =>
        new AttendanceDto(this.AttendanceID, this.StudentGroupID, this.AttendanceDate, this.Notes);

    public AttendanceCreationDto ToAttendanceCreationDto() =>
        new AttendanceCreationDto(this.StudentGroupID, this.Notes);

    private bool _Validate(out (bool isValid, string? errorMessage) result)
    {
        bool isValid = ValidationHelper.Validate
        (
            this,

            // ID Check: Ensure AttendanceID is valid if in Update mode
            idCheck: a => (a.Mode != enMode.Update) || ValidationHelper.HasValue(a.AttendanceID),

            // Value Check: Ensure StudentGroupID is not empty
            valueCheck: c => ValidationHelper.HasValue(c.StudentGroupID),

            // Date Validation: No date provided for verification.
            dateCheck: null,

            // Retrieve the error message returned by the validation method, if available.
            // This message provides specific details about the validation failure.
            out var errorMessage,

            // Additional Checks: There is no addition checks
            additionalChecks: null
        );

        // Package the overall validation outcome and its corresponding error message into the output tuple.
        result = (isValid, errorMessage);

        return isValid;
    }

    private bool _Add()
    {
        this.AttendanceID = clsAttendanceData.Add(this.ToAttendanceCreationDto());
        return (this.AttendanceID.HasValue);
    }

    private bool _Update() => clsAttendanceData.Update(this.ToAttendanceDto());


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

    public static List<AttendanceDto> All() => clsAttendanceData.All();

    public static clsAttendance? Find(int id)
    {
        AttendanceDto? attendanceDto = clsAttendanceData.GetAttendanceById(id);
        return (attendanceDto != null) ? new clsAttendance(attendanceDto, enMode.Update) : null;
    }

    public static bool Delete(int id) => clsAttendanceData.Delete(id);

    public static bool Exists(int id) => clsAttendanceData.Exists(id);
}