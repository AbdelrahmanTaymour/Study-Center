namespace StudyCenter_DataAccessLayer.DTOs.ExamResultDTOs;

public record ExamResultDto
(
    int? ExamResultID,
    int? ExamID,
    int? StudentID,
    decimal MarksObtained
);