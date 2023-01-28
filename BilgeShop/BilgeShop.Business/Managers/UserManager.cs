using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.Business.Types;
using BilgeShop.Data.Entities;
using BilgeShop.Data.Repositories;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Business.Managers
{
    public class UserManager : IUserService
    {
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IDataProtector _dataProtector;
        public UserManager(IRepository<UserEntity> userRepository , IDataProtectionProvider dataProtectionProvider)
        {
            _userRepository = userRepository;
            _dataProtector = dataProtectionProvider.CreateProtector("security");
        }

        public ServiceMessage AddUser(AddUserDto addUserDto)
        {
            var hasMail = _userRepository.GetAll(x => x.Email.ToLower() == addUserDto.Email.ToLower()).ToList();

            if(hasMail.Any()) // hasMail.Count != 0 
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu mail adresi ile kayıtlı bir kullanıcı zaten mevcut."
                };
            }

            var encryptedPassword = _dataProtector.Protect(addUserDto.Password);

            var userEntity = new UserEntity()
            {
                FirstName = addUserDto.FirstName,
                LastName = addUserDto.LastName,
                Email = addUserDto.Email,
                Password = encryptedPassword,
                UserType = Data.Enums.UserTypeEnum.User
            };

            _userRepository.Add(userEntity);

            return new ServiceMessage
            {
                IsSucceed = true
            };

        }

        public UserDto Login(LoginDto loginDto)
        {
            var user = _userRepository.Get(x => x.Email.ToLower() == loginDto.Email.ToLower());

            if(user is null)
            {
                return null;
            }

            var rawPassword = _dataProtector.Unprotect(user.Password);

            if(rawPassword != loginDto.Password)
            {
                return null;
            }
            else
            {
                return new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserType = user.UserType
                };

                // Eğer forma girilence Email ve Şifre ile eşleşen bir veri bulunduysa, oturum açılacağı için, bu nesnenin bilgilerini view'e geri gönderiyorum. Tarayıcıda cookie adı verilen dosyalarda saklayacağım.

            }
        }

        public void UpdateUser(UserEditDto userEditDto)
        {
            var entity = _userRepository.GetById(userEditDto.Id);

            entity.FirstName = userEditDto.FirstName;
            entity.LastName = userEditDto.LastName;
            entity.Email = userEditDto.Email;

            _userRepository.Update(entity);
        }
    }
}

// Repository'i dependency injection ile eklediğim için , bu class içerisinde artık repository metotlarını kullanabilirim.