using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DirectFocusCodingAssignment.Data;
using Microsoft.Extensions.Logging;
using AutoMapper;
using DirectFocusCodingAssignment.Data.Entities;
using DirectFocusCodingAssignment.Models;

namespace DirectFocusCodingAssignment.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserRepository userRepository,IMapper mapper, ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
        }

        
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var results = _userRepository.GetUsers();
                if(results != null && results.Count != 0)
                {
                  return Ok(_mapper.Map<List<User>,List<UserViewModel>>(results));
                }
                else
                {
                    return NotFound();
                }
               
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get users: {ex}");
                return BadRequest("Failed to get users");
            }

            
        }

        
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var result = _userRepository.GetUserByID(id);
                if (result != null) return Ok(_mapper.Map<User, UserViewModel>(result));
                else return NotFound();
               
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get user: {ex}");
                return BadRequest("Failed to get user");
            }
        }

        
        [HttpPost]
        public IActionResult Post([FromBody]UserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newUser = _mapper.Map<UserViewModel, User>(model);
                    _userRepository.CreateUser(newUser);
                    if (_userRepository.SaveAll())
                    {
                        return Created($"api/Users/{newUser.Id}", _mapper.Map<User, UserViewModel>(newUser));
                    }
                    else
                    {
                        return BadRequest("Failed to save user");
                    }

                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save user: {ex}");
                return BadRequest("Failed to save user");
            }

        }

        
        [HttpPut("{id:int}")]
        public IActionResult Put([FromBody]UserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentUser = _mapper.Map<UserViewModel, User>(model);
                    _userRepository.UpdateUser(currentUser);
                    if (_userRepository.SaveAll())
                    {
                        return Ok( _mapper.Map<User, UserViewModel>(currentUser));
                    }
                    else
                    {
                        return BadRequest("Failed to update user");
                    }

                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update user: {ex}");
                return BadRequest("Failed to update user");
            }

        }

        
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                 _userRepository.DeleteUser(id);

                if (_userRepository.SaveAll())
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Failed to delete user");
                }                
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete user: {ex}");
                return BadRequest("Failed to delete user");
            }
        }
    }
}
