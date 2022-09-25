using System.Collections.Generic;
using Entities.Account;
using Entities.User;
using Service.WebFramework.Api;
using System.ComponentModel.DataAnnotations;

namespace Service.DTOs
{
    public class AccountDto : BaseDto<AccountDto, Account>
    {
        [Required]
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string RawDescription { get; set; }
        public string DescriptionUrl { get; set; }
        public string CreatedTime { get; set; }
        public int FollowersCount { get; set; }
        public int FriendsCount { get; set; }
        public int StatusesCount { get; set; }
        public string Location { get; set; }
        public string LinkUrl { get; set; }
        public string ProfileImageUrl { get; set; }
        public string ProfileBannerUrl { get; set; }
        public double TweetsCredibilityScore { get; set; }
        public double UserReputationScore { get; set; }
        public double ReTweetHIndexScore { get; set; }
        public double LikeHIndexScore { get; set; }
        public double InfluenceScore { get; set; }
        public int AuthorId { get; set; }
        public User User { get; set; }
        public ICollection<Entities.Tweet.Tweet> Tweets { get; set; }
    }

    public class CreateAccountDto : BaseDtoComplexKey<CreateAccountDto, Account>
    {
        [Required]
        public string Username { get; set; }
    }

    public class EditAccountDto : BaseDtoComplexKey<EditAccountDto, Account>
    {
        [Required]
        public string Username { get; set; }
    }

    public class AbstractAccountDto : BaseDto<AbstractAccountDto, Account>
    {
        public string Username { get; set; }
    }
}
