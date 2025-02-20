using System.Data;
using Microsoft.Data.SqlClient;
using StudyCenter_DataAccessLayer.DTOs.GroupDTOs;
using StudyCenter_DataAccessLayer.Global_Classes;
using StudyCenter_DataAccessLayer.Mappings;

namespace StudyCenter_DataAccessLayer;

public class clsGroupData
{
    public static GroupDto? GetInfoById(int? groupId)
        => clsDataAccessHelper.GetBy("SP_Groups_GetGroupInfoByID", "GroupID", groupId, GroupMapping.MapToGroupDto);

    public static int? Add(GroupCreationDto newGroupDto)
        => clsDataAccessHelper.Add("SP_Groups_AddNewGroup", "NewGroupID", newGroupDto);

    public static bool Update(GroupUpdateDto updatedGroupDto)
        => clsDataAccessHelper.Update("SP_Groups_UpdateGroupInfo", updatedGroupDto);

    public static bool Delete(int? groupId)
        => clsDataAccessHelper.Delete("SP_Groups_DeleteGroup", "GroupID", groupId);

    public static bool Exists(int? groupId)
        => clsDataAccessHelper.Exists("SP_Groups_DoesGroupExists", "GroupID", groupId);

    public static List<GroupDto> All()
        => clsDataAccessHelper.All("SP_Groups_GetAllGroups", GroupMapping.MapToGroupDto);
    
}