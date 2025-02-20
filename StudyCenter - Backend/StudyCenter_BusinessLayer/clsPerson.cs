using StudyCenter_DataAccessLayer;
using StudyCenter_DataAccessLayer.DTOs.PersonDTOs;

namespace StudyCenter_BusinessLayer;

public class clsPerson
{
    public enum enMode { AddNew = 0, Update = 1 };
    public enMode Mode = enMode.AddNew;
    public enum enGender { Male = 0, Female = 1 };

    public int? PersonID { get; set; }
    public string? FirstName { get; set; }
    public string? SecondName { get; set; }
    public string? ThirdName { get; set; }
    public string? LastName { get; set; }
    public enGender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }

    public PersonDto ToPersonDto() => new PersonDto(this.PersonID, this.FirstName, this.SecondName, this.ThirdName, this.LastName, (byte)this.Gender, this.DateOfBirth, this.PhoneNumber, this.Email, this.Address);
    public string FullName => string.Concat(FirstName, " ", SecondName, " ", ThirdName ?? "", " ", LastName);
    public string GenderName => Gender.ToString();
    
    public clsPerson(PersonDto personDto, enMode mode = enMode.AddNew)
    {
        PersonID = personDto.PersonID;
        FirstName = personDto.FirstName;
        SecondName = personDto.SecondName;
        ThirdName = personDto.ThirdName;
        LastName = personDto.LastName;
        Gender = (enGender)personDto.Gender;
        DateOfBirth = personDto.DateOfBirth;
        PhoneNumber = personDto.PhoneNumber;
        Email = personDto.Email;
        Address = personDto.Address;

        Mode = mode;
    }

    
    private bool _Validate(out (bool isValid, string? errorMessage) result)
    {
        bool isValid = ValidationHelper.Validate
        (
            this,
            
            // ID Check: Ensure PersonID is valid if in Update mode
            idCheck: person => (Mode != enMode.Update) || ValidationHelper.HasValue(person.PersonID),
            
            // Value Check: Ensure required properties are not null or empty
            valueCheck: person => ValidationHelper.IsNotEmpty(person.FirstName) &&
                                  ValidationHelper.IsNotEmpty(person.SecondName)&&
                                  ValidationHelper.IsNotEmpty(person.LastName),
            
            // Date Check: Ensure DateOfBirth is not in the future
            dateCheck: person => ValidationHelper.IsDateValid(person.DateOfBirth, DateTime.Now),
                                  
            // Retrieve the error message returned by the validation method, if available.
            out string? errorMessage,
            
            // Additional Checks: Perform miscellaneous validations and return corresponding error messages.
            additionalChecks: new (Func<clsPerson, bool>, string )[]
            {
                // Gender Validation: Ensure the value is either 'Male' or 'Female'.
                (person => person.Gender is enGender.Male or enGender.Female, "Gender is wrong"),
                
                // Phone Number Validation: Ensure the provided phone number is in a valid format.
                (person => ValidationHelper.IsValidPhoneNumber(person.PhoneNumber), "Phone number is not valid"),
                
                // Email Validation: Ensure the provided email address is in a valid format.
                (person => person.Email != null && ValidationHelper.IsValidEmail(person.Email), "Email is not valid"),
            }
        );
        
        // Package the validation result and message into the output tuple.
        result = (isValid, errorMessage);
        return isValid;
    }

    private bool _Add()
    {
        this.PersonID = clsPersonData.Add(new PersonCreationDto(this.FirstName, this.SecondName, this.ThirdName,
            this.LastName, (byte)this.Gender, this.DateOfBirth, this.PhoneNumber, this.Email, this.Address));
        return this.PersonID.HasValue;
    }

    private bool _Update() => clsPersonData.Update(new PersonDto(this.PersonID, this.FirstName, this.SecondName, this.ThirdName,
        this.LastName, (byte)this.Gender, this.DateOfBirth, this.PhoneNumber, this.Email, this.Address));

    
    public bool Save(out string? validationMessage)
    {
        // Validate the current user data. If validation fails, return false with the error message.
        if (!_Validate(out (bool success, string? message) result))
        {
            validationMessage =  result.message;
            return false;
        }
        
        // Clear the validation message if validation succeeds.
        validationMessage = string.Empty;

        switch (Mode)
        {
            case enMode.AddNew:
                if (_Add())
                {
                    Mode = enMode.Update;
                    return true;
                }
                return false;
            
            case enMode.Update:
                return _Update();
        }
        return false;
    }

    public static List<PersonViewDto> GetAllPeople()
        => clsPersonData.GetAllPeople();
    public static clsPerson? Find(int? personId)
    {
        PersonDto? personDto = clsPersonData.GetInfoById(personId);

        return (personDto != null) ? (new clsPerson(personDto, enMode.Update)) : null;
    }

    public static bool Delete(int? personID) => clsPersonData.Delete(personID);

    public static bool ExistsByID(int? personID) => clsPersonData.Exists(personID);

}