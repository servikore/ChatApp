using Application.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using System.Threading.Tasks;

namespace Application.DomainServices
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string email, string password);
        Task<User> RegisterAsync(User user);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<User> RegisterAsync(User user)
        {
            validate(user);
            await this.userRepository.InsertAsync(user);
            //TODO: Encrypt password.
            await this.userRepository.SaveAsync();

            return user;
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                throw new AuthenticationException();

            var user = await this.userRepository.GetByEmailAsync(email);
            //TODO: Decrypt password.
            if (user == null || user.Password != password)
                throw new AuthenticationException();
            else
                return user;
        }

        private void validate(User user)
        {
            if (user == null
                || string.IsNullOrEmpty(user.Email)
                || string.IsNullOrEmpty(user.NickName)
                || string.IsNullOrEmpty(user.Password))
            {
                throw new InvalidUserException() { User = user };
            }
        }
    }
}
