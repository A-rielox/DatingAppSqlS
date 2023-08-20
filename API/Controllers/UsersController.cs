using API.DTOs;
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

    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }



    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    // /api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await _userRepository.GetUsersAsync();
        var members = _mapper.Map<IEnumerable<MemberDto>>(users);

        return Ok(members);
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    // api/Users/bob
    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {// està usando getMember, lo mismo pero mapea el query con AutoMapper
        var user = await _userRepository.GetUserByUsernameAsync(username);
        var member = _mapper.Map<MemberDto>(user);

        return Ok(member);
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    // api/Users
    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _userRepository.GetUserByUsernameAsync(username);

        // lo q esta em memberUpdateDto lo mete a user
        //                |---------->
        _mapper.Map(memberUpdateDto, user);

        // aùn y si no hay cambios me sobreescribe todo
        if (await _userRepository.UpdateAsync(user)) return NoContent();

        return BadRequest("Failed to update user.");
    }
}
