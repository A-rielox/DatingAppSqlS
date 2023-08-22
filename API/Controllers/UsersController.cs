using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;


[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;

    public UsersController( IUserRepository userRepository,
                            IMapper mapper,
                            IPhotoService photoService )
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _photoService = photoService;
    }



    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    // GET /api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await _userRepository.GetUsersAsync();
        var members = _mapper.Map<IEnumerable<MemberDto>>(users);

        return Ok(members);
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    // GET api/Users/bob
    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {// està usando getMember, lo mismo pero mapea el query con AutoMapper
        var user = await _userRepository.GetUserByUsernameAsync(username);
        var member = _mapper.Map<MemberDto>(user);

        return Ok(member);
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    // PUT api/Users
    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        //var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var username = User.GetUsername();

        var user = await _userRepository.GetUserByUsernameAsync(username);

        // lo q esta em memberUpdateDto lo mete a user
        //                |---------->
        _mapper.Map(memberUpdateDto, user);

        // aùn y si no hay cambios me sobreescribe todo
        if (await _userRepository.UpdateAsync(user)) return NoContent();

        return BadRequest("Failed to update user.");
    }


    ////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    // POST: api/Users/add-photo
    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        // al probar en postman la "key" q mando en el body se debe llamar como le pongo aca
        // el paramatro " File "

        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null) return NotFound();

        var result = await _photoService.AddPhotoAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
            AppUserId = user.Id
        };

        // si es su primera foto => la pongo como mail
        // como estoy checando "user.Photos.Count" tengo que cargar las fotos con "GetUserByUsernameAsync"
        if (user.Photos.Count == 0)
        {
            photo.IsMain = 1;
        }

        var photoId = await _userRepository.AddPhotoAsync(photo);

        if(photoId > 0) return _mapper.Map<PhotoDto>(photo);

        return BadRequest("Problem adding the photo.");

        //user.Photos.Add(photo);

        //if (await _userRepository.SaveAllAsync())
        //{//                        <-------
        //    return _mapper.Map<PhotoDto>(photo);
        //    // el " new {} " es xq la ruta GetUser ocupa ese parametro para ir al user
        //    // el tercer parametro es el object que se creó
        //    //return CreatedAtAction(nameof(GetUser),
        //    //    new { username = user.UserName }, _mapper.Map<PhotoDto>(photo));
        //}

        //return BadRequest("Problem adding photo.");
    }

    ////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    // PUT: api/Users/set-main-photo/{photoId}
    //[HttpPut("set-main-photo/{photoId}")]
    //public async Task<ActionResult> SetMainPhoto(int photoId)
    //{
    //    var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

    //    if (user == null) return NotFound();

    //    var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

    //    if (photo == null) return NotFound();

    //    if (photo.IsMain) return BadRequest("This is already your main photo.");

    //    var currentMain = user.Photos.FirstOrDefault(p => p.IsMain);

    //    if (currentMain != null) currentMain.IsMain = false;

    //    photo.IsMain = true;

    //    if (await _userRepository.SaveAllAsync()) return NoContent();

    //    return BadRequest("Problem setting the main photo");
    //}

    ////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    // DELETE: api/Users/delete-photo/{photoId}
    //[HttpDelete("delete-photo/{photoId}")]
    //public async Task<ActionResult> DeletePhoto(int photoId)
    //{
    //    // saco el usernane del token
    //    var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

    //    var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

    //    if (photo == null) return NotFound();

    //    if (photo.IsMain) return BadRequest("You can not delete your main photo.");

    //    // si esta en cloudinary => tiene una publicId
    //    if (photo.PublicId != null)
    //    {
    //        var result = await _photoService.DeletePhotoAsync(photo.PublicId);
    //        if (result.Error != null) return BadRequest(result.Error.Message);
    //    }

    //    user.Photos.Remove(photo);

    //    if (await _userRepository.SaveAllAsync()) return Ok();

    //    return BadRequest("Failed to delete the photo.");
    //}
}
