namespace StudyCenter_DataAccessLayer.DTOs.PersonDTOs;

public record PersonCreationDto
(
    string? FirstName,
    string? SecondName,
    string? ThirdName,
    string? LastName,
    byte Gender,
    DateTime DateOfBirth,
    string PhoneNumber,
    string? Email,
    string? Address
);