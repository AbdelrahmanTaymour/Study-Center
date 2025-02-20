using StudyCenter_DataAccessLayer.DTOs.AttendanceDTOs;
using StudyCenter_DataAccessLayer.Global_Classes;
using StudyCenter_DataAccessLayer.Mappings;

namespace StudyCenter_DataAccessLayer;

public class clsAttendanceData
{
    public static List<AttendanceDto> All() =>
        clsDataAccessHelper.All("SP_Attendance_GetAllAttendance", AttendanceMapping.MapToAttendanceDto);

    public static AttendanceDto? GetAttendanceById(int id) => clsDataAccessHelper.GetBy(
        "SP_Attendance_GetAttendanceInfoByID", "AttendanceID", id, AttendanceMapping.MapToAttendanceDto);

    public static int? Add(AttendanceCreationDto newAttendance)
        => clsDataAccessHelper.Add("SP_Attendance_AddNewAttendance", "NewAttendanceID", newAttendance);

    public static bool Update(AttendanceDto updatedAttendance)
        => clsDataAccessHelper.Update("SP_Attendance_UpdateAttendanceInfo", updatedAttendance);
    
    public static bool Delete(int id)
        => clsDataAccessHelper.Delete("SP_Attendance_DeleteAttendance", "AttendanceID", id);

    public static bool Exists(int id)
        => clsDataAccessHelper.Exists("SP_Attendance_DoesAttendanceExists", "AttendanceID", id);

}