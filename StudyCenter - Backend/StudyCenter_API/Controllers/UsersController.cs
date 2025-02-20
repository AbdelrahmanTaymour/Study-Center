using Microsoft.AspNetCore.Mvc;
using StudyCenter_BusinessLayer;
using StudyCenter_DataAccessLayer;
using StudyCenter_DataAccessLayer.DTOs;

namespace StudyCenter_API.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpGet("all", Name = "GetAllUsers")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<UserViewDto>> GetAllUsers()
    {
        List<UserViewDto> users = clsUserData.GetAllUsers();
        if(users is null || users.Count == 0) return NotFound("No users found");
        return Ok(users);
    }

    [HttpGet("{userId}", Name = "GetUserById")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<UserDto> GetUserById(int? userId)
    {
        if (userId is null || userId < 1) return BadRequest($"Not accepted ID {userId}");
        clsUser? user = clsUser.FindByID(userId);
        if (user is null) return NotFound($"User with id {userId} not found");
        return Ok(user.ToUserDto());
    }

    [HttpGet("person/{personId}", Name = "GetUserByPersonId")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<UserDto> GetUserByPersonId(int? personId)
    {
        if(personId is null || personId < 1) return BadRequest($"Not accepted ID {personId}");
        var user = clsUser.FindByPersonID(personId);
        if(user == null) return NotFound($"User with id {personId} not found");
        return Ok(user.ToUserDto());
    }

    [HttpGet("username", Name = "GetUserByUsername")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<UserDto> GetUserByUsername(string username)
    {
        if(string.IsNullOrWhiteSpace(username)) return BadRequest($"Not accepted data");
        var user = clsUser.FindByUsername(username);
        if(user is null) return NotFound($"User with username {username} not found");
        return Ok(user.ToUserDto());
    }

    [HttpGet("username-password", Name = "Login")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<UserDto> Login(string username, string password)
    {
        if(string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) return BadRequest($"Not accepted data");
        var user = clsUser.Login(username, password);
        if(user is null) return NotFound($"User with this username/password is not found.");
        return Ok(user.ToUserDto());
    }

    [HttpPost("", Name = "AddUser")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<UserDto> AddUser(UserCreationDto? newUser)
    {
        if(newUser is null) return BadRequest($"Not accepted data");

        clsUser user = new clsUser
        (
            new UserDto
            (
                null,
                newUser.PersonID,
                newUser.Username,
                newUser.Password,
                newUser.Permissions,
                newUser.IsActive
            )
        );

        if (!user.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new{message = "Error adding user"});
        }
        
        return CreatedAtRoute("GetUserById", new{userId = user.UserID}, user.ToUserDto());
    }
    
    [HttpPut("", Name = "UpdateUser")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<UserDto> UpdateUser(int? userId, UserDto updatedUser)
    {
        if(updatedUser is null || userId < 1) return BadRequest($"Not accepted data.");
        clsUser? user = clsUser.FindByID(userId);
        if(user is null) return NotFound($"User with id {userId} not found");
        
        user.PersonID = updatedUser.PersonID;
        user.Username = updatedUser.Username;
        user.Password = updatedUser.Password;
        user.Permissions = updatedUser.Permissions;
        user.IsActive = updatedUser.IsActive;

        if (!user.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new{message = validationMessage});
        }
        
        return Ok(user.ToUserDto());
    }

    [HttpPut("password", Name = "ChangePassword")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<bool> ChangePassword(int? userId, string newPassword)
    {
        if(userId is null || userId < 1 || string.IsNullOrWhiteSpace(newPassword)) return BadRequest($"Not accepted data.");
        if (clsUserData.ChangePassword(userId, newPassword)) return Ok(true);
        return StatusCode(500, new{message = "Error changing password"});
    }

    [HttpDelete("{userId}", Name = "DeleteUser")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult DeleteUser(int? userId)
    {
        if(userId < 1) return BadRequest($"Not accepted data.");
        if(!clsUser.ExistByUserID(userId)) return NotFound($"User with id {userId} not found");
        if(clsUser.Delete(userId)) return Ok($"User with ID {userId} has been deleted.");
        return StatusCode(500, new { message = "Error deleting user" });
    }

    [HttpGet("exists/user-id/{userId}", Name = "ExistsByUserId")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult ExistsByUserId(int? userId)
    {
        if(userId is null || userId < 1) return BadRequest($"Not accepted data.");
        if (clsUser.ExistByUserID(userId)) return Ok(true);
        return NotFound(false);
    }

    [HttpGet("exists/{personId}", Name = "ExistsByPersonId")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult ExistsByPersonId(int? personId)
    {
        if(personId is null || personId < 1) return BadRequest($"Not accepted data.");
        if (clsUserData.ExistsByPersonID(personId)) return Ok(true);
        return NotFound(false);
    }

    [HttpGet("exists/username-password", Name = "ExistsByUsernamePassword")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult ExistsByUsernamePassword(string username, string password)
    {
        if(string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) return BadRequest($"Not accepted data.");
        if(clsUserData.ExistsByUsernameAndPassword(username, password)) return Ok(true);
        return NotFound(false);
    }
    
    [HttpGet("exists/username/{username}", Name = "ExistsByUsername")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult ExistsByUsername(string? username)
    {
        if(string.IsNullOrWhiteSpace(username)) return BadRequest($"Not accepted data.");
        if(clsUser.ExistsByUsername(username)) return Ok(true);
        return NotFound(false);
    }
    
    
    
}