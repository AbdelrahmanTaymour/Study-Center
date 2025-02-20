namespace StudyCenter_DataAccessLayer.DTOs.GroupDTOs;

public record GroupDto
(
    int? GroupId,
    string GroupName,
    int? ClassId,
    int? TeacherId,
    int? SubjectTeacherId,
    int? MeetingTimeId,
    string? Description,
    int? CreatedByUserId,
    DateTime CreationDate,
    DateTime? LastModifiedDate,
    bool IsActive,
    byte? StudentCount
);