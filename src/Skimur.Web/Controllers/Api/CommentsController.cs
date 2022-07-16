using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skimur.Data.ReadModel;
using Skimur.Data.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Skimur.Web.Controllers.Api
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CommentsController : Controller
    {
        private readonly ICommentDao _commentDao;
        public CommentsController(ICommentDao commentDao)
        {
            _commentDao = commentDao;
        }

        [HttpGet]
        public IList<Comment> GetPostComments(Guid postId)
        {
            var comments = _commentDao.GetAllCommentsForPost(postId);
            return comments;
        }
    }
}
