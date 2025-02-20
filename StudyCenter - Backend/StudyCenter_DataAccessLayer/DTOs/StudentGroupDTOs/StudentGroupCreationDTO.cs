namespace StudyCenter_DataAccessLayer.DTOs.StudentGroupDTOs;

public record StudentGroupCreationDto
(
    int? StudentId,
    int? GroupId,
    DateTime StartDate,
    DateTime? EndDate,
    bool IsActive,
    int? CreatedByUserId
);