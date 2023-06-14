using System.Net;
using Api.Requests.SendMail;
using Application.SendMail.Services.Interfaces;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.MailSends;

[Route("mailsends")]
[Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
public class MailSendController : Controller
{
    private readonly ISendMailService _sendMail;

    public MailSendController(ISendMailService sendMail)
    {
        _sendMail = sendMail;
    }

    [Route("send")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> SendMail([FromBody] MailRequest request)
    {
        var statusEmail = await _sendMail.SendMail(request.EventId, request.Subject, request.Message);

        if (statusEmail)
        {
            return Ok(statusEmail);
        }

        return BadRequest(statusEmail);
    }

    [Route("sendOne")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> SendOneMail([FromBody] MailOneRequest request)
    {
        var statusEmail = await _sendMail.SendOneMail(request.EventId, request.Email);

        if (statusEmail)
        {
            return Ok(statusEmail);
        }

        return BadRequest(statusEmail);
    }
}