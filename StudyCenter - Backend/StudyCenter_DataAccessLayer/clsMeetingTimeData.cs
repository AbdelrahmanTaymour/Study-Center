using StudyCenter_DataAccessLayer.DTOs.MeetingTimeDTOs;
using StudyCenter_DataAccessLayer.Global_Classes;
using StudyCenter_DataAccessLayer.Mappings;

namespace StudyCenter_DataAccessLayer;

public class clsMeetingTimeData
{
    public static MeetingTimeDto? GetInfoById(int? meetingTimeId) => clsDataAccessHelper.GetBy(
        "SP_MeetingTimes_GetMeetingTimeInfoByID", "MeetingTimeID", meetingTimeId,
        MeetingTimeMapping.MapToMeetingTimeDto);

    public static int? Add(MeetingTimeCreationDto newMeetingTime) =>
        clsDataAccessHelper.Add("SP_MeetingTimes_AddNewMeetingTime", "NewMeetingTimeID", newMeetingTime);

    public static bool Update(MeetingTimeDto updatedMeetingTimeDto) =>
        clsDataAccessHelper.Update("SP_MeetingTimes_UpdateMeetingTimeInfo", updatedMeetingTimeDto);

    public static bool Delete(int? meetingTimeId)
        => clsDataAccessHelper.Delete("SP_MeetingTimes_DeleteMeetingTime", "MeetingTimeID", meetingTimeId);

    public static bool Exists(int? meetingTimeId)
        => clsDataAccessHelper.Exists("SP_MeetingTimes_DoesMeetingTimeExists", "MeetingTimeID", meetingTimeId);

    public static bool Exists(TimeSpan startTime, byte meetingDays)
        => clsDataAccessHelper.Exists("SP_MeetingTimes_DoesMeetingTimeExistByStartTimeAndMeetingDays", "StartTime", startTime,
            "MeetingDays", meetingDays);

    public static List<MeetingTimeDto> All()
        => clsDataAccessHelper.All("SP_MeetingTimes_GetAllMeetingTimes", MeetingTimeMapping.MapToMeetingTimeDto);

}