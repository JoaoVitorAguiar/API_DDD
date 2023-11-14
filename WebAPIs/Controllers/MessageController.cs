using AutoMapper;
using Domain.Interfaces;
using Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIs.Models;

namespace WebAPIs.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly IMapper _IMapper;
    private readonly IMessage _IMessage; // Repsitoty

    public MessageController(IMapper IMapper, IMessage IMessage)
    {
        _IMapper = IMapper;
        _IMessage = IMessage;
    }

    [Authorize]
    [Produces("application/json")]
    [HttpPost("/api/Add")]
    public async Task<List<Notifies>> Add(
        [FromBody] MessageViewModel message)
    {
        message.UserId = await ReturnUserIdLogged();
        var messageMap = _IMapper.Map<Message>(message);
        await _IMessage.Add(messageMap);
        return messageMap.Notifications;
    }

    [Authorize]
    [Produces("application/json")]
    [HttpPut("/api/Update/{int:id}")]
    public async Task<List<Notifies>> Update(
        [FromRoute] int id,
        [FromBody] MessageViewModel messageViewModel)
    {
        var message = await _IMessage.GetEntityById(id);

        message = _IMapper.Map<Message>(messageViewModel);
        await _IMessage.Update(message);
        return message.Notifications;
    }

    [Authorize]
    [Produces("application/json")]
    [HttpPost("/api/Delete/{id:int}")]
    public async Task<List<Notifies>> Delete(
        [FromRoute] int id)
    {
        var message = await _IMessage.GetEntityById(id);
        await _IMessage.Delete(message);
        return message.Notifications;
    }

    [Authorize]
    [Produces("application/json")]
    [HttpPost("/api/GetEntityById/{id:int}")]
    public async Task<MessageViewModel> GetById(
        [FromRoute] int id
        )
    {
        var message = await _IMessage.GetEntityById(id);
        var messageMap = _IMapper.Map<MessageViewModel>(message);
        return messageMap;
    }

    [Authorize]
    [Produces("application/json")]
    [HttpPost("/api/List")]
    public async Task<List<MessageViewModel>> List()
    {
        var mensagens = await _IMessage.List();
        var messageMap = _IMapper.Map<List<MessageViewModel>>(mensagens);
        return messageMap;
    }



    private async Task<string> ReturnUserIdLogged()
    {
        if (User != null)
        {
            var userId = User.FindFirst("UserId");
            return userId.Value;
        }

        return string.Empty;

    }
}
