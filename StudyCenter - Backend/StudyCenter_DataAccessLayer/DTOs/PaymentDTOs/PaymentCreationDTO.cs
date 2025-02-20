namespace StudyCenter_DataAccessLayer.DTOs.PaymentDTOs;

public record PaymentCreationDto(
    int? StudentGroupID,
    int? SubjectGradeLevelID,
    decimal PaymentAmount,
    string? PaymentMethod,
    byte PaymentStatus,
    int? CreatedByUserID
);