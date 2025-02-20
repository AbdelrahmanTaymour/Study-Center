using System.Data;
using StudyCenter_DataAccessLayer.DTOs.AnswerDTOs;

namespace StudyCenter_DataAccessLayer.Mappings;

public class AnswerMapping
{
    public static AnswerDto MapToAnswerDto(IDataRecord record)
    {
        return new AnswerDto
        (
            record.GetInt32(record.GetOrdinal("AnswerID")),
            record.GetInt32(record.GetOrdinal("QuestionID")),
            record.GetString(record.GetOrdinal("AnswerText")),
            record.GetBoolean(record.GetOrdinal("IsCorrect"))
        );
    }

    public static AnswerCreationDto MapToAnswerCreationDto(IDataRecord record)
    {
        return new AnswerCreationDto
        (
            record.GetInt32(record.GetOrdinal("QuestionID")),
            record.GetString(record.GetOrdinal("AnswerText")),
            record.GetBoolean(record.GetOrdinal("IsCorrect"))
        );
    }
}