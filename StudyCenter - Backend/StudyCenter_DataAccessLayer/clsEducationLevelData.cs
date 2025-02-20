using StudyCenter_DataAccessLayer.DTOs.EducationLevelDTOs;
using StudyCenter_DataAccessLayer.Global_Classes;
using StudyCenter_DataAccessLayer.Mappings;

namespace StudyCenter_DataAccessLayer;

public class clsEducationLevelData
{
    public static EducationLevelDto? GetInfoById(int? educationLevelId) =>
        clsDataAccessHelper.GetBy("SP_EducationLevels_GetEducationLevelInfoByID", "EducationLevelID", educationLevelId,
            EducationLevelMapping.MapToEducationLevelDto);

    public static int? Add(EducationLevelCreationDto educationLevelCreationDto) => clsDataAccessHelper.Add(
        "SP_EducationLevels_AddNewEducationLevel",
        "NewEducationLevelID", educationLevelCreationDto);

    public static bool Update(EducationLevelDto updatedEducationLevel) =>
        clsDataAccessHelper.Update("SP_EducationLevels_UpdateEducationLevelInfo", updatedEducationLevel);

    public static bool Delete(int? educationLevelId)
        => clsDataAccessHelper.Delete("SP_EducationLevels_DeleteEducationLevel", "EducationLevelID", educationLevelId);

    public static bool Exists(int? educationLevelId)
        => clsDataAccessHelper.Exists("SP_EducationLevels_DoesEducationLevelExists", "EducationLevelID",
            educationLevelId);

    public static bool Exists(string levelName)
        => clsDataAccessHelper.Exists("SP_EducationLevels_DoesEducationLevelExistsByLevelName", "LevelName", levelName);

    public static List<EducationLevelDto> All()
        => clsDataAccessHelper.All("SP_EducationLevels_GetAllEducationLevels",
            EducationLevelMapping.MapToEducationLevelDto);

    public static List<EducationLevelCreationDto> AllLevelNames()
        => clsDataAccessHelper.All("SP_EducationLevels_GetAllEducationLevelsName",
            EducationLevelMapping.MapToEducationLevelCreationDto);

    public static string? GetEducationLevelName(int? educationLevelId) =>
        clsDataAccessHelper.GetStringValue("SP_EducationLevels_GetEducationLevelName", "EducationLevelID",
            educationLevelId, "LevelName", 50);

    public static int? GetEducationLevelId(string educationLevelName) =>
        clsDataAccessHelper.GetIntValue("SP_EducationLevels_GetEducationLevelID", "LevelName",
            educationLevelName, "EducationLevelID");
}