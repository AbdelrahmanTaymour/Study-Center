using StudyCenter_DataAccessLayer;
using StudyCenter_DataAccessLayer.DTOs;

namespace StudyCenter_BusinessLayer;

public class clsUser
{
    public enum enPermissions
    {
        All = -1,
        AddUser = 1,
        UpdateUser = 2,
        DeleteUser = 4,
        ListUsers = 8
    }
    
    public enum enMode { AddNew = 0, Update = 1 };
    public enMode Mode = enMode.AddNew;
    
    public int? UserID { get; private set; }
    private int? _oldPersonID = null;
    private int? _personID = null;
    public int? PersonID
    {
        get => _personID;

        set
        {
            if (!_oldPersonID.HasValue)
            {
                _oldPersonID = _personID;
            }

            _personID = value;
        }
    }

    private string? _oldUsername = string.Empty;
    private string? _userName = string.Empty;
    public string? Username
    {
        get => _userName;

        set
        {
            // If the old username is not set (indicating either a new user or the username is being set for the first time),
            // initialize it with the current username value to track changes.
            if (string.IsNullOrWhiteSpace(_oldUsername))
            {
                _oldUsername = _userName;
            }

            _userName = value;
        }
    }

    public string? Password { get; set; }
    public int Permissions { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastLoginDate { get; set; } = null;
    
    private clsPerson? _personInfo;
    public clsPerson? PersonInfo
    {
        // Retrieves the person's information based on the assigned PersonID.
        // If the _personInfo object is null and PersonID is valid (> 0),
        // it fetches the corresponding person details from the database.
        // The retrieved data is then cached in _personInfo for future access.
        get
        {
            if (_personInfo is null && PersonID > 0)
                _personInfo = clsPerson.Find(PersonID);
            
            return _personInfo;
        }
    }

    public clsUser(UserDto userDto, enMode mode = enMode.AddNew)
    {
        this.UserID = userDto.UserID;
        this.PersonID = userDto.PersonID;
        this.Username = userDto.Username;
        this.Password = userDto.Password;
        this.Permissions = userDto.Permissions;
        this.IsActive = userDto.IsActive;
        
        this.Mode = mode;
    }

    public UserDto ToUserDto() => new UserDto(this.UserID, this.PersonID, this.Username, this.Password,
        this.Permissions, this.IsActive);
    
    
    private bool _Validate(out (bool isValid, string? errorMessage) result)
    {
        bool isValid = ValidationHelper.Validate(
            this,
            
            // ID Check: Ensure UserID is valid if in Update mode
            idCheck: user => (Mode != enMode.Update || ValidationHelper.HasValue(user.UserID)),
            
            // Value Check: Ensure required properties are not null or empty
            valueCheck: user => ValidationHelper.HasValue(user.PersonID) && user.PersonID > 0,
            
            // Date Validation: No date provided for verification.
            dateCheck: null,
            
            // Retrieve the error message returned by the validation method, if available.
            // This message provides specific details about the validation failure.
            out var errorMessage,
            
            // Additional Checks: Perform miscellaneous validations and return corresponding error messages.
            additionalChecks: new (Func<clsUser, bool>, string)[]
            {
                // If the old username is different from the new username:
                // - In AddNew Mode: This indicates the new username, requiring validation.
                // - In Update Mode: This indicates that the username has been changed, so we need to check if it already exists in the database.
                // If the new username already exists in the database, return false to indicate validation failure.

                // Check if PersonID already exists, considering mode and previous value
                (user => (Mode != enMode.AddNew && _oldPersonID == user.PersonID) ||
                         !ExistsByPersonID(user.PersonID), "Person already exists."),

                // Check if Username is not empty
                (user => ValidationHelper.IsNotEmpty(user.Username), "Username is empty."),

                // Username Check: Ensure the username is unique, considering the current mode and previous value.
                (user => _oldUsername != null && ((Mode != enMode.AddNew && _oldUsername.Equals(user.Username)) ||
                                                  !ExistsByUsername(user.Username)), "Username already exists."),

                // Check if Password is not empty
                (user => ValidationHelper.IsNotEmpty(user.Password), "Password is empty.")
            }
        );

        // Package the overall validation outcome and its corresponding error message into the output tuple.
        result = (isValid, errorMessage);
        
        return isValid;
    }
    
    private bool _Add()
    {
        this.UserID = clsUserData.Add(new UserCreationDto(this.PersonID, this.Username, this.Password, this.Permissions, this.IsActive));
        return this.UserID.HasValue;
    }
    private bool _Update()
    {
        return clsUserData.Update(this.ToUserDto());
    }
    
    public bool Save(out string? validationMessage)
    {
        // Validate the current user data. If validation fails, return false with the error message.
        if (!_Validate(out (bool success, string? message) result))
        {
            validationMessage = result.message;
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
    
    public static List<UserViewDto> GetAllUsers() => clsUserData.GetAllUsers();
    
    public static clsUser? FindByID(int? userID)
    {
        UserDto? userDto = clsUserData.GetUserInfoByID(userID);
        return (userDto != null) ? new clsUser(userDto, enMode.Update) : null;
    }

    public static clsUser? FindByPersonID(int? personID)
    {
        UserDto? userDto = clsUserData.GetUserInfoByPersonID(personID);
        return (userDto != null) ? new clsUser(userDto, enMode.Update) : null;
    }

    public static clsUser? FindByUsername(string username)
    {
        UserDto? userDto = clsUserData.GetUserInfoByUsername(username);
        return (userDto != null) ? new clsUser(userDto, enMode.Update) : null;
    }

    public static clsUser? Login(string username, string password)
    {
        UserDto? userDto = clsUserData.Login(username, password);
        return (userDto != null) ? new clsUser(userDto, enMode.Update) : null;
    }
    
    public static bool Delete(int? userID) => clsUserData.Delete(userID);
    
    public static bool ExistByUserID(int? userID) 
        => clsUserData.ExistsByUserID(userID);
    public static bool ExistsByPersonID(int? personID)
        => clsUserData.ExistsByPersonID(personID);
    public static bool ExistsByUsername(string? username) 
        => clsUserData.ExistsByUsername(username);
    
    public static bool ExistsByUsernameAndPassword(string username, string password) 
        => clsUserData.ExistsByUsernameAndPassword(username, password);
    
    public static bool ChangePassword(int? UserID, string newPassword) 
        => clsUserData.ChangePassword(UserID, newPassword);
}