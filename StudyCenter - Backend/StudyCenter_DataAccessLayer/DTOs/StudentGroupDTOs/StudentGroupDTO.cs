namespace StudyCenter_DataAccessLayer.DTOs.StudentGroupDTOs;

public record StudentGroupDto
(
    int? StudentGroupId,
    int? StudentId,
    int? GroupId,
    DateTime StartDate,
    DateTime? EndDate,
    bool IsActive,
    int? CreatedByUserId
);