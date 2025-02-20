namespace StudyCenter_DataAccessLayer.DTOs.ExamResultDTOs;

public record ExamResultCreationDto
(
    int? ExamID,
    int? StudentID,
    decimal MarksObtained
);