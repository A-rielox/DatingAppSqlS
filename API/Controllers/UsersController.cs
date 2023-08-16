using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
}
