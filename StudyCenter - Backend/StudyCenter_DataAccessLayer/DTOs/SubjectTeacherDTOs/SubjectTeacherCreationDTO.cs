namespace StudyCenter_DataAccessLayer.DTOs.SubjectTeacherDTOs;

public record SubjectTeacherCreationDto
(
    int? SubjectGradeLevelId,
    int? TeacherId,
    bool IsActive
);