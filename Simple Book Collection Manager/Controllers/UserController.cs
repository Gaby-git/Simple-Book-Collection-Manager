

using Domaine.Model;
using Domaine.Service;
using Microsoft.AspNetCore.Mvc;

namespace Simple_Book_Collection_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            var addUser = _userService.AddUser(user);

            return CreatedAtAction(nameof(GetUserById), new { userId = addUser.UserId, addUser });
        }

        [HttpGet("{UserId:int}")]
        public IActionResult GetUserById(int userId)
        { 
            var getById =  _userService.GetUserById(userId);

            return Ok(getById);
        }

        [HttpGet] 

        public IActionResult GetUser(User user)
        {
            var GetUser = _userService.GetUser(user);

            return Ok(GetUser);
        }
    }
}
