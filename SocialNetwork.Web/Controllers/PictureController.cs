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
    [Produces("application/json")]
    [Route("api/Image")]
    public class PictureController : Controller
    {
        private readonly IPictureService _pictureService;

        public PictureController(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }

        public async Task<IActionResult> Get(int id)
        {
            Picture image = await _pictureService.ById(id);

            byte[] imageInBinary = image.ImageData;

            return File(imageInBinary, "image/png");
        }
    }
}