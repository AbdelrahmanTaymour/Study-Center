namespace StudyCenter_DataAccessLayer.DTOs.StudentAnswerDTOs;

public record StudentAnswerDto
(
    int? StudentAnswerID,
    int? ExamResultID,
    int? QuestionID,
    int? AnswerID,
    string? AnswerText,
    decimal? MarksAwarded
);