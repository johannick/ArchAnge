using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Abstraction;
using Abstraction.Database;
using Abstraction.Repository;
using Model.Contact;
using Model.Interest;
using Model.Profile;
using System.Data;
using System.Net.Mime;
using Microsoft.VisualBasic;
using ArchAnge.ServiceDefaults.Endpoint;
using ArchAnge.ServiceDefaults.Exception;
using ArchAnge.ServiceDefaults.Context;
using Model.User;

namespace ApiService.Controllers;

/// <summary>
/// User Profile controller
/// </summary>
/// <param name="requestContext"></param>
/// <param name="repositoryProfile"> <see cref="IRepository{Profile}"/> </param>
/// <param name="repositoryPicture"> <see cref="IRepository{ProfilePicture}"/> </param>
/// <param name="repositoryAddress"> </param>
/// <param name="repositoryInterest"> </param>
/// <param name="repositoryPhone"> </param>
/// <param name="repositoryLocation"> </param>
/// <param name="mapper"> Automapper <see cref="IMapper"/> </param>
public class ProfileController
    (ConnectionRequestContext requestContext
    , IRepository<ProfileModel> repositoryProfile
    , IRepository<ProfilePictureModel> repositoryPicture
    , IRepository<ProfileAddressModel> repositoryAddress
    , IRepository<ProfileInterestModel> repositoryInterest
    , IRepository<ProfilePhoneModel> repositoryPhone
    , IRepository<LocationModel> repositoryLocation)
    : ApiControllerBase
{
    /// <summary>
    /// Get profile model
    /// </summary>
    /// <returns> <see cref="ProfileModel"/> </returns>
    [HttpGet(Abstraction.Constants.Empty)]
    [ProducesResponseType(typeof(ProfileModel), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Get()
    {
        var entity = new Entity<Guid> { Id = requestContext.Id ?? throw new UserNotConnectedException() };
        await using var enumerator = repositoryProfile.Where(entity, nameof(entity.Id)).GetAsyncEnumerator(HttpContext.RequestAborted);

        if (!await enumerator.MoveNextAsync())
        {
            return await Task.FromResult(new StatusCodeResult(StatusCodes.Status404NotFound));
        }

        var profile = enumerator.Current;

        var addresses = await repositoryAddress.Where(profile, nameof(ProfileAddressModel.IdProfile)).ToListAsync(HttpContext.RequestAborted);
        var locations = addresses.SelectMany(address => repositoryLocation.Where(new Entity<Guid> { Id = address.IdLocation }, nameof(LocationModel.Id)).ToBlockingEnumerable());
        var phones = await repositoryPhone.Where(entity, nameof(entity.Id)).ToListAsync(HttpContext.RequestAborted);
        var pictures = await repositoryPicture.Where(new ProfilePictureModel(profile, string.Empty), nameof(ProfilePictureModel.IdProfile)).ToListAsync(HttpContext.RequestAborted);
        var interests = await repositoryInterest.Where(profile).ToListAsync(HttpContext.RequestAborted);

        profile.Addresses = new ProfileLocationModel(locations);
        profile.Phones = phones.First();
        profile.Pictures = [.. pictures.Select(p => p.Name)];
        profile.Interests = interests.First();

        return await Task.FromResult(new JsonResult(profile));
    }

    /// <summary>
    /// Get Profile by id
    /// </summary>
    /// <param name="profileId"> ProfileId, which is the same as userId</param>
    /// <returns> <see cref="ProfileModel"/></returns>
    [HttpGet]
    [ProducesResponseType(typeof(ProfileModel), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Details(Guid profileId)
    {
        await using var enumerator = repositoryProfile.Where(new Entity<Guid> { Id = profileId }).GetAsyncEnumerator(HttpContext.RequestAborted);

        if (!await enumerator.MoveNextAsync())
        {
            return await Task.FromResult(new StatusCodeResult(StatusCodes.Status404NotFound));
        }
        return await Task.FromResult(new JsonResult(enumerator.Current));
    }

    /// <summary>
    /// Upload profile pictures
    /// </summary>
    /// <param name="isAvatar"></param>
    /// <param name="files"></param>
    /// <remarks> TODO Check file size, remove exif information </remarks>
    /// <returns> True if image was successfully download </returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ProducesResponseType(typeof(IEnumerable<Tuple<bool, string>>), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public async Task<IEnumerable<Tuple<bool, string>>> Upload(bool isAvatar, List<IFormFile> files)
    {
        var pictures = await repositoryPicture.Where().ToListAsync(HttpContext.RequestAborted);
        var limit = 10;
        var filesToAdd = files.Take(limit - pictures.Count).ToList();
        var result = new List<Tuple<bool, string>>();
        var profile = new Entity<Guid> { Id = requestContext.Id ?? throw new UserNotConnectedException() };

        isAvatar = isAvatar || (pictures.Count == 0);
        foreach (var file in filesToAdd)
        {
            result.Add(await AddFile(file, isAvatar, profile));
            isAvatar = false;
        }
        return await Task.FromResult(result);
    }

    private async Task<Tuple<bool, string>> AddFile(IFormFile file, bool isAvatar, Entity<Guid> profile)
    {
        var path = Path.Combine(requestContext.ImageFolder(), file.FileName);
        using var stream = new FileStream(path, FileMode.Create);

        await file.CopyToAsync(stream).ConfigureAwait(false);
        await repositoryPicture.Insert(new ProfilePictureModel(profile, file.FileName)).ConfigureAwait(false);

        if (isAvatar)
        {
            await using var enumerator = repositoryProfile.Where(profile, nameof(profile.Id)).GetAsyncEnumerator(HttpContext.RequestAborted);

            if (!await enumerator.MoveNextAsync().ConfigureAwait(false))
            {
                return await Task.FromResult(Tuple.Create(false, file.FileName));
            }
            enumerator.Current.Avatar = file.FileName;
            await repositoryProfile.Update(enumerator.Current);
        }
        return await Task.FromResult(Tuple.Create(true, file.FileName));
    }

    /// <summary>
    /// Delete picture
    /// </summary>
    /// <param name="deleteModel"></param>
    /// <returns> A list of deleted pictures, true if it's deleted </returns>
    [HttpDelete]
    [ProducesResponseType(typeof(IEnumerable<Tuple<string, bool>>), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public async Task Pictures([FromBody] ProfileDeletePictureModel deleteModel)
    {
        var token = HttpContext.RequestAborted;
        var userId = requestContext.Id ?? throw new UserNotConnectedException();
        var profiles = await repositoryProfile.Where(new Entity<Guid> { Id = userId }, nameof(UserModel.Id)).ToListAsync(token);
        var profile = profiles.First();

        token.ThrowIfCancellationRequested();

        await DeleteProfilePictures(profile, deleteModel);

        if (deleteModel.Pictures.Any(item => profile.Avatar.Equals(item.Item1 + item.Item2)))
        {
            profile.Avatar = string.Empty;
            await repositoryProfile.Update(profile);
        }
    }

    /// <summary>
    /// Delete files in user folder
    /// </summary>
    /// <param name="profile"> Profile </param>
    /// <param name="model"> fileNames from user input </param>
    /// <returns> A list of deleted pictures, true if it's deleted</returns>
    private async Task DeleteProfilePictures(ProfileModel profile, ProfileDeletePictureModel model)
    {
        var userFolder = requestContext.ImageFolder();

        foreach (var picture in model.Pictures)
        {
            var filename = picture.Item1 + picture.Item2;

            await repositoryPicture.Delete(new ProfilePictureModel(profile, filename));
            await Task.Run(() => System.IO.File.Delete(Path.Combine(userFolder, filename)));
        }
    }
}
