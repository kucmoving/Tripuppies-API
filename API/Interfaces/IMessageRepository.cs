using API.Dtos;
using API.Helpers;
using API.Models;

namespace API.Interfaces
{
    public interface IMessageRepository
    {

        //contract to build 
        void AddMessage(Message message); //contract
        void DelMessage(Message message); //contract


        //get message by id  -
        Task<Message> GetMessage(int id);

        //get message list 
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipentUsername);

        //to create 
        Task<bool> SaveAllAsync();

    }
}
