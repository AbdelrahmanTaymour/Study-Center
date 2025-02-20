namespace StudyCenter_DataAccessLayer.DTOs.AnswerDTOs;

public record AnswerCreationDto
(
    int? QuestionID,
    string AnswerText,
    bool IsCorrect
);