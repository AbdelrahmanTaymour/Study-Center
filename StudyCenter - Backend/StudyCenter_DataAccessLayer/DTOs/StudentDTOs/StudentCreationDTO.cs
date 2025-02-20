namespace StudyCenter_DataAccessLayer.DTOs.StudentDTOs;

public record StudentCreationDto
(
    int? PersonId,
    int? GradeLevelId,
    bool Status,
    string? Notes,
    int? CreatedByUserId
);
