using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Interaces;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
            //return await _context.Users
            //    .Where(x => x.UserName == username)
            //    .Select(user => new MemberDto
            //    {
            //        UserName = user.UserName,
            //        Id = user.Id,
            //        KnownAs = user.KnownAs,
            //    }).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await _context.Users
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider) //Projection loads photos for us
                .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                                 .Include(p => p.Photos)
                                 .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
                                  .Include(p => p.Photos)
                                  .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Users.Entry(user).State = EntityState.Modified;
        }
    }
}
