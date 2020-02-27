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

        [HttpGet("")]
        [ProducesResponseType(200, Type = typeof(List<User>))]
        [ProducesResponseType(404)]
        public IActionResult GetUsers()
        {
            var users=_userService.GetUsers();
            if(users==null) return NotFound();
            var userDtos=_mapper.Map<List<UserDto>>(users);
            return Ok(userDtos);
        } 
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]
        public IActionResult GetUserById(string id)
        {
            var userData=_userService.GetUserById(id);
            if(userData==null) return NotFound();
            return Ok(new {Id=userData.ID,Username=userData.Username ,
            Firstname=userData.FirstName,Lastname=userData.LastName});
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
                var Token = _tokeniser.CreateToken(user.ID.ToString(),u);

                return Ok(new { user.ID, user.Username
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
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult PutUser([FromBody] UserInfo userInfo)
        {
            var user = _mapper.Map<User>(userInfo);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if(!_userService.UpdateUser(user)) return NotFound();
            return Ok(new {Status="User updated"});
        }
    }
}