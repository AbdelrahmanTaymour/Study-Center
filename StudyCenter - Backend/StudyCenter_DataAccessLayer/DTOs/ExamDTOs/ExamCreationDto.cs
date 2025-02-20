namespace StudyCenter_DataAccessLayer.DTOs.ExamDTOs;

public record ExamCreationDto(
    int? SubjectGradeLevelID,
    string ExamName,
    DateTime ExamDate,
    decimal TotalMarks,
    decimal PassingMarks
);