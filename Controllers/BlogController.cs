using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SimpleWebsite.Models;
using SimpleWebsite.Models.ViewModels;
using SimpleWebsite.Models.Repositories;

namespace SimpleWebsite.Controllers
{
    public class BlogController : Controller
    {
        private IRepository<Article> _articles;
        private IRepository<Comment> _comments;
        private UserManager<User> _userManager;

        public BlogController(IRepository<Article> articles, IRepository<Comment> comments, UserManager<User> userManager)
        {
            _articles = articles;
            _comments = comments;
            _userManager = userManager;
        }

        public ViewResult List(int page = 1)
        {
            int elementsOnPage = 3;
            return View(new BlogListViewModel {
                ElementOnPage = elementsOnPage,
                CurrentPage = page,
                TotalElements = _articles.Entities.Count(),
                Articles = _articles.Entities
                            .Include(a => a.Comments)
                            .OrderByDescending(a => a.PublishDate)
                            .Skip((page - 1) * elementsOnPage)
                            .Take(elementsOnPage)
            });
        }

        public IActionResult Article(int id)
        {
            Article article = _articles.Entities.Include(a => a.Comments).ThenInclude(c => c.User).FirstOrDefault(a => a.Id == id);
            if (article != null)
            {
                return View(article);
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddComment([FromForm]int articleId, [FromForm]string text)
        {
            if (text != null && text.Length > 0)
            {
                Article article = _articles[articleId];
                if (article != null)
                {
                    _comments.Add(new Comment
                    {
                        UserId = _userManager.GetUserId(User),
                        ArticleId = articleId,
                        Text = text,
                        PublishTime = DateTime.Now
                    });
                }
            }

            return RedirectToAction(nameof(Article), new
            {
                id = articleId
            });
        }

        [Authorize(Roles = "Bloggers")]
        public ViewResult New() => View();

        [HttpPost]
        [Authorize(Roles = "Bloggers")]
        public IActionResult New(Article article)
        {
            if (ModelState.IsValid)
            {
                _articles.Add(new Article
                {
                    Header = article.Header,
                    PublishDate = DateTime.Now,
                    Text = article.Text
                });
                return RedirectToAction(nameof(List));
            }

            return View(article);
        }

        [Authorize(Roles = "Bloggers")]
        public IActionResult Edit(int id)
        {
            Article article = _articles.Entities.FirstOrDefault(a => a.Id == id);
            if (article != null)
            {
                return View(article);
            }

            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        [Authorize(Roles = "Bloggers")]
        public IActionResult Edit(Article article)
        {
            if (ModelState.IsValid)
            {
                Article editableArticle = _articles.Entities.FirstOrDefault(a => a.Id == article.Id);
                if (editableArticle != null)
                {
                    editableArticle.Header = article.Header;
                    editableArticle.Text = article.Text;
                    _articles.Update(editableArticle);
                }
                return RedirectToAction(nameof(List));
            }


            return View(article);
        }


        [HttpPost]
        [Authorize(Roles = "Bloggers")]
        public IActionResult Delete(int id)
        {
            _articles.Delete(id);
            return RedirectToAction(nameof(List));
        }
    }
}