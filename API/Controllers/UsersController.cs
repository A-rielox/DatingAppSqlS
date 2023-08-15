using API.Entities;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API.Controllers;

public class UsersController : BaseApiController
{
    private IDbConnection db;

    public UsersController(IConfiguration configuration)
    {
        this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }



    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    // /api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await db.QueryAsync<AppUser>("sp_getAllUsers",
                                  commandType: CommandType.StoredProcedure);

        return Ok(users);
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    // api/Users/3
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var user = await db.QueryAsync<AppUser>("sp_getUserById",
                                    new { userId = id },
                                    commandType: CommandType.StoredProcedure);

        return Ok(user.SingleOrDefault());
    }
}
