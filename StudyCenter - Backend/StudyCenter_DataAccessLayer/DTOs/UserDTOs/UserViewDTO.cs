namespace StudyCenter_DataAccessLayer.DTOs;

public record UserViewDto
(
    int? UserID,
    string FullName, 
    string Username,
    string Gender,
    string PhoneNumber,
    bool IsActive
);