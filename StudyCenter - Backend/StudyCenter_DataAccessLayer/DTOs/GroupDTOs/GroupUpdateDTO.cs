namespace StudyCenter_DataAccessLayer.DTOs.GroupDTOs;

public record GroupUpdateDto
(
    int? GroupId,
    string GroupName,
    int? ClassId,
    int? TeacherId,
    int? SubjectTeacherId,
    int? MeetingTimeId,
    string? Description,
    int? CreatedByUserId,
    bool IsActive
);