namespace StudyCenter_DataAccessLayer.DTOs.StudentAnswerDTOs;

public record StudentAnswerCreationDto
(
    int? ExamResultID,
    int? QuestionID,
    int? AnswerID,
    string? AnswerText,
    decimal? MarksAwarded
);