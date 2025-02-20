namespace StudyCenter_DataAccessLayer.DTOs.TeacherDTOs;

public record TeacherUpdateDto
(
    int? TeacherId,
    int? PersonId,
    int? EducationLevelId,
    byte? TeachingExperience,
    string? Certifications,
    bool Status,
    string? Notes
);
