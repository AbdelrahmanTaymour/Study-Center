namespace StudyCenter_DataAccessLayer.DTOs;

public record UserCreationDto
(
    int? PersonID,
    string? Username,
    string? Password,
    int Permissions,
    bool IsActive
);