namespace StudyCenter_DataAccessLayer.DTOs.MeetingTimeDTOs;

public record MeetingTimeDto(int? MeetingTimeId, TimeSpan StartTime, TimeSpan EndTime, byte MeetingDays);