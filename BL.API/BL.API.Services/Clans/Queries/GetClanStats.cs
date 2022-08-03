using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Clan;
using BL.API.Core.Domain.Match;
using BL.API.Core.Exceptions;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Clans.Queries
{
    public static class GetClanStats
    {
        public record Query(Guid ClanId) : IRequest<ClanStatsItem>;

        public class GetClanStatsHandler : IRequestHandler<Query, ClanStatsItem>
        {
            private readonly IRepository<Clan> _clans;
            private readonly IRepository<Match> _matches;

            public GetClanStatsHandler(IRepository<Clan> clans,
                IRepository<Match> matches)
            {
                _clans = clans;
                _matches = matches;
            }

            public async Task<ClanStatsItem> Handle(Query request, CancellationToken cancellationToken)
            {
                var clan = await _clans.GetByIdAsync(request.ClanId);

                if (clan == null)
                {
                    throw new NotFoundException();
                }

                var clanRoster = clan.ClanMembers
                    .OrderBy(cm => cm.MemberType)
                    .Select(cm => new ClanMemberStatsItem
                    {
                        Nickname = cm.Player.Nickname,
                        Role = cm.MemberType.ToString(),
                        MMR = cm.Player.GetPlayerMMR(clan.Region.ShortName).MMR
                    })
                    .ToArray();

                var clanStats = new ClanStatsItem
                {
                    Name = clan.Name,
                    Description = clan.Description,
                    Color = clan.Color,
                    AvatarURL = clan.AvatarURL,
                    LeaderNickname = clan.ClanMembers?.FirstOrDefault(cr => cr.MemberType == ClanMemberType.Leader)?.Player?.Nickname,
                    AvgMMR = clanRoster.Select(c => c.MMR).Average(),
                    Roster = clanRoster
                };

                return clanStats;
            }
        }
    }
}
