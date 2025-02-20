namespace StudyCenter_DataAccessLayer.DTOs.QuestionDTOs;

public record QuestionDto
(
    int? QuestionID,
    int? ExamID,
    string QuestionText,
    byte QuestionType,
    decimal Marks
);