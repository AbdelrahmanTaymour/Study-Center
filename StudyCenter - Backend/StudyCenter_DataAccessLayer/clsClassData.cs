using StudyCenter_DataAccessLayer.DTOs.ClassDTOs;
using StudyCenter_DataAccessLayer.Global_Classes;
using StudyCenter_DataAccessLayer.Mappings;

namespace StudyCenter_DataAccessLayer;

public class clsClassData
{
    public static List<ClassDto> All()
        => clsDataAccessHelper.All("SP_Classes_GetAllClasses", ClassMapping.MapToClassDto);

    public static ClassDto? GetInfoById(int? classId) => clsDataAccessHelper.GetBy("SP_Classes_GetClassInfoByID",
        "ClassID", classId, ClassMapping.MapToClassDto);

    public static bool Exists(int? classId)
        => clsDataAccessHelper.Exists("SP_Classes_DoesClassExists", "ClassID", classId);

    public static bool Exists(string className)
        => clsDataAccessHelper.Exists("SP_Classes_DoesClassExistByClassName", "ClassName", className);

    public static int? Add(ClassCreationDto classCreationDto)
        => clsDataAccessHelper.Add("SP_Classes_AddNewClass", "NewClassID", classCreationDto);

    public static bool Update(ClassDto updatedClassDto) =>
        clsDataAccessHelper.Update("SP_Classes_UpdateClassInfo", updatedClassDto);

    public static bool Delete(int? classId)
        => clsDataAccessHelper.Delete("SP_Classes_DeleteClass", "ClassID", classId);
}