using StudyCenter_DataAccessLayer.DTOs.GradeLevelDTOs;
using StudyCenter_DataAccessLayer.Global_Classes;
using StudyCenter_DataAccessLayer.Mappings;

namespace StudyCenter_DataAccessLayer;

public class clsGradeLevelData
{

    public static GradeLevelDto? GetInfoById(int? gradeId) =>
        clsDataAccessHelper.GetBy("SP_GradeLevels_GetGradeLevelInfoByID", "GradeLevelID", gradeId,
            GradeLevelMapping.MapToGradeLevelDto);
    
    
    public static int? GetGradeLevelId(string gradeName) => clsDataAccessHelper.GetIntValue(
        "SP_GradeLevels_GetGradeLevelID", "GradeName", gradeName, "GradeLevelID");
    

    public static int? Add(GradeLevelCreationDto gradeLevelDto)
        => clsDataAccessHelper.Add("SP_GradeLevels_AddNewGradeLevel", "NewGradeLevelID",
           gradeLevelDto);

    public static bool Update(GradeLevelDto gradeLevelDto) =>
        clsDataAccessHelper.Update("SP_GradeLevels_UpdateGradeLevel", gradeLevelDto);

    public static bool Delete(int? gradeLevelId) =>
        clsDataAccessHelper.Delete("SP_GradeLevels_DeleteGradeLevel", "GradeLevelID", gradeLevelId);

    public static bool Exists(int? gradeLevelId) =>
        clsDataAccessHelper.Exists("SP_GradeLevels_DoesGradeLevelExistByGradeLevelID", "GradeLevelID", gradeLevelId);

    public static bool Exists(string? gradeName) =>
        clsDataAccessHelper.Exists($"SP_GradeLevels_DoesGradeLevelExistByGradeLevelName", "GradeName", gradeName);

    public static List<GradeLevelDto> GetAllGradeLevels()
        => clsDataAccessHelper.All("SP_GradeLevels_GetAllGradeLevels",
            GradeLevelMapping.MapToGradeLevelDto);

    public static List<GradeLevelCreationDto> GetAllGradeLevelsName() =>
        clsDataAccessHelper.All("SP_GradeLevels_GetAllGradeLevelsName", GradeLevelMapping.MapToGradeLevelCreationDto);

}