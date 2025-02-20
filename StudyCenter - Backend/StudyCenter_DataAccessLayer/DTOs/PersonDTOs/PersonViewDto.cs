namespace StudyCenter_DataAccessLayer.DTOs.PersonDTOs;

public record PersonViewDto
(
    int? PersonID,
    string FullName,
    string Gender,
    DateTime DateOfBirth,
    string PhoneNumber,
    string? Email,
    string? Address
);