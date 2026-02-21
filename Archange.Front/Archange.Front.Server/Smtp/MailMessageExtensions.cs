using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Reflection;
namespace ArchAnge.ApiService.Smtp;

/// <summary>
/// Mail message extensions
/// </summary>
public static class MailMessageExtensions
{
    /// <summary>
    /// Save mail to eml
    /// </summary>
    /// <param name="client"></param>
    /// <param name="message"></param>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static async Task<IActionResult> SaveMailAsync(this SmtpClient client, MailMessage message, string filename)
    {
        if (client.PickupDirectoryLocation == null)
        {
            await client.SendMailAsync(message).ConfigureAwait(false);
            return await Task.FromResult(new JsonResult(true));
        }

        using var memoryStream = new MemoryStream();
        using var fileStream = new FileStream(Path.Combine(client.PickupDirectoryLocation, filename), FileMode.Create);
        var content = message.ToEml(memoryStream);

        await fileStream.WriteAsync(content).ConfigureAwait(false);
        return await Task.FromResult(new JsonResult(true));
    }

    /// <summary>
    /// Get mail as content
    /// </summary>
    /// <param name="message"></param>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static byte[] ToEml(this MailMessage message, MemoryStream stream)
    {
        var assembly = message.GetType().Assembly;
        var mailWriterType = assembly.GetType($"{assembly.GetName()}.MailWriter");
        var flags = (BindingFlags)36; // BindingFlags.Instance | BindingFlags.NonPublic
        var mailWriterContructor = mailWriterType?.GetConstructor(flags, null, [typeof(System.IO.Stream)], null);
        var mailWriter = mailWriterContructor?.Invoke([stream]);
        var sendMethod = typeof(MailMessage).GetMethod("Send", flags);

        sendMethod?.Invoke(message, flags, null, [mailWriter, true, true], null);

        var closeMethod = mailWriter?.GetType().GetMethod("Close", flags);

        closeMethod?.Invoke(mailWriter, flags, null, [], null);

        return stream.ToArray();
    }
}