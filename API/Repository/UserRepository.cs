﻿using API.Data;
using API.Dtos;
using API.Helpers;
using API.Interfaces;
using API.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _imapper;

        public UserRepository(DataContext dataContext, IMapper imapper)
        {
            _dataContext = dataContext;
            _imapper = imapper;
        }
        public async Task<PuppyDto> GetPuppyAsync(string username)
        {
            return await _dataContext.Users
                .Where(x => x.UserName == username)
                .ProjectTo<PuppyDto>(_imapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }
        //using project to map, configurationprovider imapper built in function
        //but is it good for testing or others to read??


        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _dataContext.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _dataContext.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _dataContext.Users
                .Include(p => p.Photos)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _dataContext.Entry(user).State = EntityState.Modified; //using built in function to change state
        }

        public async Task<PagedList<PuppyDto>> GetPuppiesAsync(UserParams userParams)
        {
            var query = _dataContext.Users.AsQueryable();

            //no user
            query = query.Where(u => u.UserName != userParams.CurrentUsername);
            //same role 
            query = query.Where(u => u.Role == userParams.Role);
            //experience 
            query = query.Where(u => u.Experience >= userParams.MinExp && u.Experience <= userParams.MaxExp);

            // using switch to do simple filtering
            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(x => x.Created),
                _ => query.OrderByDescending(u => u.LastTime)
            };

            //asnotracking, to prevent 2 query same time ==> error
            return await PagedList<PuppyDto>.CreateAsync(query.ProjectTo<PuppyDto>(_imapper
                .ConfigurationProvider).AsNoTracking(),
                    userParams.PageNumber, userParams.PageSize);
        }
    }
}