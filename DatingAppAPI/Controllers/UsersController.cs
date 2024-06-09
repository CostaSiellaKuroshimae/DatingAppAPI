﻿using AutoMapper;
using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Extensions;
using DatingAppAPI.Interaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppAPI.Controllers
{
	[Authorize]
	public class UsersController : BaseApiController
	{
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;
		private readonly IPhotoService _photoService;

		public UsersController(IUserRepository userRepository,
			IMapper mapper,
			IPhotoService photoService)
		{
			_userRepository = userRepository;
			_mapper = mapper;
			this._photoService = photoService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
		{

			//var users =  await _userRepository.GetUsersAsync();
			//var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
			var users = await _userRepository.GetMembersAsync();
			return Ok(users);
		}

		[HttpGet("{username}")] //api/users/2
		public async Task<ActionResult<MemberDto>> GetUser(string username)
		{
			return await _userRepository.GetMemberAsync(username);
		}

		[HttpPut] //Username comes from token
		public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
		{
			var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
			if (user == null) { return NotFound(); }

			_mapper.Map(memberUpdateDto, user);

			if (await _userRepository.SaveAllAsync()) return NoContent();

			return BadRequest("Failed to update user");
		}

		[HttpPost("add-photo")]
		public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
		{
			var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
			if (user == null) { return NotFound(); }

			var result = await _photoService.AddPhotoAsync(file);

			if (result.Error != null) { return BadRequest(result.Error); }

			var photo = new Photo
			{
				Url = result.SecureUrl.AbsoluteUri,
				PublicId = result.PublicId,
			};

			if (user.Photos.Count == 0)
			{
				photo.IsMain = true;
			};

			if (await _userRepository.SaveAllAsync())
			{
				return _mapper.Map<PhotoDto>(photo);
			};

			return BadRequest("Problem adding photo");
		}

	}
}
