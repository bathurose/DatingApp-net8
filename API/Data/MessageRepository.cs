using API.DTOs;
using API.Entities;
using API.Helper;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository(DataContext dataContext, IMapper mapper) : IMessageRepository
    {
      

        public void AddMessage(Message message)
        {
            dataContext.Messages.Add(message);   
        }

        public void DeleteMessage(Message message)
        {
            dataContext.Messages.Remove(message);
        }

       
        public async Task<Message?> GetMessage(int id)
        {
            return await dataContext.Messages.FindAsync(id);
        }

        public async Task<PageList<MessageDto>> GetMessageForUser(MessageParams messageParams)
        {
            var query = dataContext.Messages
                .OrderBy(x => x.MessageSent)
                .AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(x => x.RecipientUserName == messageParams.UserName && x.RecipientDeleted == false),
                "Outbox" => query.Where(x => x.SenderUserName == messageParams.UserName && x.SenderDeleted == false),
                _ => query.Where(x => x.RecipientUserName == messageParams.UserName && x.DateRead == null
                && x.RecipientDeleted == false),
            };

            var messages = query.ProjectTo<MessageDto>(mapper.ConfigurationProvider);

            return await PageList<MessageDto>.CreatAsync(messages, messageParams.PageNumber,
                messageParams.PageSize);
        }

      

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientUserName)
        {
            var messages =  await dataContext.Messages
                 
                  .Where(x =>
                  x.RecipientUserName == currentUserName 
                  && x.RecipientDeleted == false 
                  && x.SenderUserName == recipientUserName
                  || x.SenderUserName == currentUserName 
                  && x.SenderDeleted == false 
                  && x.RecipientUserName == recipientUserName
                  ).OrderBy(x => x.MessageSent)
                  .ProjectTo<MessageDto>(mapper.ConfigurationProvider)
                  .ToListAsync();

            var unreadMessage = messages.Where(x=>x.DateRead == null && 
            x.RecipientUserName ==currentUserName).ToList();

            if(unreadMessage.Count > 0) {
                unreadMessage.ForEach(x => x.DateRead = DateTime.UtcNow);
                
            }
            return messages;
        }
        public void AddGroup(Group group)
        {
            dataContext.Groups.Add(group);
        }
        public async Task<Connection?> GetConnection(string connectionId)
        {
            return await dataContext.Connections.FindAsync(connectionId);
        }

        public void RemoveConnection(Connection connection)
        {
            dataContext.Connections.Remove(connection);
        }
        public  async Task<Group?> GetMessageGroup(string groupName)
        {
            return await dataContext.Groups.Include(x => x.Connections)
                                            .FirstOrDefaultAsync(x => x.Name == groupName);
        }


        public async Task<Group?> GetGroupForConnection(string connectionId)
        {
            return await dataContext.Groups
            .Include(x =>x.Connections)
            .Where(x=>x.Connections.Any(c=>c.ConntectionId == connectionId))
            .FirstOrDefaultAsync();
        }
    }
}
