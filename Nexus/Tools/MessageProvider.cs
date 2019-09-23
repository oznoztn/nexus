using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Nexus.Tools
{
    public enum OperationType
    {
        Create,
        Update,
        Delete
    }

    public interface IMessageProvider
    {
        string SuccessMessage(OperationType status, string placeholder);
        string FailMessage(OperationType operation, string placeholder);
    }

    public class MessageProvider : IMessageProvider
    {
        public string GenericSuccessfulCreationMessage()
        {
            return "The item has been created!";
        }

        public string SuccessMessage(OperationType status, string placeholder)
        {
            switch (status)
            {
                case OperationType.Create:
                    return $"The {placeholder} has been created successfully!";
                case OperationType.Update:
                    return $"The {placeholder} has been updated successfully!";
                case OperationType.Delete:
                    return $"The {placeholder} has been deleted successfully!";
                default:
                    return "The operation was successful!";
            }
        }

        public string FailMessage(OperationType operation, string placeholder)
        {
            switch (operation)
            {
                case OperationType.Create:
                    return $"The {placeholder} was not created. Something went wrong. Please try again.";
                case OperationType.Update:
                    return $"The {placeholder} was not updated. Something went wrong. Please try again.";
                case OperationType.Delete:
                    return $"The {placeholder} was not deleted. Something went wrong. Please try again.";
                default:
                    return "The operation was successful!";
            }
        }

        public string SuccessfulCreationMessage(string placeholder)
        {
            return $"The {placeholder} has been created!";
        }

        public string SuccessfulCreationMessage(string placeholder, string message)
        {
            return $"The {placeholder} has been created! {message}";
        }

        public string GenericSuccessfulUpdateMessage()
        {
            return "The item has been updated!";
        }

        public string SuccessfulUpdateMessage(string placeholder)
        {
            return $"The {placeholder} has been updated!";
        }
    }
}
