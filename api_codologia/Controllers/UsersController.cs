using api_codologia.Models;
using Microsoft.AspNetCore.Mvc;

namespace api_codologia.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private static readonly List<User> Users = [];
    private static readonly object UsersLock = new();
    private static int nextId;

    [HttpGet]
    public ActionResult<IEnumerable<User>> GetAll()
    {
        lock (UsersLock)
        {
            return Ok(Users.ToList());
        }
    }

    [HttpGet("{id:int}")]
    public ActionResult<User> GetById(int id)
    {
        lock (UsersLock)
        {
            var user = Users.FirstOrDefault(user => user.Id == id);
            return user is null ? NotFound() : Ok(user);
        }
    }

    [HttpPost]
    public ActionResult<User> Create(User user)
    {
        lock (UsersLock)
        {
            user.Id = ++nextId;
            Users.Add(user);
        }

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }
}
