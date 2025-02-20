using System.Text.RegularExpressions;

namespace StudyCenter_BusinessLayer;

public static class ValidationHelper
{
    /// <summary>
    /// Validates an entity based on multiple conditions.
    /// </summary>
    /// <typeparam name="T">The type of entity being validated.</typeparam>
    /// <param name="entity">The object to validate.</param>
    /// <param name="idCheck">Check for ID validity.</param>
    /// <param name="valueCheck">Check to ensure a value is valid.</param>
    /// <param name="dateCheck">Check for valid dates.</param>
    /// <param name="errorMessage">Outputs the first validation failure message.</param>
    /// <param name="additionalChecks">Additional validation rules with error messages.</param>
    /// <returns>True if all checks pass; otherwise, false with an error message.</returns>
    public static bool Validate<T>(
        T entity,
        Func<T, bool>? idCheck,
        Func<T, bool>? valueCheck,
        Func<T, bool>? dateCheck,
        out string? errorMessage,
        params (Func<T, bool> condition, string errorMessage)[]? additionalChecks)
    {
        errorMessage = null;

        if 
         (
             (idCheck != null && !idCheck(entity)) ||
             (valueCheck != null && !valueCheck(entity)) ||
             (dateCheck != null && !dateCheck(entity))
         )
        {
            errorMessage = "Missing data!";
            return false;
        }

        if (additionalChecks == null) return true;
        foreach (var (condition, errMsg) in additionalChecks)
        {
            if (condition(entity)) continue;
            errorMessage = errMsg; // Capture the first failing validation message
            return false;
        }

        return true;
    }

    public static bool HasValue<T>(T? value) where T : struct
    {
        return value.HasValue;
    }
    
    public static bool IsNotEmpty(string? value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }
    
    public static bool IsDateValid(DateTime? date, DateTime referenceDate, bool isBefore = true)
    {
        
        return date != null && (isBefore ? date.Value.Date < referenceDate.Date : date.Value.Date > referenceDate.Date);
    }

    public static bool IsValidPhoneNumber(string phoneNumber)
    {
        Regex pattern = new Regex(@"^\+[0-9].*");
        return pattern.IsMatch(phoneNumber);
    }
    
    public static bool IsValidEmail(string email)
    {
        Regex pattern = new Regex(@"^.+@.+\..+$");
        return pattern.IsMatch(email);
    }

}