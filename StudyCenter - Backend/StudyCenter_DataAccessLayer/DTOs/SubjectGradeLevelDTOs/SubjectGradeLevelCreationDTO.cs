namespace StudyCenter_DataAccessLayer.DTOs.SubjectGradeLevelDTOs;

public record SubjectGradeLevelCreationDto
(
    int? SubjectId,
    int? GradeLevelId,
    decimal Fees,
    bool IsMandatory,
    string? Description
);