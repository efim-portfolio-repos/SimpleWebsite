using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SimpleWebsite.Models;
using SimpleWebsite.Models.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace SimpleWebsite.Infrastructure.TagHelpers
{
    [HtmlTargetElement("select", Attributes = "model-for")]
    public class SelectOptionTagHelper : TagHelper
    {
        private IServiceProvider _serviceProvider;

        public SelectOptionTagHelper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ModelExpression ModelFor { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.Content.AppendHtml(
                (await output.GetChildContentAsync(false)).GetContent()
            );

            if (ModelFor.ModelExplorer.ModelType == typeof(Category))
            {
                Category category = ModelFor.Model as Category;
                IRepository<Category> categories = _serviceProvider.GetRequiredService<IRepository<Category>>();
                foreach (Category c in categories.Entities.OrderBy(c => c.Name))
                {
                    if (category != null && category.Id == c.Id)
                    {
                        output.Content.AppendHtml($"<option value='{c.Id}' selected='true'>{WebUtility.HtmlEncode(c.Name)}</option>");
                    }
                    else
                    {
                        output.Content.AppendHtml($"<option value='{c.Id}'>{WebUtility.HtmlEncode(c.Name)}</option>");
                    }
                }
            }


            output.Attributes.SetAttribute("Name", ModelFor.Name + "Id");
            output.Attributes.SetAttribute("Id", ModelFor.Name + "Id");
        }
    }
}