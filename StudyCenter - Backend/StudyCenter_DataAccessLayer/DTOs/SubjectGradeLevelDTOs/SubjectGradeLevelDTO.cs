namespace StudyCenter_DataAccessLayer.DTOs.SubjectGradeLevelDTOs;

public record SubjectGradeLevelDto(
    int? SubjectGradeLevelId,
    int? SubjectId,
    int? GradeLevelId,
    decimal Fees,
    bool IsMandatory,
    string? Description
);