using Chat.Application.ModelsDto;
using Chat.Application.Validations;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace Chat.Application.Tests.Validations
{
    [TestClass]
    public class MessageDtoValidatorTests
    {
        [TestMethod]
        public void MessageDtoValidator_Must_Valid()
        {
            // Arrange
            MessageDtoValidator messageDtoValidator = new();
            MessageDto messageDto = new() { Text = "123" };

            // Act

            ValidationResult valResult = messageDtoValidator.Validate(messageDto);

            // Assert
            Assert.IsNotNull(valResult);
            Assert.IsTrue(valResult.IsValid);
        }

        [TestMethod]
        public void MessageDtoValidator_Must_Not_Valid()
        {
            // Arrange
            MessageDtoValidator messageDtoValidator = new();
            MessageDto messageDto = new() { Text = "1" };

            // Act

            ValidationResult valResult = messageDtoValidator.Validate(messageDto);

            // Assert
            Assert.IsNotNull(valResult);
            Assert.IsFalse(valResult.IsValid);
        }

        [TestMethod]
        public void MessageDtoValidator_Must_Not_Valid2()
        {
            // Arrange
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("ru");
            MessageDtoValidator messageDtoValidator = new();
            MessageDto messageDto = new() { Text = string.Empty };

            // Act

            ValidationResult valResult = messageDtoValidator.Validate(messageDto);

            // Assert
            Assert.IsNotNull(valResult);
            Assert.IsFalse(valResult.IsValid);
        }
    }
}
