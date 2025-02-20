namespace StudyCenter_DataAccessLayer.DTOs;

public record UserDto
(
    int? UserID, 
    int? PersonID, 
    string? Username, 
    string? Password, 
    int Permissions, 
    bool IsActive
);