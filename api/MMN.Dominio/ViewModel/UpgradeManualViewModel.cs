using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class UpgradeManualViewModel
    {
        public Guid IdUsuario { get; set; }
        public int IdProduto { get; set; }
    }

    public class UpgradeManualViewModelValidator : AbstractValidator<UpgradeManualViewModel>
    {
        public UpgradeManualViewModelValidator()
        {
            RuleFor(u => u.IdUsuario).NotNull();
            RuleFor(u => u.IdProduto).NotNull();
        }
    }
}
