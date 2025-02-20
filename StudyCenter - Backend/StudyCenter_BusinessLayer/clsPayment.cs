using System.Runtime.CompilerServices;
using StudyCenter_DataAccessLayer;
using StudyCenter_DataAccessLayer.DTOs.PaymentDTOs;

namespace StudyCenter_BusinessLayer;

public class clsPayment
{
    public enum enMode
    {
        AddNew = 0,
        Update = 1
    };

    public enMode Mode;

    public enum enPaymentStatus
    {
        Pending = 0,
        Completed = 1
    };

    public int? PaymentID { get; set; }
    public int? StudentGroupID { get; set; }
    public int? SubjectGradeLevelID { get; set; }
    public decimal PaymentAmount { get; set; }
    public string? PaymentMethod { get; set; }
    public enPaymentStatus PaymentStatus { get; set; }
    public DateTime PaymentDate { get; set; }
    public int? CreatedByUserID { get; set; }

    public clsPayment(PaymentDto obj, enMode mode = enMode.AddNew)
    {
        PaymentID = obj.PaymentID;
        StudentGroupID = obj.StudentGroupID;
        SubjectGradeLevelID = obj.SubjectGradeLevelID;
        PaymentAmount = obj.PaymentAmount;
        PaymentMethod = obj.PaymentMethod;
        PaymentStatus = (enPaymentStatus)obj.PaymentStatus;
        PaymentDate = obj.PaymentDate;
        CreatedByUserID = obj.CreatedByUserID;

        Mode = mode;
    }

    public PaymentDto ToPaymentDto()
        => new PaymentDto(this.PaymentID, this.StudentGroupID, this.SubjectGradeLevelID, this.PaymentAmount,
            this.PaymentMethod, (byte)this.PaymentStatus, this.PaymentDate, this.CreatedByUserID);

    public PaymentCreationDto ToPaymentCreationDto()
        => new PaymentCreationDto(this.StudentGroupID, this.SubjectGradeLevelID, this.PaymentAmount,
            this.PaymentMethod, (byte)this.PaymentStatus, this.CreatedByUserID);

    
    private bool _Validate(out (bool isValid, string? errorMessage) result)
    {
        bool isValid = ValidationHelper.Validate(
            this,

            // ID Check: Ensure PaymentID is valid if in Update mode
            idCheck: payment => (payment.Mode != enMode.Update) || ValidationHelper.HasValue(payment.PaymentID),

            // Value Check: Ensure required properties are not null or empty
            valueCheck: payment => ValidationHelper.HasValue(payment.StudentGroupID) &&
                                   ValidationHelper.HasValue(payment.SubjectGradeLevelID) &&
                                   ValidationHelper.HasValue(payment.CreatedByUserID) &&
                                   payment.PaymentStatus is enPaymentStatus.Pending or enPaymentStatus.Completed &&
                                   payment.PaymentAmount >= 0,
            
            // Date Check: Ensure PaymentDate is not in the future if in AddNew mode
            dateCheck: payment => (payment.Mode != enMode.AddNew) ||
                                  !ValidationHelper.IsDateValid(payment.PaymentDate, DateTime.Now),

            // Retrieve the error message returned by the validation method, if available.
            // This message provides specific details about the validation failure.
            out string? errorMessage,

            // Additional Checks: There is no addition checks
            additionalChecks: null
        );

        // Package the overall validation outcome and its corresponding error message into the output tuple.
        result = (isValid, errorMessage);

        return isValid;
    }

    private bool _Add()
    {
        PaymentID = clsPaymentData.Add(this.ToPaymentCreationDto());
        return (PaymentID.HasValue);
    }

    private bool _Update() => clsPaymentData.Update(this.ToPaymentDto());

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

    public static List<PaymentDto> All()
        => clsPaymentData.All();

    public static clsPayment? Find(int? paymentID)
    {
        PaymentDto? paymentDto = clsPaymentData.GetInfoByID(paymentID);
        return (paymentDto != null) ? new clsPayment(paymentDto, enMode.Update) : null;
    }

    public static bool Delete(int? paymentID)
        => clsPaymentData.Delete(paymentID);

    public static bool Exists(int? paymentID)
        => clsPaymentData.Exists(paymentID);
}