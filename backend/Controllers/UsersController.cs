using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using WebApi.Helpers;
using WebApi.Model;
using WebApi.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private IUserService _userService;
        private IMapper _mapper;
        private ITokeniser _tokeniser;

        public UsersController(IUserService userService, IMapper mapper, ITokeniser tokeniser)
        {
            _userService = userService;
            _mapper = mapper;
            _tokeniser = tokeniser;
        }

        [HttpGet("GetUsers")]
        [ProducesResponseType(200, Type = typeof(List<User>))]
        [ProducesResponseType(404)]
        public IActionResult GetUsers()
        {
            var users=_userService.GetUsers();
            if(users==null) return NotFound();
            return Ok(users);
        } 
        [HttpGet("GetUserById/{id}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]
        public IActionResult GetUserById(int id)
        {
            var userData=_userService.GetUserById(id);
            if(userData==null) return NotFound();
            return Ok(new {Id=userData.Id,Username=userData.Username ,Firstname=userData.FirstName,Lastname=userData.LastName});
        }        
        [HttpPost("authenticate")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Authenticate([FromBody]UserDto userDto)
        {
            try
            {
                var user = _userService.Authenticate(userDto.Username, userDto.Password);
                var claims=this.User.Claims;
                if (user == null)
                    return BadRequest("Username or password is incorrect");
                var u=userDto.Username;
                var Token = _tokeniser.CreateToken(user.Id.ToString(),u);

                return Ok(new { user.Id, user.Username
                ,user.Roles
                , user.FirstName, user.LastName, Token });
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);//shout/catch/throw/log
            }            
        }
    
        [HttpPost("register")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult Register([FromBody]UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            try
            {
                _userService.Create(user, userDto.Password);
                return Ok(new {User=userDto.Username});
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);//shout/catch/throw/log
            }
        }
        [HttpPut("update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Update(User user)
        {
            return Ok(_userService.UpdateUser(user));
        }
    }
}