using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entities.Post;
using Service.WebFramework.Api;

namespace Service.DTOs
{
    public class PostDto : BaseDto<PostDto, Post, Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }

        public string CategoryName { get; set; } //=> Category.Name
        public string AuthorFullName { get; set; } //=> Author.FullName

        public string FullTitle { get; set; } // => mapped from "Title (Category.Name)"

        //[IgnoreMap]
        //public string Category { get; set; }

        public override void CustomMappings(IMappingExpression<Post, PostDto> mappingExpression)
        {
            mappingExpression.ForMember(
                dest => dest.FullTitle,
                config => config.MapFrom(src => $"{src.Title} ({src.Category.Name})"));
        }
    }
}
