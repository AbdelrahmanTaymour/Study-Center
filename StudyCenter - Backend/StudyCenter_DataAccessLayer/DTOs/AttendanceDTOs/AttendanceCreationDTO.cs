namespace StudyCenter_DataAccessLayer.DTOs.AttendanceDTOs;

public record AttendanceCreationDto
(
    int? StudentGroupId,
    string? Notes
);