using API.Data;
using API.Dtos;
using API.Helpers;
using API.Interfaces;
using API.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public MessageRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public void AddMessage(Message message)
        {
            _dataContext.Messages.Add(message);
        }

        public void DelMessage(Message message)
        {
            _dataContext.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _dataContext.Messages
                .Include(x => x.Sender)
                .Include(x => x.Recipient)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _dataContext.Messages
                .OrderByDescending(m => m.SendTime)                                  //SendTime(Date) in Messages
                .AsQueryable();
            query = messageParams.Container switch
            {
                "Inbox" => query.Where(x => x.Recipient.UserName == messageParams.UserName),
                "Outbox" => query.Where(x => x.Sender.UserName == messageParams.UserName),
                _ => query.Where(x => x.Recipient.UserName ==
                messageParams.UserName && x.ReadTime == null)                 //ReadTime(Date) in Message 
            };
            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);
            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }


        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            var messages = await _dataContext.Messages
                .Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                           //你接收的信 // 未刪 // 對方寄的來信
                           //你寄出的信 // 未刪 // 對方應應收的來信
                .Where(m => m.Recipient.UserName == currentUsername && m.RecipientDel == false
                        && m.Sender.UserName == recipientUsername
                        || m.Recipient.UserName == recipientUsername
                        && m.Sender.UserName == currentUsername && m.SenderDel == false
                )
                .OrderBy(m => m.SendTime)  //orderby time
                .ToListAsync();

            //to set unread message 
            var unreadMessages = messages.Where(m => m.ReadTime == null
                && m.Recipient.UserName == currentUsername).ToList();

            //loop to be list 
            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.ReadTime = DateTime.Now;
                }

                await _dataContext.SaveChangesAsync();
            }

            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;

        }
    }
}