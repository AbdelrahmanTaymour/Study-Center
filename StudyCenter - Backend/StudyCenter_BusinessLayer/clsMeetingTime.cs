using StudyCenter_DataAccessLayer;
using StudyCenter_DataAccessLayer.DTOs.MeetingTimeDTOs;

namespace StudyCenter_BusinessLayer;

public class clsMeetingTime
{
    public enum enMode
    {
        AddNew = 0,
        Update = 1
    };

    public enMode Mode = enMode.AddNew;

    public int? MeetingTimeID { get; set; }

    private TimeSpan _oldStartTime = TimeSpan.Zero;
    private TimeSpan _startTime = TimeSpan.Zero;

    public TimeSpan StartTime
    {
        get => _startTime;

        set
        {
            if (_oldStartTime != TimeSpan.Zero)
            {
                _oldStartTime = _startTime;
            }

            _startTime = value;
        }
    }

    public TimeSpan EndTime { get; set; }

    private byte? _oldMeetingDays = null;
    private byte? _meetingDays = null;

    public byte MeetingDays
    {
        get => _meetingDays ?? 0;

        set
        {
            if (!_oldMeetingDays.HasValue)
            {
                _oldMeetingDays = _meetingDays;
            }

            _meetingDays = value;
        }
    }

    public clsMeetingTime(MeetingTimeDto meetingTimeDto, enMode mode = enMode.AddNew)
    {
        this.MeetingTimeID = meetingTimeDto.MeetingTimeId;
        this.StartTime = meetingTimeDto.StartTime;
        this.EndTime = meetingTimeDto.EndTime;
        this.MeetingDays = meetingTimeDto.MeetingDays;

        this.Mode = mode;
    }

    public MeetingTimeDto ToMeetingTimeDto() =>
        new MeetingTimeDto(this.MeetingTimeID, this.StartTime, this.EndTime, this.MeetingDays);


    private bool _Validate(out (bool isValid, string? errorMessage) result)
    {
        bool isValid = ValidationHelper.Validate(
            this,
            idCheck: mt => (mt.Mode != clsMeetingTime.enMode.Update) || ValidationHelper.HasValue(mt.MeetingTimeID),

            // Value Check: Ensure StartTime is before EndTime and MeetingDays is within valid range
            valueCheck: mt => mt.StartTime < mt.EndTime && mt.MeetingDays is >= 0 and <= 2,

            // Date Validation: No date provided for verification.
            dateCheck: null,

            // Retrieve the error message returned by the validation method, if available.
            // This message provides specific details about the validation failure.
            out string? errorMessage,

            // Additional Checks: Perform miscellaneous validations and return corresponding error messages.
            additionalChecks: new (Func<clsMeetingTime, bool>, string)[]
            {
                (mt =>
                    (mt.Mode != enMode.AddNew && _oldStartTime == mt.StartTime && _oldMeetingDays == mt.MeetingDays) ||
                    !Exists(mt.StartTime, mt.MeetingDays), "Meeting time already exists.")
            }
        );

        // Package the overall validation outcome and its corresponding error message into the output tuple.
        result = (isValid, errorMessage);

        return isValid;
    }

    private bool _Add()
    {
        this.MeetingTimeID =
            clsMeetingTimeData.Add(new MeetingTimeCreationDto(this.StartTime, this.EndTime, this.MeetingDays));
        return (MeetingTimeID.HasValue);
    }

    private bool _Update() => clsMeetingTimeData.Update(this.ToMeetingTimeDto());

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

    public static List<MeetingTimeDto> All() => clsMeetingTimeData.All();

    public static clsMeetingTime? Find(int? meetingTimeId)
    {
        MeetingTimeDto? meetingTimeDto = clsMeetingTimeData.GetInfoById(meetingTimeId);
        return (meetingTimeDto != null ? new clsMeetingTime(meetingTimeDto, enMode.Update) : null);
    }

    public static bool Exists(int? meetingTimeId)
        => clsMeetingTimeData.Exists(meetingTimeId);
    public static bool Exists(TimeSpan startTime, byte meetingDays)
        => clsMeetingTimeData.Exists(startTime, meetingDays);
    
    public static bool Delete(int? meetingTimeId)
        => clsMeetingTimeData.Delete(meetingTimeId);

}