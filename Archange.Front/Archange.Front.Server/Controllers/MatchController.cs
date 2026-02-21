using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Abstraction.Database;
using Abstraction.Repository;
using Model.Hub;
using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.RegularExpressions;
using ArchAnge.ApiService.Hubs;
using ArchAnge.ServiceDefaults.Endpoint;
using ArchAnge.ServiceDefaults.Exception;

namespace ApiService.Controllers;

/// <summary>
/// Match controller
/// </summary>
/// <param name="requestContext"></param>
/// <param name="repositoryMatch"></param>
/// <param name="repositoryRoom"></param>
/// <param name="repositoryParticipant"></param>
/// <param name="hubContext"></param>
/// <param name="mapper"></param>
public class MatchController
    (ConnectionRequestContext requestContext
    , IRepository<MatchModel> repositoryMatch
    , IRepository<RoomModel> repositoryRoom
    , IRepository<RoomParticipantModel> repositoryParticipant
    , IHubContext<RoomHub> hubContext
    , IMapper mapper)
    : ApiControllerBase
{
    /// <summary>
    /// Like method
    /// </summary>
    /// <param name="idProfile"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(MatchModel), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Like(Guid idProfile, MatchStatus status)
    {
        var userId = requestContext.Id ?? throw new UserNotConnectedException();
        var reverseMatch = new MatchModel
        {
            FromIdProfile = idProfile,
            ToIdProfile = userId,
            Status = MatchStatus.Pending,
            CreatedAt = DateTime.MinValue
        };
        var newMatch = new MatchModel()
        {
            CreatedAt = DateTime.UtcNow,
            FromIdProfile = userId,
            ToIdProfile = idProfile,
            Status = status == MatchStatus.Rejected ? MatchStatus.Rejected : MatchStatus.Pending
        };
        var matchExist = await repositoryMatch.UniqueOrDefault(reverseMatch);
        var model = await MatchBack(matchExist, newMatch, status, idProfile);

        await repositoryMatch.Insert(newMatch);
        return await Task.FromResult(new JsonResult(model));
    }

    private async Task<MatchModel> MatchBack(MatchModel? matchExist, MatchModel newMatch, MatchStatus status, Guid idProfile)
    {
        if (matchExist == null)
        {
            return await Task.FromResult(mapper.Map<MatchModel>(newMatch));
        }

        newMatch.Status = matchExist.Status == MatchStatus.Pending ? status : MatchStatus.Rejected;

        if (newMatch.Status == MatchStatus.Accepted)
        {
            var userId = requestContext.Id ?? throw new UserNotConnectedException();

            newMatch.IdRoom = await MatchAccepted(userId, idProfile, matchExist);
        }
        return await Task.FromResult(mapper.Map<MatchModel>(newMatch));
    }

    private async Task<Guid?> MatchAccepted(Guid userId, Guid idProfile, MatchModel matchExist)
    {
        var room = await repositoryRoom.Insert(new RoomModel { Id = Guid.NewGuid(), RoomType = RoomType.Private });
        var participants = new List<RoomParticipantModel>()
        {
            new() { IdRoom = room.Id, IdProfile = idProfile },
            new() { IdRoom = room.Id, IdProfile = userId},
        };
        repositoryParticipant.Insert(participants);

        matchExist.Status = MatchStatus.Accepted;
        matchExist.IdRoom = room.Id;
        await repositoryMatch.Update(matchExist);

        var roomCreated = "Match.Created";
        var message = new MessageModel
        {
            FromIdProfile = userId,
            ToIdRoom = room.Id,
            MediaType = Abstraction.Database.MediaType.Event,
            Content = Encoding.UTF8.GetBytes(roomCreated),
        };
        await hubContext.Clients.Group(room.Id.ToString()).SendAsync(roomCreated, message, HttpContext.RequestAborted);
        return await Task.FromResult(room.Id);
    }

    /// <summary>
    /// Get user matches
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IAsyncEnumerable<MatchModel>), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public async IAsyncEnumerable<MatchModel> Matches()
    {
        var userId = requestContext.Id ?? throw new UserNotConnectedException();
        var matches = repositoryMatch.Where(new Entity<Guid> { Id = userId });

        await foreach (var match in matches)
        {
            if (match.Status == MatchStatus.Accepted)
            {
                yield return mapper.Map<MatchModel>(match);
            }
        }
    }
}
