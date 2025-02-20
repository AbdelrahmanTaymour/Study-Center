using System.Data;
using StudyCenter_DataAccessLayer.DTOs.ExamResultDTOs;

namespace StudyCenter_DataAccessLayer.Mappings;

public class ExamResultMapping
{
    public static ExamResultDto MapToExamResultDto(IDataRecord record)
    {
        return new ExamResultDto
        (
            record.GetInt32(record.GetOrdinal("ExamResultID")),
            record.GetInt32(record.GetOrdinal("ExamID")),
            record.GetInt32(record.GetOrdinal("StudentID")),
            record.GetDecimal(record.GetOrdinal("MarksObtained"))
        );
    }

    public static ExamResultCreationDto MapToExamResultCreationDto(IDataRecord record)
    {
        return new ExamResultCreationDto
        (
            record.GetInt32(record.GetOrdinal("ExamID")),
            record.GetInt32(record.GetOrdinal("StudentID")),
            record.GetDecimal(record.GetOrdinal("MarksObtained"))
        );
    }
}