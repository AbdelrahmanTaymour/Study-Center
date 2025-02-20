using System.Data;
using StudyCenter_DataAccessLayer.DTOs.QuestionDTOs;

namespace StudyCenter_DataAccessLayer.Mappings;

public class QuestionMapping
{
    public static QuestionDto MapToQuestionDto(IDataRecord record)
    {
        return new QuestionDto
        (
            record.GetInt32(record.GetOrdinal("QuestionID")),
            record.GetInt32(record.GetOrdinal("ExamID")),
            record.GetString(record.GetOrdinal("QuestionText")),
            (byte)record.GetValue(record.GetOrdinal("QuestionType")),
            record.GetDecimal(record.GetOrdinal("Marks"))
        );
    }

    public static QuestionCreationDto MapToQuestionCreationDto(IDataRecord record)
    {
        return new QuestionCreationDto
        (
            record.GetInt32(record.GetOrdinal("ExamID")),
            record.GetString(record.GetOrdinal("QuestionText")),
            (byte) record.GetValue(record.GetOrdinal("QuestionType")),
            record.GetDecimal(record.GetOrdinal("Marks"))
        );
    }
}