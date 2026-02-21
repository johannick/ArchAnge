using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Abstraction.Repository;
using Model.Contact;
using Model.Interest;
using System.Net.Mime;
using Model.Profile;
using ArchAnge.ServiceDefaults.Endpoint;

namespace ApiService.Controllers;

/// <summary>
/// Contact controller
/// </summary>
/// <param name="repositoryCountry"></param>
/// <param name="repositoryInterest"></param>
/// <param name="repositoryLocation"></param>
/// <param name="repositoryPictures"></param>
/// <param name="repositoryProfileInterest"></param>
/// <param name="repositoryPhones"></param>
/// <param name="mapper"></param>
public class ContactController
    (IRepository<CountryModel> repositoryCountry
    , IRepository<InterestModel> repositoryInterest
    , IRepository<LocationModel> repositoryLocation
    , IRepository<ProfilePictureModel> repositoryPictures
    , IRepository<ProfileInterestModel> repositoryProfileInterest
    , IRepository<ProfilePhoneModel> repositoryPhones
    , IMapper mapper)
    : ApiControllerBase
{
    /// <summary>
    /// Get All Countries
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(IAsyncEnumerable<CountryModel>), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public async IAsyncEnumerable<CountryModel> Countries()
    {
        var result = repositoryCountry.GetAll();

        await foreach (var item in result)
        {
            yield return mapper.Map<CountryModel>(item);
        }
    }

    /// <summary>
    /// User pictures
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IAsyncEnumerable<string>), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public async IAsyncEnumerable<string> Pictures()
    {
        var pictures = repositoryPictures.Where();

        await foreach (var picture in pictures)
        {
            yield return picture.Name;
        }
    }

    /// <summary>
    /// Get All Interests
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IAsyncEnumerable<InterestModel>), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public async IAsyncEnumerable<InterestModel> Interests()
    {
        var result = repositoryInterest.GetAll();

        await foreach (var item in result)
        {
            yield return mapper.Map<InterestModel>(item);
        }
    }

    /// <summary>
    /// Get User interests
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IAsyncEnumerable<InterestModel>), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public async IAsyncEnumerable<InterestModel> Interest()
    {
        var userInterests = repositoryProfileInterest.Where();

        await foreach (var userInterest in userInterests)
        {
            yield return mapper.Map<InterestModel>(userInterest);
        }
    }

    /// <summary>
    /// Get User addresses
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IAsyncEnumerable<LocationModel>), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public async IAsyncEnumerable<LocationModel> Addresses()
    {
        var locations = repositoryLocation.Where();

        await foreach (var item in locations)
        {
            yield return mapper.Map<LocationModel>(item);
        }
    }

    /// <summary>
    /// Get user phones
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IAsyncEnumerable<PhoneNumberModel>), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public async IAsyncEnumerable<PhoneNumberModel> Phones()
    {
        var result = repositoryPhones.Where();

        await foreach (var item in result)
        {
            yield return mapper.Map<PhoneNumberModel>(item);
        }
    }
}
