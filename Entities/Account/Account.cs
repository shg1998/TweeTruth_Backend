using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Account
{
    public class Account : BaseEntity
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
        public User.User User { get; set; }
        public ICollection<Tweet.Tweet> Tweets { get; set; }
    }

    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasOne(p => p.User).WithMany(c => c.Accounts).HasForeignKey(p => p.AuthorId);
            builder.HasMany(p => p.Tweets).WithOne(c => c.Account).HasForeignKey(p => p.AccountId);
        }
    }
}
