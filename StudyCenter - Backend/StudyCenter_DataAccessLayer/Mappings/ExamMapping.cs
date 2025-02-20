using System.Data;
using StudyCenter_DataAccessLayer.DTOs.ExamDTOs;

namespace StudyCenter_DataAccessLayer.Mappings;

public class ExamMapping
{
    public static ExamDto MapToExamDto(IDataRecord record)
    {
        return new ExamDto
        (
            record.GetInt32(record.GetOrdinal("ExamID")),
            record.GetInt32(record.GetOrdinal("SubjectGradeLevelID")),
            record.GetString(record.GetOrdinal("ExamName")),
            record.GetDateTime(record.GetOrdinal("ExamDate")),
            record.GetDecimal(record.GetOrdinal("TotalMarks")),
            record.GetDecimal(record.GetOrdinal("PassingMarks"))
        );
    }

    public static ExamCreationDto MapToExamCreationDto(IDataRecord record)
    {
        return new ExamCreationDto
        (
            record.GetInt32(record.GetOrdinal("SubjectGradeLevelID")),
            record.GetString(record.GetOrdinal("ExamName")),
            record.GetDateTime(record.GetOrdinal("ExamDate")),
            record.GetDecimal(record.GetOrdinal("TotalMarks")),
            record.GetDecimal(record.GetOrdinal("PassingMarks"))
        );
    }
}