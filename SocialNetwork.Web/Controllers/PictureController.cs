using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.DataModel.Models;
using SocialNetwork.Services.Contracts;

namespace SocialNetwork.Web.Controllers
{
    [Route("api/Image")]
    public class PictureController : Controller
    {
        private readonly IPictureService _pictureService;

        public PictureController(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Picture image = await _pictureService.ByIdAsync(id);

            byte[] imageInBinary = image.ImageData;

            return File(imageInBinary, "image/png");
        }
    }
}