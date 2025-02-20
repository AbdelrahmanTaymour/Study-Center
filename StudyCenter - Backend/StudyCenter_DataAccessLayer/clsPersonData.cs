using StudyCenter_DataAccessLayer.DTOs.PersonDTOs;
using StudyCenter_DataAccessLayer.Global_Classes;
using StudyCenter_DataAccessLayer.Mappings;

namespace StudyCenter_DataAccessLayer;

public class clsPersonData
{
    public static List<PersonViewDto> GetAllPeople()
        => clsDataAccessHelper.All("SP_People_GetAllPeople", PersonMapping.MapToPersonViewDto);
    public static PersonDto? GetInfoById(int? personId)
        => clsDataAccessHelper.GetBy("SP_People_GetPersonInfoByID", "PersonID", personId, PersonMapping.MapToPersonDto);

    public static int? Add(PersonCreationDto personDto)
        => clsDataAccessHelper.Add("SP_People_AddNewPerson", "NewPersonID", personDto);

    public static bool Update(PersonDto personDto)
        => clsDataAccessHelper.Update("SP_People_UpdatePersonInfo", personDto);

    public static bool Delete(int? personID)
        => clsDataAccessHelper.Delete("SP_People_DeletePerson", "PersonID", personID);

    public static bool Exists(int? personID)
        => clsDataAccessHelper.Exists("SP_People_CheckIfPersonExists", "PersonID", personID);

    
}