namespace StudyCenter_DataAccessLayer.DTOs.PersonDTOs;

public record PersonDto
(
    int? PersonID,
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