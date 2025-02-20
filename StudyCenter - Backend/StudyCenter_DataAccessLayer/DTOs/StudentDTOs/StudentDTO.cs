namespace StudyCenter_DataAccessLayer.DTOs.StudentDTOs;

public record StudentDto
(
    int? StudentId,
    int? PersonId,
    int? GradeLevelId,
    bool Status,
    string? Notes,
    int? CreatedByUserId,
    DateTime? CreationDate
);
