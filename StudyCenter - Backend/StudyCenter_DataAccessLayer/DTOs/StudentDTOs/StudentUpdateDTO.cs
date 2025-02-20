namespace StudyCenter_DataAccessLayer.DTOs.StudentDTOs;

public record StudentUpdateDto
(
    int? StudentId,
    int? PersonId,
    int? GradeLevelId,
    bool Status,
    string? Notes
);
