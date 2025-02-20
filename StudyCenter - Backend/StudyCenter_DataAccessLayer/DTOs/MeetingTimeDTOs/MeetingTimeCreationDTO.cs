namespace StudyCenter_DataAccessLayer.DTOs.MeetingTimeDTOs;

public record MeetingTimeCreationDto(TimeSpan StartTime, TimeSpan EndTime, byte MeetingDays);