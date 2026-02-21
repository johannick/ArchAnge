using System.Net.Mail;
using System.Net.Mime;
using System.Security.Authentication;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Abstraction;
using Abstraction.Database;
using Abstraction.Repository;
using Microsoft.VisualBasic;
using Model.User;
using Archange.Front.Server.Smtp;
using ArchAnge.ServiceDefaults.Endpoint;
using ArchAnge.ServiceDefaults.Context;
using ArchAnge.ApiService.Smtp;

namespace ApiService.Controllers;

/// <summary>
/// User controller
/// </summary>
/// <param name="requestContext"></param>
/// <param name="repositoryUser"></param>
/// <param name="hasher"></param>
/// <param name="mapper"></param>
/// <param name="options"></param>
[AllowAnonymous]
public class UserController
    (ConnectionRequestContext requestContext
    , IRepository<UserModel> repositoryUser
    , IPasswordHasher<UserModel> hasher
    , IMapper mapper
    , IOptions<SmtpOptions> options)
    : ApiControllerBase
{
    /// <summary>
    /// Get user model
    /// </summary>
    /// <returns> <see cref="UserModel"/> </returns>
    [HttpGet(Abstraction.Constants.Empty)]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Get()
    {
        var model = new UserModel
        {
            Email = requestContext.Email ?? string.Empty,
            Password = string.Empty
        };

        if (string.IsNullOrWhiteSpace(model.Email))
            return await Task.FromResult(new StatusCodeResult(StatusCodes.Status418ImATeapot));

        var user = await repositoryUser.UniqueOrDefault(mapper.Map<UserModel>(model));

        if (user == null)
        {
            return await Task.FromResult(new StatusCodeResult(StatusCodes.Status404NotFound));
        }
        return await Task.FromResult(new JsonResult(mapper.Map<UserModel>(user)));
    }

    /// <summary>
    /// Forgot password
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Forgot([FromBody] UserForgotPasswordModel model)
    {
        var entity = await repositoryUser.UniqueOrDefault(new UserModel { Email = model.Email, Id = Guid.Empty });

        if (entity == null)
            return await Task.FromResult(NotFound());

        entity.ForgotDate = DateTime.Now;
        entity.ForgotToken = Guid.NewGuid();
        if (!await repositoryUser.Update(entity))
            return await Task.FromException<IActionResult>(new Exception("Update fail"));

        using var mailMessage = new MailMessage(new MailAddress(options.Value.Sender), new MailAddress(entity.Email))
        {
            Subject = $"[PRIVATE] Forgot token",
            IsBodyHtml = true
        };
        mailMessage.Body = CreateForgotBody(entity.ForgotToken.Value);
        using var mailer = new SmtpClient();
        mailer.PickupDirectoryLocation = requestContext.ImageFolder();
        return await mailer.SaveMailAsync(mailMessage, "forgot.eml");
    }

    private static string CreateForgotBody(Guid forgotToken)
    {
        var builder = new StringBuilder();
        var lines = new string[]
        {
            $"Application : PARADIS",
            $"ResetToken  : {forgotToken}", string.Empty,
            " This mail is automatic, do not reply to this email"
        };
        builder.AppendJoin("<br />", lines);
        return builder.ToString();
    }

    /// <summary>
    /// Reset password (call forgot before to get thje token)
    /// </summary>
    /// <param name="model"> Rest password model </param>
    /// <returns> true if password was updated </returns>
    [HttpPost]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Reset([FromBody] UserModel model)
    {
        var entity = await repositoryUser.UniqueOrDefault(mapper.Map<UserModel>(model));

        if (entity == null)
            return await Task.FromResult(StatusCode(StatusCodes.Status404NotFound));
        if (entity.ForgotToken != model.ForgotToken)
            return await Task.FromResult(StatusCode(StatusCodes.Status403Forbidden));
        if (entity.ForgotDate == null || (DateTime.Now - entity.ForgotDate) > TimeSpan.FromMinutes(15))
            return await Task.FromResult(StatusCode(StatusCodes.Status408RequestTimeout));

        entity.Password = hasher.HashPassword(model, model.Password ?? throw new AuthenticationException());
        entity.ForgotDate = null;
        entity.ForgotToken = null;
        var result = await repositoryUser.Update(entity);

        return await Task.FromResult(new JsonResult(result));
    }
}