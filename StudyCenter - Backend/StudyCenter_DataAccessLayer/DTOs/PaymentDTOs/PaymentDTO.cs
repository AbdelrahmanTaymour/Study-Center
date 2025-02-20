namespace StudyCenter_DataAccessLayer.DTOs.PaymentDTOs;

public record PaymentDto
(
    int? PaymentID,
    int? StudentGroupID,
    int? SubjectGradeLevelID,
    decimal PaymentAmount,
    string? PaymentMethod,
    byte PaymentStatus,
    DateTime PaymentDate,
    int? CreatedByUserID
);