namespace StudyCenter_DataAccessLayer.DTOs.AnswerDTOs;

public record AnswerDto(
    int? AnswerID,
    int? QuestionID,
    string AnswerText,
    bool IsCorrect
);