using Microsoft.AspNetCore.Mvc;
using StudyCenter_BusinessLayer;
using StudyCenter_DataAccessLayer.DTOs.PaymentDTOs;

namespace StudyCenter_API.Controllers;

[Route("api/Payments")]
[ApiController]
public class PaymentsController : ControllerBase
{
    [HttpGet(Name = "GetPayments")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<PaymentDto>> GetPayments()
    {
        var payments = clsPayment.All();
        if (payments.Count == 0) return NotFound("No Payments found");
        return Ok(payments);
    }
    
    [HttpGet("{id:int}", Name = "GetPaymentById")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<PaymentDto> GetPaymentById(int id)
    {
        if (id < 1) return BadRequest($"Not accepted ID {id}");
        clsPayment? payment = clsPayment.Find(id);
        if (payment is null) return NotFound($"Payment with id {id} not found");
        return Ok(payment.ToPaymentDto());
    }
    
    [HttpGet("exists/{id:int}", Name = "ExistsByPaymentId")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult ExistsByPaymentId(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (clsPayment.Exists(id)) return Ok(true);
        return NotFound(false);
    }
    
    [HttpPost(Name = "AddPayment")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<PaymentDto> AddPayment(PaymentCreationDto? newPayment)
    {
        if (newPayment is null) return BadRequest($"Not accepted data.");
        clsPayment payment = new clsPayment
        (
            new PaymentDto
            (
                null,
                newPayment.StudentGroupID,
                newPayment.SubjectGradeLevelID,
                newPayment.PaymentAmount,
                newPayment.PaymentMethod,
                newPayment.PaymentStatus,
                DateTime.Today, 
                newPayment.CreatedByUserID
            )
        );

        if (!payment.Save(out string? validationMessage))
        {
            return !string.IsNullOrWhiteSpace(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error adding payment" });
        }

        return CreatedAtRoute("GetPaymentById", new { id = payment.PaymentID },
            payment.ToPaymentDto());
    }
    
    [HttpPut(Name = "UpdatePayment")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<PaymentDto> UpdatePayment(int id, PaymentDto? updatedPayment)
    {
        if (id < 1 || updatedPayment is null) return BadRequest($"Not accepted data.");
        clsPayment? payment = clsPayment.Find(id);
        if (payment is null) return NotFound($"Payment with id {id} not found");

        payment.StudentGroupID = updatedPayment.StudentGroupID;
        payment.SubjectGradeLevelID = updatedPayment.SubjectGradeLevelID;
        payment.PaymentAmount = updatedPayment.PaymentAmount;
        payment.PaymentMethod = updatedPayment.PaymentMethod;
        payment.PaymentStatus = (clsPayment.enPaymentStatus)updatedPayment.PaymentStatus;
        payment.PaymentDate = updatedPayment.PaymentDate;
        payment.CreatedByUserID = updatedPayment.CreatedByUserID;

        if (!payment.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error updating Payment" });
        }

        return Ok(payment.ToPaymentDto());
    }
    
    [HttpDelete("{id:int}", Name = "DeletePayment")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult DeletePayment(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (!clsPayment.Exists(id)) return NotFound($"Payment with id {id} not found");
        return clsPayment.Delete(id)
            ? Ok($"Payment with ID {id} has been deleted.")
            : StatusCode(500, new { message = "Error deleting payment" });
    }
    
    
}