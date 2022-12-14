using System;
using be_abisoft.Models;
using be_abisoft.Services;
using Microsoft.AspNetCore.Mvc;

namespace be_abisoft.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: ControllerBase
	{
        private IConfiguration _config;

        public UserController(IConfiguration config)
        {
            _config = config;
        }


        [HttpPost("ins_upd_user")]
        public async Task<IActionResult> ins_upd_user([FromBody] RequestUser_CU request)
        {
            FormResponseModel retorno = new FormResponseModel();
            try
            {
                FormResponseModel response = await UserService.Ins_upd_user(_config, request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                return Ok(m);
            }
        }

        [HttpPost("list_user")]
        public async Task<IActionResult> sel_user([FromBody] RequestUser request)
        {
            FormResponseModel retorno = new FormResponseModel();
            try
            {
                FormResponseModel response = await UserService.Sel_user(_config, request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                return Ok(m);
            }
        }
    }
}

