using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ServiceStack.OrmLite;
using Skimur.Data.ReadModel;
using Skimur.Data.Models;
using Skimur.Backend.Sql;
using Skimur.Common.Utils;

namespace Skimur.Data.Services.Impl
{
    public class PostService : IPostService
    {
        private readonly IDbConnectionProvider _conn;

        public PostService(IDbConnectionProvider conn)
        {
            _conn = conn;
        }

        public void InsertPost(Post post)
        {
            _conn.Perform(conn => conn.Insert(post));
        }

        public void UpdatePost(Post post)
        {
            _conn.Perform(conn => conn.Update(post));
        }

        public Post GetPostById(Guid id)
        {
            return _conn.Perform(conn => conn.SingleById<Post>(id));
        }

        public SeekedList<Guid> GetPosts(List<Guid> subs = null,
            PostsSortBy sortby = PostsSortBy.New,
            PostsTimeFilter timeFilter = PostsTimeFilter.All,
            Guid? userId = null,
            bool hideRemovedPosts = true,
            bool showDeleted = false,
            bool onlyAll = false,
            bool? nsfw = null,
            bool? sticky = null,
            bool stickyFirst = false,
            int? skip = null,
            int? take = null)
        {
            return _conn.Perform(conn =>
            {
                var query = conn.From<Post>();
                if (subs != null && subs.Count > 0)
                {
                    query.Where(x => subs.Contains(x.SubId));
                }

                if (timeFilter != PostsTimeFilter.All)
                {
                    TimeSpan timeSpan;

                    switch (timeFilter)
                    {
                        case PostsTimeFilter.Hour:
                            timeSpan = TimeSpan.FromHours(1);
                            break;
                        case PostsTimeFilter.Day:
                            timeSpan = TimeSpan.FromDays(1);
                            break;
                        case PostsTimeFilter.Week:
                            timeSpan = TimeSpan.FromDays(7);
                            break;
                        case PostsTimeFilter.Month:
                            timeSpan = TimeSpan.FromDays(30);
                            break;
                        case PostsTimeFilter.Year:
                            timeSpan = TimeSpan.FromDays(365);
                            break;
                        default:
                            throw new Exception("unknown time filter");
                    }

                    var timeDiff = TimeHelper.CurrentTime() - timeSpan;
                    query.Where(x => x.CreatedAt >= timeDiff);
                }

                if (hideRemovedPosts)
                {
                    query.Where(x => x.Verdict != (int)Verdict.ModRemoved);
                }

                if (!showDeleted)
                {
                    query.Where(x => x.Deleted == false);
                }

                if (onlyAll)
                {
                    query.Where(x => x.InAll);
                }

                if (nsfw.HasValue)
                {
                    query.Where(x => x.Nsfw == nsfw.Value);
                }

                if (userId.HasValue)
                {
                    query.Where(x => x.UserId == userId.Value);
                }

                if (sticky.HasValue)
                {
                    query.Where(x => x.Sticky == sticky.Value);
                }

                var totalCount = conn.Count(query);

                query.Skip(skip).Take(take);

                var orders = new List<string>();

                if (stickyFirst)
                {
                    orders.Add("sticky");
                }

                switch (sortby)
                {
                    case PostsSortBy.Hot:
                        orders.Add("host(vote_up_count, vote_down_count, created_at)");
                        orders.Add("created_at");
                        break;
                    case PostsSortBy.New:
                        orders.Add("created_at");
                        break;
                    case PostsSortBy.Rising:
                        throw new Exception("not implemented");
                    case PostsSortBy.Controversial:
                        orders.Add("controversy(vote_up_count, vote_down_count)");
                        orders.Add("created_at");
                        break;
                    case PostsSortBy.Top:
                        orders.Add("score(vote_up_count, vote_down_count)");
                        orders.Add("create_at");
                        query.OrderByExpression = "ORDER BY (, created_at) DESC";
                        break;
                    default:
                        throw new Exception("unkonw sort");
                }

                if (orders.Count > 0)
                {
                    query.OrderByExpression = "ORDER BY (" + string.Join(", ", orders) + ") DESC";
                }

                query.SelectExpression = "SELECT \"id\"";

                return new SeekedList<Guid>(conn.Select(query).Select(x => x.Id), skip ?? 0, take, totalCount);
            });
        }


    }
}
