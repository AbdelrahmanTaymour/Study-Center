using System.Data;
using StudyCenter_DataAccessLayer.DTOs.StudentAnswerDTOs;

namespace StudyCenter_DataAccessLayer.Mappings;

public class StudentAnswerMapping
{
    public static StudentAnswerDto MapToStudentAnswer(IDataRecord record)
    {
        return new StudentAnswerDto
        (
            record.GetInt32(record.GetOrdinal("StudentAnswerID")),
            record.GetInt32(record.GetOrdinal("ExamResultID")),
            record.GetInt32(record.GetOrdinal("QuestionID")),
            record.GetValue(record.GetOrdinal("AnswerID")) as int? ?? null,
            record.GetValue(record.GetOrdinal("AnswerText")) as string ?? null,
            record.GetValue(record.GetOrdinal("MarksAwarded")) as decimal? ?? null
        );
    }

    public static StudentAnswerCreationDto MapToStudentAnswerCreation(IDataRecord record)
    {
        return new StudentAnswerCreationDto
        (
            record.GetInt32(record.GetOrdinal("ExamResultID")),
            record.GetInt32(record.GetOrdinal("QuestionID")),
            record.GetValue(record.GetOrdinal("AnswerID")) as int? ?? null,
            record.GetValue(record.GetOrdinal("AnswerText")) as string ?? null,
            record.GetValue(record.GetOrdinal("MarksAwarded")) as decimal? ?? null
        );
    }
}