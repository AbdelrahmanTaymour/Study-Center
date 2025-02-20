namespace StudyCenter_DataAccessLayer.DTOs.SubjectTeacherDTOs;

public record SubjectTeacherDto
(
    int? SubjectTeacherId,
    int? SubjectGradeLevelId,
    int? TeacherId,
    DateTime AssignmentDate,
    DateTime? LastModifiedDate,
    bool IsActive
);