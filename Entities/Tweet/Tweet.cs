using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Tweet
{
    public class Tweet : BaseEntity
    {
        public string TweetDate { get; set; }
        public string TweetUrl { get; set; }
        public string UserName { get; set; }
        public string TweetContent { get; set; }
        public int ReplyCount { get; set; }
        public int ReTweetCount { get; set; }
        public int LikeCount { get; set; }

        public int AccountId { get; set; }
        public Account.Account Account { get; set; }
    }

    public class TweetConfiguration : IEntityTypeConfiguration<Tweet>
    {
        public void Configure(EntityTypeBuilder<Tweet> builder)
        {
            builder.HasOne(p => p.Account).WithMany(c => c.Tweets).HasForeignKey(p => p.AccountId);
        }
    }
}
