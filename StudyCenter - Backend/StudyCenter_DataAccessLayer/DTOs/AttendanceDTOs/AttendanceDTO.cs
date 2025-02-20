namespace StudyCenter_DataAccessLayer.DTOs.AttendanceDTOs;

public record AttendanceDto
(
    int? AttendanceId,
    int? StudentGroupId,
    DateTime? AttendanceDate,
    string? Notes
);