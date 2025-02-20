namespace StudyCenter_DataAccessLayer.DTOs.SubjectTeacherDTOs;

public record SubjectTeacherUpdateDto
(
    int? SubjectTeacherId,
    int? SubjectGradeLevelId,
    int? TeacherId,
    bool IsActive
);