using Chat.Application.Common;
using Chat.Application.ModelsDto;
using FluentValidation;

namespace Chat.Application.Validations
{
    /// <summary>
    /// Валидация <see cref="MessageDto"/>
    /// </summary>
    public class MessageDtoValidator : AbstractValidator<MessageDto>
    {
        public MessageDtoValidator()
        {
            RuleFor(x => x.Text)
                .Length(ChatConfiguration.MinTextLength, ChatConfiguration.MaxTextLength);
        }
    }
}
