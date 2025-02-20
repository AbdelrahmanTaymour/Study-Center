using StudyCenter_DataAccessLayer;
using StudyCenter_DataAccessLayer.DTOs.EducationLevelDTOs;

namespace StudyCenter_BusinessLayer;

public class clsEducationLevel
{
       public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? EducationLevelID { get; set; }

        private string _oldLevelName = string.Empty;
        private string _levelName = string.Empty;
        public string LevelName
        {
            get => _levelName;

            set
            {
                // If the old LevelName is not set (indicating either a new user or the LevelName is being set for the first time),
                // initialize it with the current LevelName value to track changes.
                if (string.IsNullOrWhiteSpace(_oldLevelName))
                {
                    _oldLevelName = _levelName;
                }

                _levelName = value;
            }
        }
        public clsEducationLevel(EducationLevelDto educationLevelDto, enMode mode = enMode.AddNew)
        {
            this.EducationLevelID = educationLevelDto.EducationLevelId;
            this.LevelName = educationLevelDto.LevelName;

            Mode = mode;
        }
        public EducationLevelDto ToEducationLevelDto() => new EducationLevelDto(this.EducationLevelID, this.LevelName);
       
        private bool _Validate(out (bool isValid, string? errorMessage) result)
        {
            bool isValid =  ValidationHelper.Validate
            (
                this,

                // ID Check: Ensure EducationLevelID is valid if in Update mode
                idCheck: el => (el.Mode != enMode.Update) || ValidationHelper.HasValue(el.EducationLevelID),

                // Value Check: Ensure LevelName is not empty
                valueCheck: el => !string.IsNullOrWhiteSpace(el.LevelName),

                // Date Validation: No date provided for verification.
                dateCheck: null,

                // Retrieve the error message returned by the validation method, if available.
                // This message provides specific details about the validation failure.
                out var errorMessage,

                // Additional Checks: Ensure LevelName does not already exist in the database
                additionalChecks: new (Func<clsEducationLevel, bool>, string)[]
                {
                    (el => (el.Mode != enMode.AddNew && _oldLevelName.Equals(el.LevelName)) ||
                           !clsEducationLevelData.Exists(el.LevelName), "Education level name already exists.")
                }
            );
            
            // Package the overall validation outcome and its corresponding error message into the output tuple.
            result = (isValid, errorMessage);

            return isValid;
        }
        private bool _Add()
        {
            this.EducationLevelID = clsEducationLevelData.Add(new EducationLevelCreationDto(this.LevelName));
            return EducationLevelID.HasValue;
        }
        private bool _Update() => clsEducationLevelData.Update(this.ToEducationLevelDto());

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

       
        public static List<EducationLevelDto> All() => clsEducationLevelData.All();

        public static List<EducationLevelCreationDto> AllLevelNames() => clsEducationLevelData.AllLevelNames();

        public static string? GetEducationLeveName(int? educationLevelId)
            => clsEducationLevelData.GetEducationLevelName(educationLevelId);

        public static int? GetEducationLeveId(string levelName)
            => clsEducationLevelData.GetEducationLevelId(levelName);
        
        public static clsEducationLevel? Find(int? educationLevelId)
        {
            EducationLevelDto? educationLevelDto = clsEducationLevelData.GetInfoById(educationLevelId);
            return (educationLevelDto != null) ? new clsEducationLevel(educationLevelDto, enMode.Update) : null;
        }
        
        public static bool Exists(int? educationLevelId)
            => clsEducationLevelData.Exists(educationLevelId);

        public static bool Exists(string levelName)
            => clsEducationLevelData.Exists(levelName);
        
        public static bool Delete(int? educationLevelId)
            => clsEducationLevelData.Delete(educationLevelId);

        
}