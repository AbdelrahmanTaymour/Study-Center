using System.Data;
using StudyCenter_DataAccessLayer.DTOs.PaymentDTOs;

namespace StudyCenter_DataAccessLayer.Mappings;

public class PaymentMapping
{
    public static PaymentDto MapToPaymentDto(IDataRecord record)
    {
        return new PaymentDto
        (
            record.GetInt32(record.GetOrdinal("PaymentID")),
            record.GetInt32(record.GetOrdinal("StudentGroupID")),
            record.GetInt32(record.GetOrdinal("SubjectGradeLevelID")),
            record.GetDecimal(record.GetOrdinal("PaymentAmount")),
            record.GetValue(record.GetOrdinal("PaymentMethod")) as string ?? null,
            (byte) record.GetValue(record.GetOrdinal("PaymentStatus")),
            record.GetDateTime(record.GetOrdinal("PaymentDate")),
            record.GetInt32(record.GetOrdinal("CreatedByUserID"))
        );
    }

    public static PaymentCreationDto MapToPaymentCreationDto(IDataRecord record)
    {
        return new PaymentCreationDto
        (
            record.GetInt32(record.GetOrdinal("StudentGroupID")),
            record.GetInt32(record.GetOrdinal("SubjectGradeLevelID")),
            record.GetDecimal(record.GetOrdinal("PaymentAmount")),
            record.GetValue(record.GetOrdinal("PaymentMethod")) as string ?? null,
            (byte) record.GetValue(record.GetOrdinal("PaymentStatus")),
            record.GetInt32(record.GetOrdinal("CreatedByUserID"))
        );
    }
}