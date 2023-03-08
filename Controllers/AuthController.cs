using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using newplgapi.model;
using newplgapi.model.Dto;
using newplgapi.Repository.Implements;
using newplgapi.Repository.Interfaces;

namespace newplgapi.Controllers
{
    [Route("apimyrsup/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IDapperContext _context;
        private IHttpContextAccessor _httpContext;
        private IUnitOfWork _uow;
        private IConfiguration _config;

        public AuthController( IConfiguration config){
            _config = config;
            _httpContext = new HttpContextAccessor();
        }
        
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            // var userFromRepo = await _uow.AuthRepository.Login(userForLoginDto.userId.ToLower(), userForLoginDto.password);
            User userFromRepo = new User();
            using(_context = new DapperContext("RSUP")){
                _uow = new UnitOfWork(_context);
                userFromRepo = await _uow.AuthRepository.Login(userForLoginDto.userId.ToLower(), userForLoginDto.password);
            }

            if (userFromRepo == null)
                return Unauthorized();
            
            return Ok(new
            {
                token = GenerateJwtToken(userFromRepo),
                user = userFromRepo
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.userId = userForRegisterDto.userId.ToLower();
             User createdUser = new User();
            try
            {
                using(_context = new DapperContext("RSUP")){
                    _uow = new UnitOfWork(_context);
                    bool flag = await this._uow.AuthRepository.UserExists(userForRegisterDto.userId);
                    if (flag)
                        return (IActionResult)this.BadRequest((object)new
                        {
                            statusResult = "Warning",
                            messageResult = "Username already exists"
                        });
                        
                    User userToCreate = new User()
                    {
                        firstName = userForRegisterDto.firstName,
                        lastName = userForRegisterDto.lastName,
                        roles = userForRegisterDto.roles,
                        userId = userForRegisterDto.userId,
                        Dept = userForRegisterDto.Dept,
                        FacAbbr = userForRegisterDto.Factory,
                        GroupAccess = userForRegisterDto.GroupAcces,
                        nohp = userForRegisterDto.NoHp
                    };
                    
                    createdUser = await this._uow.AuthRepository.Register(userToCreate, userForRegisterDto.password);
                }
               
                return (IActionResult)this.Ok((object)new
                {
                    statusResult = "Success",
                    messageResult = "Register Account Success",
                    uid = createdUser.userId
                });
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return (IActionResult)this.BadRequest((object)new
                {
                    statusResult = "Failed",
                    messageResult = err
                });
            }
        }

        private string GenerateJwtToken(User user)
        {
            string namalengkap = user.firstName + " " + user.lastName;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.userId),
                new Claim(ClaimTypes.Name, namalengkap),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.PrimaryGroupSid, user.GroupAccess),
                new Claim(ClaimTypes.GroupSid, user.FacAbbr),
                new Claim(ClaimTypes.Country, user.Dept)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddYears(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}