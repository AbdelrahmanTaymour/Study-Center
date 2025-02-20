using Microsoft.AspNetCore.Mvc;
using StudyCenter_BusinessLayer;
using StudyCenter_DataAccessLayer.DTOs.PersonDTOs;

namespace StudyCenter_API.Controllers;

[Route("api/people")]
[ApiController]
public class PeopleController : Controller
{

    [HttpGet(Name = "GetAllPeople")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<PersonViewDto>> GetAllPeople()
    {
        List<PersonViewDto> people = clsPerson.GetAllPeople();
        if(people is null || people.Count == 0) return NotFound("No people found!");
        return Ok(people);
    }
    
    
    [HttpGet("{personId}", Name = "GetPersonById")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<PersonDto> GetPersonById(int? personId)
    {
        if(personId < 1) 
            return BadRequest($"Not accepted ID {personId}");

        clsPerson? person = clsPerson.Find(personId);
        
        if(person == null) return NotFound($"User with ID {personId} is not found.");
        
        return Ok(person.ToPersonDto());
    }
    
    [HttpPost("", Name = "AddPerson")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<PersonDto> AddPerson(PersonCreationDto newPerson)
    {
        if (newPerson == null) return BadRequest("Not accepted data");

        clsPerson person = new clsPerson
        (
            new PersonDto
            (
                null,
                newPerson.FirstName,
                newPerson.SecondName,
                newPerson.ThirdName,
                newPerson.LastName,
                newPerson.Gender,
                newPerson.DateOfBirth,
                newPerson.PhoneNumber,
                newPerson.Email,
                newPerson.Address
            )
        );

        if (!person.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage }) // Return specific validation message
                : StatusCode(500, new { message = "Error adding person" });
        }
        return CreatedAtRoute("GetPersonById", new { personId = person.PersonID }, person.ToPersonDto());
    }

    [HttpPut("{personID}", Name = "UpdatePerson")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<PersonDto> UpdatePerson(int personID, PersonDto? updatedPerson)
    {
        if (updatedPerson is null || personID < 1) return BadRequest("Not accepted data");
        
        clsPerson? person = clsPerson.Find(personID);
        if(person is null) return NotFound($"User with ID {personID} is not found.");
        
        person.FirstName = updatedPerson.FirstName;
        person.SecondName = updatedPerson.SecondName;
        person.ThirdName = updatedPerson?.ThirdName;
        person.LastName = updatedPerson?.LastName;
        person.Gender = (clsPerson.enGender)updatedPerson.Gender;
        person.DateOfBirth = updatedPerson.DateOfBirth;
        person.PhoneNumber = updatedPerson.PhoneNumber;
        person.Email = updatedPerson?.Email;
        person.Address = updatedPerson?.Address;

        if (!person.Save(out string? validationMessage))
        {
            return !String.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error updating person" });
        }
        
        return Ok(person.ToPersonDto());
    }

    [HttpDelete("{Id}", Name = "DeletePerson")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult DeletePerson(int Id)
    {
        if (Id < 1) return BadRequest("Not accepted data");
        if(!clsPerson.ExistsByID(Id)) return NotFound($"User with ID {Id} is not found.");
        if(clsPerson.Delete(Id)) return Ok($"Person with ID {Id} has been deleted.");
        return StatusCode(500, new { message = "Error deleting person" });
    }

    [HttpGet("exists/{Id}", Name = "ExistsPersonByPersonId")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult ExistsPersonByPersonId(int Id)
    {
        if (Id<1) return BadRequest("Not accepted data");
        if(clsPerson.ExistsByID(Id)) return Ok(true);
        return NotFound(false);
    }
}