using StudyCenter_DataAccessLayer.DTOs.PaymentDTOs;
using StudyCenter_DataAccessLayer.Global_Classes;
using StudyCenter_DataAccessLayer.Mappings;

namespace StudyCenter_DataAccessLayer;

public class clsPaymentData
{
    public static List<PaymentDto> All()
        => clsDataAccessHelper.All("SP_Payments_GetAllPayments", PaymentMapping.MapToPaymentDto);

    public static PaymentDto? GetInfoByID(int? paymentID)
        => clsDataAccessHelper.GetBy("SP_Payments_GetPaymentInfoByID", "PaymentID", paymentID,
            PaymentMapping.MapToPaymentDto);

    public static int? Add(PaymentCreationDto newPayment)
        => clsDataAccessHelper.Add("SP_Payments_AddNewPayment", "NewPaymentID", newPayment);

    public static bool Update(PaymentDto updatedPayment)
        => clsDataAccessHelper.Update("SP_Payments_UpdatePaymentInfo", updatedPayment);

    public static bool Delete(int? paymentID)
        => clsDataAccessHelper.Delete("SP_Payments_DeletePayment", "PaymentID", paymentID);

    public static bool Exists(int? paymentID)
        => clsDataAccessHelper.Exists("SP_Payments_DoesPaymentExists", "PaymentID", paymentID);
}