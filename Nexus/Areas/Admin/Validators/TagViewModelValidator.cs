using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Nexus.Areas.Admin.Models;
using Nexus.Service.ServiceInterfaces;

namespace Nexus.Areas.Admin.Validators
{
    public class TagViewModelValidator : AbstractValidator<TagViewModel>
    {
        public TagViewModelValidator(ITagService tagService)
        {
            RuleFor(model => model.Id)
                .NotEmpty()
                .WithMessage("This property is required.");

            RuleFor(t => t.Title).NotEmpty()
                .When(model => string.IsNullOrWhiteSpace(model.Title))
                .WithMessage("Title property can not be empty.");
        }
    }
}
