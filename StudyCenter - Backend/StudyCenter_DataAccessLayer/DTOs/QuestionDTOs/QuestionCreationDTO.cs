namespace StudyCenter_DataAccessLayer.DTOs.QuestionDTOs;

public record QuestionCreationDto
(
    int? ExamID,
    string QuestionText,
    byte QuestionType,
    decimal Marks
);