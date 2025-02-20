namespace StudyCenter_DataAccessLayer.DTOs.ExamDTOs;

public record ExamDto
(
    int? ExamID,
    int? SubjectGradeLevelID,
    string ExamName,
    DateTime ExamDate,
    decimal TotalMarks,
    decimal PassingMarks
);