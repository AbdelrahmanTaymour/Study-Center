namespace StudyCenter_DataAccessLayer.DTOs.TeacherDTOs;

public record TeacherCreationDto
(
    int? PersonId,
    int? EducationLevelId,
    byte? TeachingExperience,
    string? Certifications,
    bool Status,
    string? Notes,
    int? CreatedByUserId
);