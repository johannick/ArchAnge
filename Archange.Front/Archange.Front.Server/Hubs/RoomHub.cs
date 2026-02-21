using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Abstraction;
using Abstraction.Database;
using Abstraction.Repository;
using Model.Hub;
using ArchAnge.ServiceDefaults.Endpoint;
using ArchAnge.ServiceDefaults.Exception;

namespace ArchAnge.ApiService.Hubs;

/// <summary>
/// Room hub
/// </summary>
/// <param name="requestContext"></param>
/// <param name="repositoryMessages"></param>
/// <param name="repositoryParticipant"></param>
public class RoomHub
    (ConnectionRequestContext requestContext
    , IRepository<MessageModel> repositoryMessages
    , IRepository<RoomParticipantModel> repositoryParticipant)
    : ApiHubBase
{
    /// <summary>
    /// On connected
    /// </summary>
    /// <returns></returns>
    public override async Task OnConnectedAsync()
    {
        var userId = requestContext.Id ?? throw new UserNotConnectedException();
        var user = new Entity<Guid> { Id = userId };
        var rooms = await repositoryParticipant.Where(user).ToListAsync(Context.ConnectionAborted);
        rooms = [.. rooms.DistinctBy(participant => participant.IdRoom)];

        foreach (var room in rooms)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room.IdRoom.ToString());
        }
    }

    /// <summary>
    /// On disconnected
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = requestContext.Id;

        if (!userId.HasValue)
            return;

        var user = new Entity<Guid> { Id = userId.Value };
        var rooms = await repositoryParticipant.Where(user).ToListAsync(Context.ConnectionAborted);

        rooms = [.. rooms.DistinctBy(participant => participant.IdRoom)];
        foreach (var room in rooms)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.IdRoom.ToString());
        }
    }

    /// <summary>
    /// Send message to room
    /// </summary>
    /// <param name="message"></param>
    /// <remarks> Add security according to Message type, (IPacket ?) </remarks>
    /// <returns> </returns>
    public async Task SendMessageToRoom(MessageModel message)
    {
        await repositoryMessages.Insert(message);
        await Clients.Group(message.ToIdRoom.ToString()).SendAsync("MessageReceived", message);
    }
}
